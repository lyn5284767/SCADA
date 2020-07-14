using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DatabaseLib;
using COM.Common;
using System.Windows.Threading;

namespace Main
{
    public partial class Login : Form
    {
        public UserData _curUsr;
        public UserData CurUsr
        {
            get { return _curUsr; }
        }
        System.Timers.Timer serviceTimer;
        public Login()
        {
            InitializeComponent();
            //textBox_Name.GotFocus += WinAPI.TextBox_Name_GotFocus;
            //textBox_Name.MouseDown += WinAPI.TextBox_Name_GotFocus;
            //textBox_Name.LostFocus += WinAPI.TextBox_Name_LostFocus;
            textBox_PassWord.GotFocus += WinAPI.TextBox_Name_GotFocus;
            //textBox_PassWord.MouseDown += WinAPI.TextBox_Name_GotFocus;
            //textBox_PassWord.LostFocus += WinAPI.TextBox_Name_LostFocus;
            _curUsr.bSuccess = false;
            this.cbType.SelectedIndex = 0;
            serviceTimer = new System.Timers.Timer(1000);
            serviceTimer.Elapsed += timer_Tick;
            serviceTimer.Enabled = true;
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        void timer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
                this.timeLable.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
          
        }

        private void BtnQuit_Click(object sender, EventArgs e)
        {
            WinAPI.TextBox_Name_LostFocus(null, null);
            this.Close();
            System.Windows.Application.Current.Shutdown();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            TryLogin();
            if (_curUsr.bSuccess)
            {
                WinAPI.TextBox_Name_LostFocus(null, null);
                this.Close();
            }
            else
            {
                MessageBox.Show("密码或账户输入错误！");
            }
        }

        private void TryLogin()
        {
            //string usrName = this.textBox_Name.Text.Trim();
            string password = this.textBox_PassWord.Text.Trim();
            string pwd = string.Empty;
            if (cbType.SelectedIndex == 0)
            {
                pwd = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Minute.ToString();
                if (password == pwd)
                {
                    _curUsr.bSuccess = true;
                    GlobalData.Instance.systemRole = SystemRole.DebugMan;
                }
            }
            else if (cbType.SelectedIndex == 1)
            {
                pwd = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString();
                if (password == pwd)
                {
                    _curUsr.bSuccess = true;
                    GlobalData.Instance.systemRole = SystemRole.TechMan;
                }
            }
            else if (cbType.SelectedIndex == 2)
            {
                pwd = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString();
                if (password == pwd)
                {
                    _curUsr.bSuccess = true;
                    GlobalData.Instance.systemRole = SystemRole.TechMan;
                }
            }
            else if (cbType.SelectedIndex == 3)
            {
                if (password == "111111")
                {
                    _curUsr.bSuccess = true;
                    GlobalData.Instance.systemRole = SystemRole.OperMan;
                }
            }
            //string sql = string.Format("SELECT Id,UserName,Password,Role,Email,Phone FROM UserList where UserName='{0}' and Password='{1}'",usrName,password);

            //using (var reader = DataHelper.Instance.ExecuteReader(sql))
            //{
            //    while (reader.Read())
            //    {
            //        _curUsr._id = reader.GetInt32(0);
            //        _curUsr._userName = reader.GetString(1);
            //        _curUsr._passWord = reader.GetString(2);
            //        _curUsr._roleID = reader.GetInt16(3);
            //        _curUsr._email = reader.GetString(4);
            //        _curUsr._phone = reader.GetString(5);
            //        _curUsr.bSuccess = true;
            //        GlobalData.Instance.systemRole = (SystemRole)_curUsr._roleID;
            //    }
            //}
        }
    }

    public struct UserData
    {
        public bool bSuccess;
        public int _id;
        public string _userName;
        public string _passWord;
        public short _roleID;
        public string _email;
        public string _phone;
    }
}
