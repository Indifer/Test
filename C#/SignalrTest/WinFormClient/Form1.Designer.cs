namespace WinFormClient
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
            this.btnCon = new System.Windows.Forms.Button();
            this.lblUrl = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.txtparams1 = new System.Windows.Forms.TextBox();
            this.txtparams2 = new System.Windows.Forms.TextBox();
            this.btnInvok = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRes = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCon
            // 
            this.btnCon.Location = new System.Drawing.Point(324, 46);
            this.btnCon.Name = "btnCon";
            this.btnCon.Size = new System.Drawing.Size(75, 23);
            this.btnCon.TabIndex = 0;
            this.btnCon.Text = "连接";
            this.btnCon.UseVisualStyleBackColor = true;
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(44, 51);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(29, 12);
            this.lblUrl.TabIndex = 1;
            this.lblUrl.Text = "Url:";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(92, 46);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(226, 21);
            this.txtUrl.TabIndex = 2;
            this.txtUrl.Text = "http://114.215.241.141:9001/";
            // 
            // txtparams1
            // 
            this.txtparams1.Location = new System.Drawing.Point(92, 139);
            this.txtparams1.Name = "txtparams1";
            this.txtparams1.Size = new System.Drawing.Size(102, 21);
            this.txtparams1.TabIndex = 2;
            this.txtparams1.Text = "abc";
            // 
            // txtparams2
            // 
            this.txtparams2.Location = new System.Drawing.Point(92, 178);
            this.txtparams2.Name = "txtparams2";
            this.txtparams2.Size = new System.Drawing.Size(102, 21);
            this.txtparams2.TabIndex = 2;
            this.txtparams2.Text = "xyz";
            // 
            // btnInvok
            // 
            this.btnInvok.Location = new System.Drawing.Point(92, 217);
            this.btnInvok.Name = "btnInvok";
            this.btnInvok.Size = new System.Drawing.Size(75, 23);
            this.btnInvok.TabIndex = 0;
            this.btnInvok.Text = "调用";
            this.btnInvok.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 142);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "参数1:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "参数2:";
            // 
            // txtRes
            // 
            this.txtRes.Location = new System.Drawing.Point(92, 257);
            this.txtRes.Multiline = true;
            this.txtRes.Name = "txtRes";
            this.txtRes.ReadOnly = true;
            this.txtRes.Size = new System.Drawing.Size(226, 92);
            this.txtRes.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "NewContosoChatMessage";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 470);
            this.Controls.Add(this.txtRes);
            this.Controls.Add(this.txtparams2);
            this.Controls.Add(this.txtparams1);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.btnInvok);
            this.Controls.Add(this.btnCon);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCon;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.TextBox txtparams1;
        private System.Windows.Forms.TextBox txtparams2;
        private System.Windows.Forms.Button btnInvok;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRes;
        private System.Windows.Forms.Label label3;
    }
}

