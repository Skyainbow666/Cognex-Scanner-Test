
namespace ScannerTool
{
    partial class Form_Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtLogOutput = new System.Windows.Forms.TextBox();
            this.btnTrigger = new System.Windows.Forms.Button();
            this.IP地址 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Location = new System.Drawing.Point(564, 69);
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(146, 21);
            this.txtIpAddress.TabIndex = 0;
            this.txtIpAddress.Text = "192.168.3.31";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(564, 116);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(146, 21);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "25";
            // 
            // txtLogOutput
            // 
            this.txtLogOutput.Location = new System.Drawing.Point(57, 55);
            this.txtLogOutput.Multiline = true;
            this.txtLogOutput.Name = "txtLogOutput";
            this.txtLogOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLogOutput.Size = new System.Drawing.Size(403, 315);
            this.txtLogOutput.TabIndex = 2;
            // 
            // btnTrigger
            // 
            this.btnTrigger.Location = new System.Drawing.Point(576, 234);
            this.btnTrigger.Name = "btnTrigger";
            this.btnTrigger.Size = new System.Drawing.Size(110, 41);
            this.btnTrigger.TabIndex = 3;
            this.btnTrigger.Text = "扫码";
            this.btnTrigger.UseVisualStyleBackColor = true;
            this.btnTrigger.Click += new System.EventHandler(this.btnTrigger_Click);
            // 
            // IP地址
            // 
            this.IP地址.AutoSize = true;
            this.IP地址.Location = new System.Drawing.Point(489, 78);
            this.IP地址.Name = "IP地址";
            this.IP地址.Size = new System.Drawing.Size(53, 12);
            this.IP地址.TabIndex = 4;
            this.IP地址.Text = "IP地址：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(489, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "端口号：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "结果：";
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.IP地址);
            this.Controls.Add(this.btnTrigger);
            this.Controls.Add(this.txtLogOutput);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIpAddress);
            this.Name = "Form_Main";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIpAddress;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtLogOutput;
        private System.Windows.Forms.Button btnTrigger;
        private System.Windows.Forms.Label IP地址;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

