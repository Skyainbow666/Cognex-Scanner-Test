using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using NModbus;

namespace ScannerTool
{
    /// <summary>
    /// Cognex 扫码器通信类，通过 Modbus TCP 协议控制扫码器
    /// </summary>
    public class CognexScanner
    {
        // 常量定义
        private const int DefaultTimeout = 2000;        // 默认连接超时时间（毫秒）
        private const int DecodeDelay = 100;            // 解码等待时间（毫秒）
        private const ushort StartAddress = 2000;       // Modbus 寄存器起始地址
        private const ushort TriggerCoil = 0;           // 触发线圈地址
        private const byte DefaultSlaveId = 1;          // 默认 Slave ID

        // 私有字段（只读，确保不可变性）
        private readonly string _ipAddress;             // 扫码器 IP 地址
        private readonly int _port;                     // 扫码器端口号
        private readonly byte _slaveId;                 // Modbus Slave ID

        /// <summary>
        /// 构造函数，初始化扫码器实例
        /// </summary>
        /// <param name="ipAddress">扫码器 IP 地址</param>
        /// <param name="port">扫码器端口号</param>
        /// <param name="slaveId">Modbus Slave ID，默认为 1</param>
        /// <exception cref="ArgumentNullException">当 ipAddress 为 null 时抛出</exception>
        public CognexScanner(string ipAddress, int port, byte slaveId = DefaultSlaveId)
        {
            _ipAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
            _port = port;
            _slaveId = slaveId;
        }

        /// <summary>
        /// 执行触发扫码并读取条码数据
        /// </summary>
        /// <returns>条码字符串或错误信息</returns>
        public string ExecuteTriggerAndRead()
        {
            try
            {
                // 创建 TCP 客户端连接
                using (var client = new TcpClient())
                {
                    // 尝试连接扫码器，带超时
                    if (!ConnectWithTimeout(client, DefaultTimeout))
                    {
                        return "通信异常: 连接读码器超时";
                    }

                    // 创建 Modbus 主站
                    var factory = new ModbusFactory();
                    var master = factory.CreateMaster(client);

                    try
                    {
                        // 1. 触发扫码
                        TriggerScanner(master);
                        
                        // 2. 等待解码完成
                        Thread.Sleep(DecodeDelay);

                        // 3. 读取头部信息（状态码和数据长度）
                        var (isReadSuccess, dataLength) = ReadHeader(master);
                        
                        // 4. 复位触发线圈
                        ResetTrigger(master);

                        // 5. 检查是否成功读取
                        if (!isReadSuccess || dataLength == 0)
                        {
                            return "No Read";
                        }

                        // 6. 读取条码原始数据
                        var rawData = ReadBarcodeData(master, dataLength);
                        
                        // 7. 转换为 ASCII 字符串
                        return ConvertRegistersToAscii(rawData, dataLength);
                    }
                    finally
                    {
                        // 确保释放 Modbus 主站资源
                        master?.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                // 捕获所有异常并返回友好的错误信息
                return $"通信异常: {ex.Message}";
            }
        }

        /// <summary>
        /// 带超时的 TCP 连接
        /// </summary>
        /// <param name="client">TCP 客户端实例</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>连接是否成功</returns>
        private bool ConnectWithTimeout(TcpClient client, int timeoutMs)
        {
            // 开始异步连接
            var asyncResult = client.BeginConnect(_ipAddress, _port, null, null);
            
            // 等待连接完成或超时
            var success = asyncResult.AsyncWaitHandle.WaitOne(timeoutMs);
            
            if (success)
            {
                // 完成连接
                client.EndConnect(asyncResult);
            }
            
            return success;
        }

        /// <summary>
        /// 触发扫码器拍照
        /// </summary>
        /// <param name="master">Modbus 主站实例</param>
        private void TriggerScanner(IModbusMaster master)
        {
            // 向触发线圈写入 true
            master.WriteSingleCoil(_slaveId, TriggerCoil, true);
        }

        /// <summary>
        /// 复位触发线圈
        /// </summary>
        /// <param name="master">Modbus 主站实例</param>
        private void ResetTrigger(IModbusMaster master)
        {
            // 向触发线圈写入 false，准备下次触发
            master.WriteSingleCoil(_slaveId, TriggerCoil, false);
        }

        /// <summary>
        /// 读取头部寄存器信息
        /// </summary>
        /// <param name="master">Modbus 主站实例</param>
        /// <returns>元组，包含是否读取成功和数据长度</returns>
        private (bool isReadSuccess, ushort dataLength) ReadHeader(IModbusMaster master)
        {
            // 读取起始地址开始的 5 个寄存器
            ushort[] header = master.ReadInputRegisters(_slaveId, StartAddress, 5);
            
            // 第 4 个寄存器（索引 3）是结果状态码
            ushort resultCode = header[3];
            
            // 第 5 个寄存器（索引 4）是数据字节长度
            ushort dataLength = header[4];
            
            // 检查状态码的第 0 位是否为 1（表示读取成功）
            bool isReadSuccess = (resultCode & 0x0001) == 1;
            
            return (isReadSuccess, dataLength);
        }

        /// <summary>
        /// 读取条码数据寄存器
        /// </summary>
        /// <param name="master">Modbus 主站实例</param>
        /// <param name="dataLength">数据字节长度</param>
        /// <returns>原始寄存器数据数组</returns>
        private ushort[] ReadBarcodeData(IModbusMaster master, ushort dataLength)
        {
            // 计算需要读取的寄存器数量（每个寄存器 2 字节）
            ushort wordsToRead = (ushort)((dataLength + 1) / 2);
            
            // 从起始地址 + 5 的位置开始读取数据
            return master.ReadInputRegisters(_slaveId, (ushort)(StartAddress + 5), wordsToRead);
        }

        /// <summary>
        /// 将 Modbus 寄存器数据转换为 ASCII 字符串
        /// </summary>
        /// <param name="registers">寄存器数据数组</param>
        /// <param name="byteLength">实际字节长度</param>
        /// <returns>条码字符串</returns>
        private string ConvertRegistersToAscii(ushort[] registers, int byteLength)
        {
            // 创建字节数组（每个寄存器 2 字节）
            byte[] finalBytes = new byte[registers.Length * 2];
            int byteIndex = 0;

            // 遍历寄存器，按大端序（高字节在前）转换为字节
            for (int i = 0; i < registers.Length && byteIndex < byteLength; i++)
            {
                // 高字节（寄存器值右移 8 位）
                finalBytes[byteIndex++] = (byte)(registers[i] >> 8);
                
                // 低字节（寄存器值与 0xFF 进行按位与）
                if (byteIndex < byteLength)
                {
                    finalBytes[byteIndex++] = (byte)(registers[i] & 0xFF);
                }
            }

            // 将字节数组转换为 ASCII 字符串
            string barcode = Encoding.ASCII.GetString(finalBytes, 0, byteLength);
            
            // 去除换行符和首尾空白字符
            return barcode.Replace("\r", "").Replace("\n", "").Trim();
        }
    }
}