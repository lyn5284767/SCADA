﻿namespace ProtocolConfig
{
    partial class MainForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tagCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripCollect = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.connectTest = new System.Windows.Forms.ToolStripButton();
            this.btnPasteTags = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnQuit = new System.Windows.Forms.ToolStripButton();
            this.toolStriptbSearch = new System.Windows.Forms.ToolStripTextBox();
            this.toolStriplbSearch = new System.Windows.Forms.ToolStripLabel();
            this.dGVAccess = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridcontextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.粘贴CSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bindSourceProtocol = new System.Windows.Forms.BindingSource(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tvProtrcol = new System.Windows.Forms.TreeView();
            this.treeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.add = new System.Windows.Forms.ToolStripMenuItem();
            this.delete = new System.Windows.Forms.ToolStripMenuItem();
            this.reName = new System.Windows.Forms.ToolStripMenuItem();
            this.paraConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.statusStrip1.SuspendLayout();
            this.toolStripCollect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVAccess)).BeginInit();
            this.dataGridcontextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindSourceProtocol)).BeginInit();
            this.treeContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tagCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 684);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1367, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tagCount
            // 
            this.tagCount.Name = "tagCount";
            this.tagCount.Size = new System.Drawing.Size(56, 17);
            this.tagCount.Text = "变量数：";
            // 
            // toolStripCollect
            // 
            this.toolStripCollect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnDelete,
            this.btnClear,
            this.toolStripSeparator1,
            this.btnSave,
            this.toolStripSeparator2,
            this.connectTest,
            this.btnPasteTags,
            this.toolStripSeparator3,
            this.btnQuit,
            this.toolStriptbSearch,
            this.toolStriplbSearch});
            this.toolStripCollect.Location = new System.Drawing.Point(0, 0);
            this.toolStripCollect.Name = "toolStripCollect";
            this.toolStripCollect.Size = new System.Drawing.Size(1367, 25);
            this.toolStripCollect.TabIndex = 1;
            this.toolStripCollect.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::ProtocolConfig.Properties.Resources.Collect;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(52, 22);
            this.btnAdd.Text = "增加";
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::ProtocolConfig.Properties.Resources.Delete;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(52, 22);
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.Image = global::ProtocolConfig.Properties.Resources.Clear;
            this.btnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(52, 22);
            this.btnClear.Text = "清除";
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::ProtocolConfig.Properties.Resources.PSave;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(52, 22);
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // connectTest
            // 
            this.connectTest.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.connectTest.Image = global::ProtocolConfig.Properties.Resources.Tool;
            this.connectTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.connectTest.Name = "connectTest";
            this.connectTest.Size = new System.Drawing.Size(88, 22);
            this.connectTest.Text = "数据库测试";
            this.connectTest.Click += new System.EventHandler(this.ConnectTest_Click);
            // 
            // btnPasteTags
            // 
            this.btnPasteTags.Image = global::ProtocolConfig.Properties.Resources.Excel;
            this.btnPasteTags.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPasteTags.Name = "btnPasteTags";
            this.btnPasteTags.Size = new System.Drawing.Size(64, 22);
            this.btnPasteTags.Text = "另存为";
            this.btnPasteTags.ToolTipText = "另存为CSV文件";
            this.btnPasteTags.Click += new System.EventHandler(this.BtnPasteTags_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnQuit
            // 
            this.btnQuit.Image = global::ProtocolConfig.Properties.Resources.Exit;
            this.btnQuit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(52, 22);
            this.btnQuit.Text = "退出";
            this.btnQuit.Click += new System.EventHandler(this.BtnQuit_Click);
            // 
            // toolStriptbSearch
            // 
            this.toolStriptbSearch.Name = "toolStriptbSearch";
            this.toolStriptbSearch.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStriplbSearch
            // 
            this.toolStriplbSearch.Name = "toolStriplbSearch";
            this.toolStriplbSearch.Size = new System.Drawing.Size(32, 22);
            this.toolStriplbSearch.Text = "搜索";
            this.toolStriplbSearch.Click += new System.EventHandler(this.ToolStriplbSearch_Click);
            // 
            // dGVAccess
            // 
            this.dGVAccess.AllowUserToAddRows = false;
            this.dGVAccess.AllowUserToOrderColumns = true;
            this.dGVAccess.AllowUserToResizeRows = false;
            this.dGVAccess.AutoGenerateColumns = false;
            this.dGVAccess.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11,
            this.Column12,
            this.Column13});
            this.dGVAccess.ContextMenuStrip = this.dataGridcontextMenuStrip;
            this.dGVAccess.DataSource = this.bindSourceProtocol;
            this.dGVAccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGVAccess.Location = new System.Drawing.Point(0, 0);
            this.dGVAccess.Name = "dGVAccess";
            this.dGVAccess.RowTemplate.Height = 23;
            this.dGVAccess.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGVAccess.Size = new System.Drawing.Size(1144, 659);
            this.dGVAccess.TabIndex = 2;
            this.dGVAccess.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.DGVAccess_RowPostPaint);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Name";
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "标签名";
            this.Column1.Name = "Column1";
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 250;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Address";
            this.Column2.HeaderText = "地址";
            this.Column2.Name = "Column2";
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 80;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "DataType";
            this.Column3.HeaderText = "数据类型";
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "Size";
            this.Column4.HeaderText = "数据长度";
            this.Column4.Name = "Column4";
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column4.Width = 60;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "Active";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle1.NullValue = false;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column5.FalseValue = "False";
            this.Column5.HeaderText = "是否激活";
            this.Column5.Name = "Column5";
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column5.TrueValue = "True";
            this.Column5.Width = 150;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "HasAlarm";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle2.NullValue = false;
            this.Column6.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column6.FalseValue = "False";
            this.Column6.HeaderText = "是否报警";
            this.Column6.Name = "Column6";
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column6.TrueValue = "True";
            this.Column6.Width = 150;
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "HasScale";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle3.NullValue = false;
            this.Column7.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column7.FalseValue = "False";
            this.Column7.HeaderText = "是否量程";
            this.Column7.Name = "Column7";
            this.Column7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column7.TrueValue = "True";
            this.Column7.Width = 150;
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "Archive";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Green;
            dataGridViewCellStyle4.NullValue = false;
            this.Column8.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column8.FalseValue = "False";
            this.Column8.HeaderText = "是否归档";
            this.Column8.Name = "Column8";
            this.Column8.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column8.TrueValue = "True";
            this.Column8.Width = 150;
            // 
            // Column9
            // 
            this.Column9.DataPropertyName = "DefaultValue";
            this.Column9.HeaderText = "默认值";
            this.Column9.Name = "Column9";
            this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column9.Width = 60;
            // 
            // Column10
            // 
            this.Column10.DataPropertyName = "Maximum";
            this.Column10.HeaderText = "最大值";
            this.Column10.Name = "Column10";
            this.Column10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column10.Width = 60;
            // 
            // Column11
            // 
            this.Column11.DataPropertyName = "Minimum";
            this.Column11.HeaderText = "最小值";
            this.Column11.Name = "Column11";
            this.Column11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column11.Width = 60;
            // 
            // Column12
            // 
            this.Column12.DataPropertyName = "Description";
            this.Column12.HeaderText = "描述";
            this.Column12.Name = "Column12";
            this.Column12.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column12.Width = 265;
            // 
            // Column13
            // 
            this.Column13.DataPropertyName = "Cycle";
            this.Column13.HeaderText = "归档周期";
            this.Column13.Name = "Column13";
            this.Column13.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridcontextMenuStrip
            // 
            this.dataGridcontextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.粘贴CSVToolStripMenuItem});
            this.dataGridcontextMenuStrip.Name = "contextMenuStrip1";
            this.dataGridcontextMenuStrip.Size = new System.Drawing.Size(124, 26);
            this.dataGridcontextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ContextMenuStrip1_ItemClicked);
            // 
            // 粘贴CSVToolStripMenuItem
            // 
            this.粘贴CSVToolStripMenuItem.Name = "粘贴CSVToolStripMenuItem";
            this.粘贴CSVToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.粘贴CSVToolStripMenuItem.Text = "粘贴CSV";
            // 
            // tvProtrcol
            // 
            this.tvProtrcol.ContextMenuStrip = this.treeContextMenuStrip;
            this.tvProtrcol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvProtrcol.ImageIndex = 0;
            this.tvProtrcol.ImageList = this.imageList1;
            this.tvProtrcol.Location = new System.Drawing.Point(0, 0);
            this.tvProtrcol.Name = "tvProtrcol";
            this.tvProtrcol.SelectedImageIndex = 0;
            this.tvProtrcol.Size = new System.Drawing.Size(219, 659);
            this.tvProtrcol.TabIndex = 3;
            this.tvProtrcol.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.TreeView1_AfterLabelEdit);
            this.tvProtrcol.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvProtrcol_AfterSelect);
            // 
            // treeContextMenuStrip
            // 
            this.treeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add,
            this.delete,
            this.reName,
            this.paraConfig});
            this.treeContextMenuStrip.Name = "treeContextMenuStrip";
            this.treeContextMenuStrip.Size = new System.Drawing.Size(125, 92);
            this.treeContextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.TreeContextMenuStrip_ItemClicked);
            // 
            // add
            // 
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(124, 22);
            this.add.Text = "增加";
            // 
            // delete
            // 
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(124, 22);
            this.delete.Text = "删除";
            // 
            // reName
            // 
            this.reName.Name = "reName";
            this.reName.Size = new System.Drawing.Size(124, 22);
            this.reName.Text = "重命名";
            // 
            // paraConfig
            // 
            this.paraConfig.Name = "paraConfig";
            this.paraConfig.Size = new System.Drawing.Size(124, 22);
            this.paraConfig.Text = "参数设置";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Bakup.gif");
            this.imageList1.Images.SetKeyName(1, "socket.jpg");
            this.imageList1.Images.SetKeyName(2, "Class.gif");
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvProtrcol);
            this.splitContainer1.Panel1MinSize = 50;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dGVAccess);
            this.splitContainer1.Size = new System.Drawing.Size(1367, 659);
            this.splitContainer1.SplitterDistance = 219;
            this.splitContainer1.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1367, 706);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStripCollect);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Text = "协议定制";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStripCollect.ResumeLayout(false);
            this.toolStripCollect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVAccess)).EndInit();
            this.dataGridcontextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindSourceProtocol)).EndInit();
            this.treeContextMenuStrip.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStripCollect;
        private System.Windows.Forms.DataGridView dGVAccess;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnQuit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton connectTest;
        private System.Windows.Forms.ToolStripStatusLabel tagCount;
        private System.Windows.Forms.BindingSource bindSourceProtocol;
        private System.Windows.Forms.ToolStripButton btnPasteTags;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ContextMenuStrip dataGridcontextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 粘贴CSVToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TreeView tvProtrcol;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ContextMenuStrip treeContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem add;
        private System.Windows.Forms.ToolStripMenuItem delete;
        private System.Windows.Forms.ToolStripMenuItem reName;
        private System.Windows.Forms.ToolStripMenuItem paraConfig;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column5;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column6;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column7;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.ToolStripTextBox toolStriptbSearch;
        private System.Windows.Forms.ToolStripLabel toolStriplbSearch;
    }
}

