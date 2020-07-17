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
using DevExpress.Xpf.Charts.Native;

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
            System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process process in processList)
            {
                if (process.ProcessName.Contains("KeyBoard"))
                {
                    process.Kill();
                }
            }
            //textBox_Name.GotFocus += WinAPI.TextBox_Name_GotFocus;
            //textBox_Name.MouseDown += WinAPI.TextBox_Name_GotFocus;
            //textBox_Name.LostFocus += WinAPI.TextBox_Name_LostFocus;
            //textBox_PassWord.GotFocus += WinAPI.TextBox_Name_GotFocus;
            //textBox_PassWord.MouseDown += WinAPI.TextBox_Name_GotFocus;
            //textBox_PassWord.LostFocus += WinAPI.TextBox_Name_LostFocus;
            //textBox_PassWord.GotFocus += TextBox_PassWord_GotFocus;
            textBox_PassWord.Click += TextBox_PassWord_Click;
            _curUsr.bSuccess = false;
            this.cbType.SelectedIndex = 0;
            serviceTimer = new System.Timers.Timer(1000);
            serviceTimer.Elapsed += timer_Tick;
            serviceTimer.Enabled = true;
            Control.CheckForIllegalCrossThreadCalls = false;
            this.cbType.Focus();
        }

        private void TextBox_PassWord_Click(object sender, EventArgs e)
        {
            GlobalData.Instance.GetKeyBoard();
        }

        //private void TextBox_PassWord_GotFocus(object sender, EventArgs e)
        //{
        //    GlobalData.Instance.GetKeyBoard();
        //}

        void timer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
                this.timeLable.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); 
        }

        private void BtnQuit_Click(object sender, EventArgs e)
        {
            WinAPI.TextBox_Name_LostFocus(null, null);
            this.Close();
            System.Windows.Application.Current.Shutdown();
            System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process process in processList)
            {
                if (process.ProcessName.Contains("Main"))
                {
                    process.Kill();
                }
            }
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
            int day = DateTime.Now.Day;
            string strday = string.Empty;
            int month = DateTime.Now.Month;
            string strmonth = string.Empty;
            int minute = DateTime.Now.Minute;
            string strminute = string.Empty;
            int year = int.Parse(DateTime.Now.Year.ToString().Substring(2,2));
            string stryear = string.Empty;
            int hour = DateTime.Now.Hour;
            string strhour = string.Empty;
            if (day < 10) strday = "0" + day;
            else strday = day.ToString();
            if (month < 10) strmonth = "0" + month;
            else strmonth = month.ToString();
            if (minute < 10) strminute = "0" + minute;
            else strminute = minute.ToString();
            if (year < 10) stryear = "0" + year;
            else stryear = year.ToString();
            if (hour < 10) strhour = "0" + hour;
            else strhour = hour.ToString();
            if (cbType.SelectedIndex == 3)
            {

                pwd = strday + strmonth + strminute;
                
                if (password == pwd)
                {
                    _curUsr.bSuccess = true;
                    GlobalData.Instance.systemRole = SystemRole.DebugMan;
                }
            }
            else if (cbType.SelectedIndex == 2)
            {
                pwd = strday + strmonth + strhour;
                if (password == pwd)
                {
                    _curUsr.bSuccess = true;
                    GlobalData.Instance.systemRole = SystemRole.TechMan;
                }
            }
            else if (cbType.SelectedIndex == 1)
            {
                pwd = stryear +strmonth;
                if (password == pwd)
                {
                    _curUsr.bSuccess = true;
                    GlobalData.Instance.systemRole = SystemRole.TechMan;
                }
            }
            else if (cbType.SelectedIndex == 0)
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
