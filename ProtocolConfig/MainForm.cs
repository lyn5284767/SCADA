using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DatabaseLib;
using Log;
using DataService;
using System.Data.SqlClient;
using System.IO;


namespace ProtocolConfig
{
    public partial class MainForm : Form
    {
        #region Field
        List<TagData> list = new List<TagData>();
        List<Driver> devices = new List<Driver>();
        List<Group> groups = new List<Group>();
        bool start = false;
        short curgroupId = 0;

        TreeNode majorTop;
        List<TagData> data = new List<TagData>();
        #endregion

        public static readonly List<DataTypeSource> DataDict = new List<DataTypeSource>
        {
           new DataTypeSource (1,"开关型"),new DataTypeSource (3,"字节"), new DataTypeSource (4,"短整型"),
           new DataTypeSource (5,"单字型"),new DataTypeSource (6,"双字型"),new DataTypeSource (7,"长整型"),
           new DataTypeSource (8,"浮点型"),new DataTypeSource (9,"系统型"),new DataTypeSource (10,"ASCII字符串"),
           new DataTypeSource(0,"")
        };

        public static readonly List<CameraTypeSource> CamDict = new List<CameraTypeSource>
        {
           new CameraTypeSource (0,"宏英"),new CameraTypeSource (1,"一通"), new CameraTypeSource (3,"不配置")
        };

        public MainForm()
        {
            InitializeComponent();

            DataGridViewComboBoxColumn col = dGVAccess.Columns["Column3"] as DataGridViewComboBoxColumn;
            col.DataSource = DataDict;
            col.DisplayMember = "Name";
            col.ValueMember = "DataType";

            //DataGridViewComboBoxColumn camcol = this.dgvCameraInfo.Columns["dataGridViewCheckBoxColumn4"] as DataGridViewComboBoxColumn;
            //camcol.DataSource = CamDict;
            //camcol.DisplayMember = "Name";
            //camcol.ValueMember = "CameraType";
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            AddTag();
        }

        private void AddTag()
        {
            TagData tag = new TagData((short)(list.Count == 0 ? 1 : list.Max(x => x.ID) + 1), short.Parse(tvProtrcol.SelectedNode.Name), "", "", 1, 1, true, false, false, false, null, "", 0, 0, 100);
            bindSourceProtocol.Add(tag);
            int index = list.BinarySearch(tag);
            if (index < 0) index = ~index;
            list.Insert(index, tag);
            dGVAccess.FirstDisplayedScrollingRowIndex = bindSourceProtocol.Count - 1;
        }

        private void ConnectTest_Click(object sender, EventArgs e)
        {
            if (DataHelper.Instance.ConnectionTest())
            {
                MessageBox.Show("数据库连接成功！");
                Log4Net.AddLog("数据库连接成功！", InfoLevel.INFO);
            }
            else
            {
                MessageBox.Show("数据库连接失败！");
                Log4Net.AddLog("数据库连接失败！",InfoLevel.FATAL);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            majorTop = tvProtrcol.Nodes.Add("", "协议汇总", 0, 0);
            LoadFromDatabase();
            tvProtrcol.ExpandAll();
        }

        private void LoadFromDatabase()
        {
            List<TagData> data = new List<TagData>();
            list.Clear();
            majorTop.Nodes.Clear();

            string sql = "SELECT DriverID,DriverType,DriverName FROM DRIVER;";
            List<Driver> driverlist = DataHelper.Instance.ExecuteList<Driver>(sql);
            //using (var reader = DataHelper.Instance.ExecuteList<Driver>(sql))
            //{
            //    while (reader.Read())
            //    {
            //        Driver device = new Driver(reader.GetInt16(0), reader.GetInt32(1), reader.GetString(2));
            //        devices.Add(device);
            //        majorTop.Nodes.Add(device.ID.ToString(), device.Name, 1, 1);
            //    }
            //}


            //foreach (TreeNode node in majorTop.Nodes)
            //{
            //    sql = string.Format("SELECT GroupID,DriverID,GroupName,UpdateRate,DeadBand,IsActive FROM ProtocolGroup WHERE DriverID={0};", node.Name);
            //    using (var reader = DataHelper.Instance.ExecuteReader(sql))
            //    {
            //        while (reader.Read())
            //        {
            //            Group group = new Group(reader.GetInt16(0), reader.GetInt16(1), reader.GetString(2), reader.GetInt32(3), reader.GetFloat(4), reader.GetBoolean(5));
            //            groups.Add(group);
            //            node.Nodes.Add(group.ID.ToString(), group.Name, 2, 2);
            //        }
            //    }
            //}

            //sql = "SELECT TagId,GroupID,TagName,Address,DataType,DataSize,IsActive,Archive,DefaultValue,Description,Maximum,Minimum,Cycle from Protocol where DataType<12";
            //using (var reader = DataHelper.Instance.ExecuteReader(sql))
            //{                
            //    while (reader.Read())
            //    {
            //        TagData tag = new TagData(reader.GetInt16(0), reader.GetInt16(1), reader.GetString(2), reader.GetString(3), reader.GetByte(4),
            //         (ushort)reader.GetInt16(5), reader.GetBoolean(6), false, false, reader.GetBoolean(7),
            //         reader.GetValue(8), reader.GetNullableString(9), reader.GetFloat(10), reader.GetFloat(11), reader.GetInt32(12));
            //        list.Add(tag);
            //        data.Add(tag);
            //    }
            //}

            //list.Sort();
            //bindSourceProtocol.DataSource = new SortableBindingList<TagData>(data);
            //tagCount.Text += list.Count.ToString();
            //start = true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            /*********为了让最后编辑的单元格数据可以保存到数据库中*****/
            toolStripCollect.Focus();
            dGVAccess.CurrentCell = null;
            /*********************************************************/

            if (Save())
            {
                MessageBox.Show("保存成功!");
            }
            else
            {
                MessageBox.Show("保存失败!");
            }
        }

        private bool Save()
        {
            bool result = true;
            string sql = "DELETE FROM Driver;DELETE FROM ProtocolGroup;";
            foreach (Driver device in devices)
            {
                sql = string.Concat(sql, string.Format("INSERT INTO Driver(DriverID,DriverName,DriverType)"
                + " VALUES({0},'{1}',{2});",
                    device.ID, device.Name, device.DeviceDriver));
            }
            foreach (Group grp in groups)
            {
                sql = string.Concat(sql, string.Format("INSERT INTO ProtocolGroup(GroupID,GroupName,DriverID,UpdateRate,DeadBand,IsActive) VALUES({0},'{1}',{2},{3},{4},'{5}');",
                    grp.ID, grp.Name, grp.DriverID, grp.UpdateRate, grp.DeadBand, grp.Active));
            }

            //IEnumerable<TagData> tempList;
            //tempList = list.Where(e => (e.ID>=31));
            //foreach (TagData ta in tempList)
            //{
            //    ta.GroupID = 4;
            //}

            TagDataReader reader = new TagDataReader(list);
            result &= DataHelper.Instance.ExecuteNonQuery(sql) >= 0;
            result &= DataHelper.Instance.BulkCopy(reader, "Protocol", "DELETE FROM Protocol", SqlBulkCopyOptions.KeepIdentity);
            return result;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            TagData tag = bindSourceProtocol.Current as TagData;
            bindSourceProtocol.Remove(tag);
            list.Remove(tag);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("将清除所有的标签，是否确定？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bindSourceProtocol.Clear();
                list.Clear();
            }
        }

        private void BtnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MessageBox.Show("退出之前是否需要保存？", "警告", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Save();
            }
        }

        private void LoadFromCsv()
        {
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                List<TagData> dataList = new List<TagData>();
                string data = Clipboard.GetText(TextDataFormat.Text);
                if (string.IsNullOrEmpty(data)) return;
                list.Clear();
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
                {
                    using (var mysr = new StreamReader(stream))
                    {
                        string strline = "";
                        while ((strline = mysr.ReadLine()) != null)
                        {
                            string[] aryline = strline.Split('\t');
                            try
                            {
                                var id = Convert.ToInt16(aryline[0]);
                                var groupid = Convert.ToInt16(aryline[1]);
                                var name = aryline[2];
                                var address = aryline[3];
                                var type = Convert.ToByte(aryline[4]);
                                var size = Convert.ToUInt16(aryline[5]);
                                var active = Convert.ToBoolean(aryline[6]);
                                var desp = aryline[7];
                                var cycle = Convert.ToInt32(aryline[8]);
                                TagData tag = new TagData(id, groupid, name, address, type, size, active, false, false, false, null, desp, 0, 0, cycle);
                                list.Add(tag);
                                dataList.Add(tag);
                            }
                            catch (Exception err)
                            {
                                Log4Net.AddLog("LoadFromCsv() " + err.Message, InfoLevel.FATAL);
                                continue;
                            }
                        }
                    }
                }
                list.Sort();

                bindSourceProtocol.DataSource = new SortableBindingList<TagData>(dataList);
                tagCount.Text += dataList.Count.ToString();
            }
        }

        private void BtnPasteTags_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "xml文件 (*.xml)|*.xml|csv文件 (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string file = saveFileDialog1.FileName;
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        SaveToCsv(file);
                        break;
                    case 2:
                        SaveToCsv(file);
                        break;
                }
            }
        }

        private void SaveToCsv(string file)
        {
            using (StreamWriter objWriter = new StreamWriter(file, false))
            {
                foreach (TagData tag in list)
                {
                    objWriter.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", tag.ID, tag.GroupID, tag.Name, tag.Address, tag.DataType, tag.Size, tag.Active, tag.DefaultValue, tag.Description,tag.Cycle);
                }
            }
        }

        /// <summary>
        /// 显示行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGVAccess_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(this.dGVAccess.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), this.dGVAccess.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        private void ContextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "粘贴CSV":
                    LoadFromCsv();
                    break;
            }
        }

        public void UpdateNode()
        {
            TreeNode node = tvProtrcol.SelectedNode;
            if (node != null && node.Level != 0)
            {
                tvProtrcol.LabelEdit = true;
                node.BeginEdit();
            }
            else
                tvProtrcol.LabelEdit = false;
        }

        public void RemoveNode()
        {
            TreeNode node = tvProtrcol.SelectedNode;
            if (node != null && ((node.Level == 2 && bindSourceProtocol.Count == 0)
                || (node.Level == 1 && node.Nodes.Count == 0)))
            {
                if (node.Level == 1)
                {
                    foreach (Driver device in devices)
                    {
                        if (device.ID.ToString() == node.Name)
                        {
                            foreach (Group grp in groups)
                            {
                                if (grp.DriverID.ToString() == node.Name)
                                {
                                    groups.Remove(grp);
                                    node.Remove();
                                    return;
                                }
                            }
                            devices.Remove(device);
                            node.Remove();
                            return;
                        }
                    }
                }
                else
                {
                    foreach (Group grp in groups)
                    {
                        if (grp.ID.ToString() == node.Name)
                        {
                            groups.Remove(grp);
                            node.Remove();
                            return;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("包含下级，禁止删除!");
            }
        }

        public void AddNode()
        {
            TreeNode node = tvProtrcol.SelectedNode;
            if (node != null)
            {
                short did = 0;// short.MinValue;
                if (node.Level == 0)
                {
                    for (int i = 0; i < devices.Count; i++)
                    {
                        short temp = devices[i].ID;
                        if (temp > did)
                            did = temp;
                    }
                    did++;
                    devices.Add(new Driver { ID = did });
                }
                else if (node.Level == 1)
                {
                    for (int i = 0; i < groups.Count; i++)
                    {
                        short temp = groups[i].ID;
                        if (temp > did)
                            did = temp;
                    }
                    did++;
                    groups.Add(new Group { ID = did, DriverID = short.Parse(node.Name) });
                }
                else if (node.Level == 2)
                {
                    AddTag();
                    return;
                }
                TreeNode nwNode = node.Nodes.Add(did.ToString(), "", node.Level + 1, node.Level + 1);
                tvProtrcol.SelectedNode = nwNode;
                tvProtrcol.LabelEdit = true;
                nwNode.BeginEdit();
                //bindingSource1.Clear();
            }
        }

        private void TreeContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "增加":
                    AddNode();
                    break;
                case "删除":
                    RemoveNode();
                    break;
                case "重命名":
                    UpdateNode();
                    break;
                case "参数设置":
                    {
                        TreeNode node = tvProtrcol.SelectedNode;
                        if (node != null)
                        {
                            //if (node.Level == 1)
                            //{
                            //    short id = short.Parse(node.Name);
                            //    foreach (Driver device in devices)
                            //    {
                            //        if (device.ID == id)
                            //        {
                            //            DriverSet frm = new DriverSet(device, typeList, arguments);
                            //            frm.ShowDialog();
                            //            node.Text = device.Name;
                            //            return;
                            //        }
                            //    }
                            //}
                            if (node.Level == 2)
                            {
                                short id = short.Parse(node.Name);
                                foreach (Group grp in groups)
                                {
                                    if (grp.ID == id)
                                    {
                                        GroupParam frm = new GroupParam(grp);
                                        frm.ShowDialog();
                                        node.Text = grp.Name;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
        }

        private void tvProtrcol_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Text == "摄像头配置")
            {
                string sql = "SELECT * FROM CameraInfo";
                List<CameraInfo> cameraInfolist = DataHelper.Instance.ExecuteList<CameraInfo>(sql);
                this.dgvCameraInfo.DataSource = cameraInfolist;
            }
            if (!start) return;
            switch (e.Node.Level)
            {
                case 0:
                    data = list;
                    break;
                case 1:
                    {
                        foreach (TreeNode node in e.Node.Nodes)
                        { 
                            curgroupId = short.Parse(node.Name);
                            int index = list.BinarySearch(new TagData(curgroupId, null));
                            if (index < 0) index = ~index;
                            if (index < list.Count)
                            {
                                TagData tag = list[index];
                                while (tag.GroupID == curgroupId)
                                {
                                    data.Add(tag);
                                    if (++index < list.Count)
                                        tag = list[index];
                                    else
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case 2:
                    {
                        curgroupId = short.Parse(e.Node.Name);
                        int index = list.BinarySearch(new TagData(curgroupId, null));
                        if (index < 0) index = ~index;
                        if (index < list.Count)
                        {
                            TagData tag = list[index];
                            while (tag.GroupID == curgroupId)
                            {
                                data.Add(tag);
                                if (++index < list.Count)
                                    tag = list[index];
                                else
                                    break;
                            }

                        }
                    }
                    break;
            }
            bindSourceProtocol.DataSource = new SortableBindingList<TagData>(data);
            tagCount.Text = data.Count.ToString();
        }

        private void TreeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (!start) return;
            if (string.IsNullOrEmpty(e.Label))
            {
                e.CancelEdit = false;
            }
            else
            {
                tvProtrcol.LabelEdit = false;
                if (e.Node.Level == 1)
                {
                    foreach (Driver device in devices)
                    {
                        if (device.ID.ToString() == e.Node.Name)
                        {
                            device.Name = e.Label;
                            break;
                        }
                    }
                }
                else
                {
                    if (!groups.Exists(x => x.Name == e.Label))
                    {
                        foreach (Group grp in groups)
                        {
                            if (grp.ID.ToString() == e.Node.Name)
                            {
                                grp.Name = e.Label;
                                break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("组名不能重复!");
                    }
                }
            }
        }

        /// <summary>
        /// add by lyn,2020.3.28 增加搜索功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStriplbSearch_Click(object sender, EventArgs e)
        {
            List<TagData> tempdata = data.Where(w => w.Name.Contains(this.toolStriptbSearch.Text)).ToList();
            bindSourceProtocol.DataSource = new SortableBindingList<TagData>(tempdata);
        }
    }
}
