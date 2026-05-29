using System;
using System.Windows.Forms;
using NModbus;

namespace ScannerTool
{
    /// <summary>
    /// 主窗体类，提供扫码器控制界面
    /// </summary>
    public partial class Form_Main : Form
    {
        /// <summary>
        /// 构造函数，初始化窗体并绑定按钮事件
        /// </summary>
        public Form_Main()
        {
            InitializeComponent();
            // 绑定扫码按钮的点击事件
            btnTrigger.Click += btnTrigger_Click;
        }

        /// <summary>
        /// 扫码按钮点击事件处理程序
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void btnTrigger_Click(object sender, EventArgs e)
        {
            // 获取用户输入的IP地址和端口号
            string ip = txtIpAddress.Text.Trim();
            string portStr = txtPort.Text.Trim();

            // 验证输入是否有效
            if (string.IsNullOrEmpty(ip) || !int.TryParse(portStr, out int port))
            {
                AppendLog("【错误】请输入有效的 IP 地址和网络端口号！");
                return;
            }

            // 禁用按钮，防止重复点击
            btnTrigger.Enabled = false;
            AppendLog($"【命令】[{DateTime.Now:HH:mm:ss}] 发送读码触发指令...");

            try
            {
                // 创建扫码器实例并执行扫码操作
                CognexScanner scanner = new CognexScanner(ip, port);
                string result = scanner.ExecuteTriggerAndRead();

                // 根据返回结果显示不同的日志信息
                if (result == "No Read")
                {
                    AppendLog($"【状态】[{DateTime.Now:HH:mm:ss}] 读码失败 (No Read)");
                }
                else if (result.StartsWith("通信异常"))
                {
                    AppendLog($"【异常】[{DateTime.Now:HH:mm:ss}] {result}");
                }
                else
                {
                    AppendLog($"【成功】[{DateTime.Now:HH:mm:ss}] 扫码数据 -> {result}");
                }
            }
            catch (Exception ex)
            {
                // 捕获并显示系统异常
                AppendLog($"【系统错误】: {ex.Message}");
            }
            finally
            {
                // 无论成功或失败，都恢复按钮可用状态
                btnTrigger.Enabled = true;
            }
        }

        /// <summary>
        /// 向日志文本框追加消息，支持跨线程调用
        /// </summary>
        /// <param name="message">要显示的日志消息</param>
        private void AppendLog(string message)
        {
            // 检查是否需要跨线程调用
            if (txtLogOutput.InvokeRequired)
            {
                // 如果需要，使用 Invoke 方法在 UI 线程上执行
                txtLogOutput.Invoke(new Action<string>(AppendLog), message);
                return;
            }

            // 直接追加文本并换行
            txtLogOutput.AppendText(message + Environment.NewLine);
        }
    }
}