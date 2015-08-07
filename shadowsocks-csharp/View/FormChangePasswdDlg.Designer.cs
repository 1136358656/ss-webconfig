namespace Shadowsocks.View
{
    partial class FormChangePasswdDlg
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.oldpasswd = new System.Windows.Forms.TextBox();
            this.newpasswd = new System.Windows.Forms.TextBox();
            this.confirmnewpasswd = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "原密码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "新密码";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "确认新密码";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(91, 6);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(188, 25);
            this.username.TabIndex = 4;
            // 
            // oldpasswd
            // 
            this.oldpasswd.Location = new System.Drawing.Point(91, 35);
            this.oldpasswd.Name = "oldpasswd";
            this.oldpasswd.PasswordChar = '●';
            this.oldpasswd.Size = new System.Drawing.Size(188, 25);
            this.oldpasswd.TabIndex = 5;
            this.oldpasswd.UseSystemPasswordChar = true;
            // 
            // newpasswd
            // 
            this.newpasswd.Location = new System.Drawing.Point(91, 64);
            this.newpasswd.Name = "newpasswd";
            this.newpasswd.PasswordChar = '●';
            this.newpasswd.Size = new System.Drawing.Size(188, 25);
            this.newpasswd.TabIndex = 6;
            this.newpasswd.UseSystemPasswordChar = true;
            // 
            // confirmnewpasswd
            // 
            this.confirmnewpasswd.Location = new System.Drawing.Point(90, 93);
            this.confirmnewpasswd.Name = "confirmnewpasswd";
            this.confirmnewpasswd.PasswordChar = '●';
            this.confirmnewpasswd.Size = new System.Drawing.Size(188, 25);
            this.confirmnewpasswd.TabIndex = 7;
            this.confirmnewpasswd.UseSystemPasswordChar = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(27, 127);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(113, 35);
            this.button2.TabIndex = 9;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(155, 127);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(113, 35);
            this.button3.TabIndex = 10;
            this.button3.Text = "确定";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FormChangePasswdDlg
            // 
            this.AcceptButton = this.button3;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 169);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.confirmnewpasswd);
            this.Controls.Add(this.newpasswd);
            this.Controls.Add(this.oldpasswd);
            this.Controls.Add(this.username);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormChangePasswdDlg";
            this.Opacity = 0.85D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改密码";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox oldpasswd;
        private System.Windows.Forms.TextBox newpasswd;
        private System.Windows.Forms.TextBox confirmnewpasswd;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}