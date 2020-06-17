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

namespace Main
{
    public partial class Login : Form
    {
        public UserData _curUsr;
        public UserData CurUsr
        {
            get { return _curUsr; }
        }

        public Login()
        {
            InitializeComponent();
            textBox_Name.GotFocus += WinAPI.TextBox_Name_GotFocus;
            textBox_Name.LostFocus += WinAPI.TextBox_Name_LostFocus;
            textBox_PassWord.GotFocus += WinAPI.TextBox_Name_GotFocus;
            textBox_PassWord.LostFocus += WinAPI.TextBox_Name_LostFocus;
            _curUsr.bSuccess = false;
        }

        private void BtnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
            System.Windows.Application.Current.Shutdown();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            TryLogin();
            if (_curUsr.bSuccess)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("密码或账户输入错误！");
            }
        }

        private void TryLogin()
        {
            string usrName = this.textBox_Name.Text.Trim();
            string password = this.textBox_PassWord.Text.Trim();

            string sql = string.Format("SELECT Id,UserName,Password,Role,Email,Phone FROM UserList where UserName='{0}' and Password='{1}'",usrName,password);

            using (var reader = DataHelper.Instance.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    _curUsr._id = reader.GetInt32(0);
                    _curUsr._userName = reader.GetString(1);
                    _curUsr._passWord = reader.GetString(2);
                    _curUsr._roleID = reader.GetInt16(3);
                    _curUsr._email = reader.GetString(4);
                    _curUsr._phone = reader.GetString(5);
                    _curUsr.bSuccess = true;
                    GlobalData.Instance.systemRole = (SystemRole)_curUsr._roleID;
                }
            }
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
