namespace SocketClient_SendImage
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
            this.btn_send = new System.Windows.Forms.Button();
            this.btm_scane = new System.Windows.Forms.Button();
            this.lab_path = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(316, 199);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(75, 23);
            this.btn_send.TabIndex = 0;
            this.btn_send.Text = "发送";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // btm_scane
            // 
            this.btm_scane.Location = new System.Drawing.Point(296, 118);
            this.btm_scane.Name = "btm_scane";
            this.btm_scane.Size = new System.Drawing.Size(125, 23);
            this.btm_scane.TabIndex = 1;
            this.btm_scane.Text = "浏览";
            this.btm_scane.UseVisualStyleBackColor = true;
            this.btm_scane.Click += new System.EventHandler(this.btm_scane_Click);
            // 
            // lab_path
            // 
            this.lab_path.AutoSize = true;
            this.lab_path.Location = new System.Drawing.Point(68, 60);
            this.lab_path.Name = "lab_path";
            this.lab_path.Size = new System.Drawing.Size(65, 12);
            this.lab_path.TabIndex = 2;
            this.lab_path.Text = "未发送图片";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 433);
            this.Controls.Add(this.lab_path);
            this.Controls.Add(this.btm_scane);
            this.Controls.Add(this.btn_send);
            this.Name = "Form1";
            this.Text = "Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.Button btm_scane;
        private System.Windows.Forms.Label lab_path;
    }
}

