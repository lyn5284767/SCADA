using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DemoDriver;
using DataService;
using Log;
using System.IO;
using DatabaseLib;
using System.Data.SqlClient;

namespace Demo
{
    public partial class MainFrm : Form
    {

        Timer timerSend = new Timer();
        DAService da = new DAService();
        List<UserData> list = new List<UserData>();
        BinaryReader reader;
        TreeNode majorTop;
        bool start = false;
        short curRoleID = 0;

        public static readonly List<RoleType> DataDict = new List<RoleType>
        {
          new RoleType(0,"管理员"),new RoleType(1,"调试员"),new RoleType(2,"技术员"), new RoleType(3,"操作员")
        };

        public MainFrm()
        {
            InitializeComponent();
            Init();
            this.FormClosed += MainFrm_FormClosed;
        }

        private void MainFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (timerSend.Enabled)
            {
                timerSend.Stop();
            }

            if (reader != null)
            {
                reader.Close();
            }
        }

        public void Init()
        {
            textBoxFile.Text = "E:\\Project\\DemoSCADA\\Main\\bin\\x86\\Debug\\Log\\2019-12-4-15-6-27-raw.bin";
            timerSend.Interval = 1100;
            timerSend.Tick += TimerSend_Tick;

            DataGridViewComboBoxColumn col = gridViewUserList.Columns["RoleID"] as DataGridViewComboBoxColumn;
            col.DataSource = DataDict;
            col.DisplayMember = "Name";
            col.ValueMember = "RoleID";
        }

        private void TimerSend_Tick(object sender, EventArgs e)
        {
            SingleRead();
        }

        private void SingleRead()
        {
            byte[] temp = reader.ReadBytes(500);
            da.SendBytes(temp);
            reader.ReadBytes(4);
        }

        private void Btn_SelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "E:\\Project\\DemoSCADA\\Main\\bin\\x86\\Debug\\Log\\";
                openFileDialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    textBoxFile.Text = openFileDialog.FileName;
                }
            }
        }

        private void Btn_Start_Click(object sender, EventArgs e)
        {

            //Read the contents of the file into a stream
            var fileName = textBoxFile.Text;

            if (!File.Exists(fileName))
            {
                MessageBox.Show("数据文件不存在！");
                return;
            }

            if (reader == null)
            {
                reader = new BinaryReader(new FileStream(fileName, FileMode.Open));
            }

            btn_TimerAuto.Enabled = true;
            btn_HandSingle.Enabled = true;
        }

        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            if (btn_TimerAuto.Text=="开始")
            {
                timerSend.Enabled = true;
                timerSend.Start();
                btn_TimerAuto.Text = "停止";
                btn_HandSingle.Enabled = false;
            }
            else
            {
                timerSend.Stop();
                btn_TimerAuto.Text = "开始";
                btn_HandSingle.Enabled = true;
            }
        }

        private void Btn_HandSingle_Click(object sender, EventArgs e)
        {
            SingleRead();
        }

        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            AddTag();
        }

        private void AddTag()
        {
            UserData usr = new UserData((int)(list.Count == 0 ? 1 : list.Max(x => x.ID) + 1),  "", "", 1, "", "");
            bindSourceUser.Add(usr);
            int index = list.BinarySearch(usr);
            if (index < 0) index = ~index;
            list.Insert(index, usr);
            gridViewUserList.FirstDisplayedScrollingRowIndex = bindSourceUser.Count - 1;
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            majorTop = treeViewRoles.Nodes.Add("", "角色管理", 0, 0);
            LoadFromDatabase();
            treeViewRoles.ExpandAll();
        }


        private void LoadFromDatabase()
        {
            List<UserData> data = new List<UserData>();
            list.Clear();
            majorTop.Nodes.Clear();

            string sql = "SELECT RoleID,RoleType,RoleDiscription FROM Roles;";
            using (var reader = DataHelper.Instance.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    RolesType roleType = new RolesType(reader.GetInt16(0), reader.GetString(1), reader.GetString(2));
                    majorTop.Nodes.Add(roleType.ID.ToString(), roleType.RoleType, 1, 1);
                }
            }

            sql = "SELECT Id,UserName,Password,Role,Email,Phone FROM UserList where Role<4";
            using (var reader = DataHelper.Instance.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    UserData tag = new UserData(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt16(3), reader.GetString(4),reader.GetString(5));
                    list.Add(tag);
                    data.Add(tag);
                }
            }

            list.Sort();
            bindSourceUser.DataSource = new SortableBindingList<UserData>(data);
            start = true;
        }

        private void GridViewUserList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(this.gridViewUserList.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), this.gridViewUserList.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            /*********为了让最后编辑的单元格数据可以保存到数据库中*****/
            gridViewUserList.CurrentCell = null;
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
            UserDataReader reader = new UserDataReader(list);
            result &= DataHelper.Instance.BulkCopy(reader, "UserList", "DELETE FROM UserList", SqlBulkCopyOptions.KeepIdentity);
            return result;
        }

        private void TreeViewRoles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!start) return;
            List<UserData> data = new List<UserData>();
            switch (e.Node.Level)
            {
                case 0:
                    data = list;
                    break;
                case 1:
                    {
                        curRoleID = short.Parse(e.Node.Name);
                        int index = list.BinarySearch(new UserData(curRoleID, null));
                        if (index < 0) index = ~index;
                        if (index < list.Count)
                        {
                            UserData usr = list[index];
                            while (usr.RoleID == curRoleID)
                            {
                                data.Add(usr);
                                if (++index < list.Count)
                                    usr = list[index];
                                else
                                    break;
                            }

                        }
                    }
                    break;
            }
            bindSourceUser.DataSource = new SortableBindingList<UserData>(data);
        }

        private void BtnDeleUser_Click(object sender, EventArgs e)
        {
            UserData usr = bindSourceUser.Current as UserData;
            bindSourceUser.Remove(usr);
            list.Remove(usr);
        }
    }

    public class RoleType
    {
        short _roleID;
        public short RoleID { get { return _roleID; } set { _roleID = value; } }

        string _name;
        public string Name { get { return _name; } set { _name = value; } }

        public RoleType(byte roleID, string name)
        {
            _roleID = roleID;
            _name = name;
        }
    }
}
