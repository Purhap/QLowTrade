namespace QlowTrade
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ConnectBt = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.disconnectBt = new System.Windows.Forms.Button();
            this.getHistoryDataBt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConnectBt
            // 
            this.ConnectBt.Location = new System.Drawing.Point(402, 57);
            this.ConnectBt.Name = "ConnectBt";
            this.ConnectBt.Size = new System.Drawing.Size(75, 23);
            this.ConnectBt.TabIndex = 0;
            this.ConnectBt.Text = "Connect";
            this.ConnectBt.UseVisualStyleBackColor = true;
            this.ConnectBt.Click += new System.EventHandler(this.ConnectBt_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(288, 62);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(41, 12);
            this.statusLabel.TabIndex = 1;
            this.statusLabel.Text = "status";
            // 
            // disconnectBt
            // 
            this.disconnectBt.Location = new System.Drawing.Point(402, 124);
            this.disconnectBt.Name = "disconnectBt";
            this.disconnectBt.Size = new System.Drawing.Size(75, 23);
            this.disconnectBt.TabIndex = 2;
            this.disconnectBt.Text = "Disconnect";
            this.disconnectBt.UseVisualStyleBackColor = true;
            this.disconnectBt.Click += new System.EventHandler(this.disconnectBt_Click);
            // 
            // getHistoryDataBt
            // 
            this.getHistoryDataBt.Location = new System.Drawing.Point(344, 180);
            this.getHistoryDataBt.Name = "getHistoryDataBt";
            this.getHistoryDataBt.Size = new System.Drawing.Size(161, 23);
            this.getHistoryDataBt.TabIndex = 3;
            this.getHistoryDataBt.Text = "Get History Data";
            this.getHistoryDataBt.UseVisualStyleBackColor = true;
            this.getHistoryDataBt.Click += new System.EventHandler(this.getHistoryDataBt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 261);
            this.Controls.Add(this.getHistoryDataBt);
            this.Controls.Add(this.disconnectBt);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.ConnectBt);
            this.Name = "Form1";
            this.Text = "QlowTrade";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConnectBt;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button disconnectBt;
        private System.Windows.Forms.Button getHistoryDataBt;
    }
}

