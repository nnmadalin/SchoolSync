using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolSync.login_signin
{
    public partial class login : UserControl
    {
        public login()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var frm = new signin();

            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
            panel.Controls.Add(frm);
            frm.Show();
            panel.Controls.Remove(this);
            GC.Collect();
        }


        private void login_Load(object sender, EventArgs e)
        {
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
            var label = panel.Controls["label1"];
            label.Text = "SchoolSync | Autentificare";
            GC.Collect();
        }

        public static string passencrypt(string text)
        {
            var plain = System.Text.Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(plain);
        }

        public static dynamic accounts_user;

        async void send_login()
        {
            string username = guna2TextBox1.Text;
            string pswd = guna2TextBox2.Text;

            pswd = passencrypt(pswd);

            var multiple_class = new multiple_class();

            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", "W!WSAnXZLOhyQ6lpt=adAhsOaF5QrI6eN4!1p/PWi7y8A9gTwKiD6DO6kmwdmcUHFeG?v99ihZYAeiLtf7NdT2MHCnzy=mvdI1MnmZLEtVOus2O0qYFo4oDfVyB7QeLBFo5SrzqueDvwtMFVBpRcLygr3Jxg-GhmOZ07IPsBpmZ8P0bhBUegmskNsTKk!x!bc2yT-LOrCwk!XU!!2I10=SLFfsf0s-OGCcmS-f=4l3X8u3lL/nsnY8vjSQ0jn13H");
            data.Add("sql", string.Format("select * from accounts where (username = '{0}' or email = '{0}') and password = '{1}'", username, pswd));

            dynamic task = await multiple_class.PostRequestAsync(url, data);

            if (task["message"] == "success")
            {
                var frm = new navbar_home();

                accounts_user = task["0"];

                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                frm.Show();
                panel.Controls.Remove(this);
                GC.Collect();
            }
            else if (task["message"] == "Database no value")
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                frm.Location = new Point(840, 50);
                frm.Show();
                notification.error.message = "Email sau parola gresita!";
                frm.BringToFront();
            }
            else
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                frm.Location = new Point(840, 50);
                frm.Show();
                notification.error.message = "Ceva nu e mers bine!";
                frm.BringToFront();
            }
        }
        

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            send_login();

        }

        private void guna2TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                send_login();
            }
        }
    }
}
