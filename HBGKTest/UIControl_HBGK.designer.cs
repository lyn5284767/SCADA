namespace HBGKTest
{
    partial class UIControl_HBGK
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.VideoPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // VideoPanel
            // 
            this.VideoPanel.BackColor = System.Drawing.SystemColors.Desktop;
            this.VideoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VideoPanel.Location = new System.Drawing.Point(0, 0);
            this.VideoPanel.Name = "VideoPanel";
            this.VideoPanel.Size = new System.Drawing.Size(543, 501);
            this.VideoPanel.TabIndex = 0;
            this.VideoPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.VideoPanel_MouseDoubleClick);
            this.VideoPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.VideoPanel_MouseClick);
            this.VideoPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VideoPanel_MouseDown);
            this.VideoPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.VideoPanel_MouseUp);
            // 
            // UIControl_Hik
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.VideoPanel);
            this.Name = "UIControl_Hik";
            this.Size = new System.Drawing.Size(543, 501);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel VideoPanel;
    }
}
