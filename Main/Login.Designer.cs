namespace Main
{
    partial class Login
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
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.textBox_Name = new System.Windows.Forms.TextBox();
            this.textBox_PassWord = new System.Windows.Forms.TextBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.timeLable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(658, 596);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(105, 34);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(837, 596);
            this.btnQuit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(105, 34);
            this.btnQuit.TabIndex = 3;
            this.btnQuit.Text = "退出";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.BtnQuit_Click);
            // 
            // textBox_Name
            // 
            this.textBox_Name.Location = new System.Drawing.Point(658, 440);
            this.textBox_Name.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_Name.Name = "textBox_Name";
            this.textBox_Name.Size = new System.Drawing.Size(282, 28);
            this.textBox_Name.TabIndex = 0;
            this.textBox_Name.Visible = false;
            // 
            // textBox_PassWord
            // 
            this.textBox_PassWord.Location = new System.Drawing.Point(658, 519);
            this.textBox_PassWord.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_PassWord.Name = "textBox_PassWord";
            this.textBox_PassWord.PasswordChar = '*';
            this.textBox_PassWord.Size = new System.Drawing.Size(282, 28);
            this.textBox_PassWord.TabIndex = 1;
            // 
            // cbType
            // 
            this.cbType.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "调试员",
            "技术员",
            "管理员",
            "操作员"});
            this.cbType.Location = new System.Drawing.Point(658, 440);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(282, 36);
            this.cbType.TabIndex = 4;
            // 
            // timeLable
            // 
            this.timeLable.AutoSize = true;
            this.timeLable.BackColor = System.Drawing.Color.Transparent;
            this.timeLable.Location = new System.Drawing.Point(655, 419);
            this.timeLable.Name = "timeLable";
            this.timeLable.Size = new System.Drawing.Size(0, 18);
            this.timeLable.TabIndex = 5;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Main.Properties.Resources.loginLogo;
            this.ClientSize = new System.Drawing.Size(1539, 1106);
            this.Controls.Add(this.timeLable);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.textBox_PassWord);
            this.Controls.Add(this.textBox_Name);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.TextBox textBox_Name;
        private System.Windows.Forms.TextBox textBox_PassWord;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label timeLable;
    }
}