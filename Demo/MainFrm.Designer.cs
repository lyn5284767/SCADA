namespace Demo
{
    partial class MainFrm
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
            this.components = new System.ComponentModel.Container();
            this.btn_SelectFile = new System.Windows.Forms.Button();
            this.textBoxFile = new System.Windows.Forms.TextBox();
            this.grpBoxSimulator = new System.Windows.Forms.GroupBox();
            this.labelSendCnt = new System.Windows.Forms.Label();
            this.btn_HandSingle = new System.Windows.Forms.Button();
            this.btn_TimerAuto = new System.Windows.Forms.Button();
            this.btn_prePareFile = new System.Windows.Forms.Button();
            this.groupBoxRoles = new System.Windows.Forms.GroupBox();
            this.gridViewUserList = new System.Windows.Forms.DataGridView();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PassWord = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RoleID = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PhoneNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindSourceUser = new System.Windows.Forms.BindingSource(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDeleUser = new System.Windows.Forms.Button();
            this.treeViewRoles = new System.Windows.Forms.TreeView();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.grpBoxSimulator.SuspendLayout();
            this.groupBoxRoles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewUserList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindSourceUser)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_SelectFile
            // 
            this.btn_SelectFile.Location = new System.Drawing.Point(446, 20);
            this.btn_SelectFile.Name = "btn_SelectFile";
            this.btn_SelectFile.Size = new System.Drawing.Size(91, 23);
            this.btn_SelectFile.TabIndex = 4;
            this.btn_SelectFile.Text = "选择回放文件";
            this.btn_SelectFile.UseVisualStyleBackColor = true;
            this.btn_SelectFile.Click += new System.EventHandler(this.Btn_SelectFile_Click);
            // 
            // textBoxFile
            // 
            this.textBoxFile.Location = new System.Drawing.Point(15, 20);
            this.textBoxFile.Name = "textBoxFile";
            this.textBoxFile.Size = new System.Drawing.Size(425, 21);
            this.textBoxFile.TabIndex = 5;
            // 
            // grpBoxSimulator
            // 
            this.grpBoxSimulator.Controls.Add(this.labelSendCnt);
            this.grpBoxSimulator.Controls.Add(this.btn_HandSingle);
            this.grpBoxSimulator.Controls.Add(this.btn_TimerAuto);
            this.grpBoxSimulator.Controls.Add(this.btn_prePareFile);
            this.grpBoxSimulator.Controls.Add(this.textBoxFile);
            this.grpBoxSimulator.Controls.Add(this.btn_SelectFile);
            this.grpBoxSimulator.Location = new System.Drawing.Point(9, 335);
            this.grpBoxSimulator.Name = "grpBoxSimulator";
            this.grpBoxSimulator.Size = new System.Drawing.Size(829, 103);
            this.grpBoxSimulator.TabIndex = 6;
            this.grpBoxSimulator.TabStop = false;
            this.grpBoxSimulator.Text = "数据回放";
            // 
            // labelSendCnt
            // 
            this.labelSendCnt.AutoSize = true;
            this.labelSendCnt.Location = new System.Drawing.Point(19, 63);
            this.labelSendCnt.Name = "labelSendCnt";
            this.labelSendCnt.Size = new System.Drawing.Size(65, 12);
            this.labelSendCnt.TabIndex = 7;
            this.labelSendCnt.Text = "已发送帧数";
            // 
            // btn_HandSingle
            // 
            this.btn_HandSingle.Enabled = false;
            this.btn_HandSingle.Location = new System.Drawing.Point(273, 59);
            this.btn_HandSingle.Name = "btn_HandSingle";
            this.btn_HandSingle.Size = new System.Drawing.Size(75, 23);
            this.btn_HandSingle.TabIndex = 6;
            this.btn_HandSingle.Text = "逐帧";
            this.btn_HandSingle.UseVisualStyleBackColor = true;
            this.btn_HandSingle.Click += new System.EventHandler(this.Btn_HandSingle_Click);
            // 
            // btn_TimerAuto
            // 
            this.btn_TimerAuto.Enabled = false;
            this.btn_TimerAuto.Location = new System.Drawing.Point(181, 59);
            this.btn_TimerAuto.Name = "btn_TimerAuto";
            this.btn_TimerAuto.Size = new System.Drawing.Size(75, 23);
            this.btn_TimerAuto.TabIndex = 6;
            this.btn_TimerAuto.Text = "开始";
            this.btn_TimerAuto.UseVisualStyleBackColor = true;
            this.btn_TimerAuto.Click += new System.EventHandler(this.Btn_Stop_Click);
            // 
            // btn_prePareFile
            // 
            this.btn_prePareFile.Location = new System.Drawing.Point(94, 59);
            this.btn_prePareFile.Name = "btn_prePareFile";
            this.btn_prePareFile.Size = new System.Drawing.Size(75, 23);
            this.btn_prePareFile.TabIndex = 6;
            this.btn_prePareFile.Text = "准备数据";
            this.btn_prePareFile.UseVisualStyleBackColor = true;
            this.btn_prePareFile.Click += new System.EventHandler(this.Btn_Start_Click);
            // 
            // groupBoxRoles
            // 
            this.groupBoxRoles.Controls.Add(this.gridViewUserList);
            this.groupBoxRoles.Controls.Add(this.btnSave);
            this.groupBoxRoles.Controls.Add(this.btnDeleUser);
            this.groupBoxRoles.Controls.Add(this.treeViewRoles);
            this.groupBoxRoles.Controls.Add(this.btnAddUser);
            this.groupBoxRoles.Location = new System.Drawing.Point(9, 13);
            this.groupBoxRoles.Name = "groupBoxRoles";
            this.groupBoxRoles.Size = new System.Drawing.Size(829, 307);
            this.groupBoxRoles.TabIndex = 8;
            this.groupBoxRoles.TabStop = false;
            this.groupBoxRoles.Text = "角色分配";
            // 
            // gridViewUserList
            // 
            this.gridViewUserList.AllowUserToAddRows = false;
            this.gridViewUserList.AllowUserToOrderColumns = true;
            this.gridViewUserList.AllowUserToResizeRows = false;
            this.gridViewUserList.AutoGenerateColumns = false;
            this.gridViewUserList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewUserList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UserName,
            this.PassWord,
            this.RoleID,
            this.Email,
            this.PhoneNum});
            this.gridViewUserList.DataSource = this.bindSourceUser;
            this.gridViewUserList.Location = new System.Drawing.Point(237, 20);
            this.gridViewUserList.Name = "gridViewUserList";
            this.gridViewUserList.RowTemplate.Height = 23;
            this.gridViewUserList.Size = new System.Drawing.Size(586, 249);
            this.gridViewUserList.TabIndex = 4;
            this.gridViewUserList.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.GridViewUserList_RowPostPaint);
            // 
            // UserName
            // 
            this.UserName.DataPropertyName = "UserName";
            this.UserName.Frozen = true;
            this.UserName.HeaderText = "用户名";
            this.UserName.Name = "UserName";
            this.UserName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PassWord
            // 
            this.PassWord.DataPropertyName = "PassWord";
            this.PassWord.Frozen = true;
            this.PassWord.HeaderText = "密码";
            this.PassWord.Name = "PassWord";
            this.PassWord.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PassWord.Width = 120;
            // 
            // RoleID
            // 
            this.RoleID.DataPropertyName = "RoleID";
            this.RoleID.Frozen = true;
            this.RoleID.HeaderText = "用户类型";
            this.RoleID.Name = "RoleID";
            // 
            // Email
            // 
            this.Email.DataPropertyName = "Email";
            this.Email.HeaderText = "邮箱";
            this.Email.Name = "Email";
            this.Email.Width = 120;
            // 
            // PhoneNum
            // 
            this.PhoneNum.DataPropertyName = "PhoneNum";
            this.PhoneNum.HeaderText = "电话";
            this.PhoneNum.Name = "PhoneNum";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(399, 275);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "保  存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnDeleUser
            // 
            this.btnDeleUser.Location = new System.Drawing.Point(318, 275);
            this.btnDeleUser.Name = "btnDeleUser";
            this.btnDeleUser.Size = new System.Drawing.Size(75, 23);
            this.btnDeleUser.TabIndex = 3;
            this.btnDeleUser.Text = "删除用户";
            this.btnDeleUser.UseVisualStyleBackColor = true;
            this.btnDeleUser.Click += new System.EventHandler(this.BtnDeleUser_Click);
            // 
            // treeViewRoles
            // 
            this.treeViewRoles.Location = new System.Drawing.Point(6, 20);
            this.treeViewRoles.Name = "treeViewRoles";
            this.treeViewRoles.Size = new System.Drawing.Size(225, 281);
            this.treeViewRoles.TabIndex = 0;
            this.treeViewRoles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewRoles_AfterSelect);
            // 
            // btnAddUser
            // 
            this.btnAddUser.Location = new System.Drawing.Point(237, 275);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(75, 23);
            this.btnAddUser.TabIndex = 2;
            this.btnAddUser.Text = "添加用户";
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new System.EventHandler(this.BtnAddUser_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 449);
            this.Controls.Add(this.groupBoxRoles);
            this.Controls.Add(this.grpBoxSimulator);
            this.MaximizeBox = false;
            this.Name = "MainFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "辅助功能";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.grpBoxSimulator.ResumeLayout(false);
            this.grpBoxSimulator.PerformLayout();
            this.groupBoxRoles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewUserList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindSourceUser)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_SelectFile;
        private System.Windows.Forms.TextBox textBoxFile;
        private System.Windows.Forms.GroupBox grpBoxSimulator;
        private System.Windows.Forms.Button btn_HandSingle;
        private System.Windows.Forms.Button btn_TimerAuto;
        private System.Windows.Forms.Button btn_prePareFile;
        private System.Windows.Forms.Label labelSendCnt;
        private System.Windows.Forms.GroupBox groupBoxRoles;
        private System.Windows.Forms.TreeView treeViewRoles;
        private System.Windows.Forms.Button btnDeleUser;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.BindingSource bindSourceUser;
        private System.Windows.Forms.DataGridView gridViewUserList;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PassWord;
        private System.Windows.Forms.DataGridViewComboBoxColumn RoleID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.DataGridViewTextBoxColumn PhoneNum;
    }
}

