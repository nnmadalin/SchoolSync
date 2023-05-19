using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

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

        string decryptpswd(string base64)
        {
            var base64byte = System.Convert.FromBase64String(base64);
            return System.Text.Encoding.UTF8.GetString(base64byte);
        }

        private void login_Load(object sender, EventArgs e)
        {
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
            var label = panel.Controls["label1"];
            label.Text = "SchoolSync | Autentificare";
            GC.Collect();

            if (Properties.Settings.Default.Data_account != "")
            {
                guna2ToggleSwitch1.Checked = true;
                dynamic json = JsonConvert.DeserializeObject(Properties.Settings.Default.Data_account);
                bool ok = false;
                try
                {
                    guna2TextBox1.Text = json["username"];
                    string x = json["password"];
                    guna2TextBox2.Text = decryptpswd(x);
                    ok = true;
                }
                catch
                {
                    guna2TextBox2.Text = guna2TextBox1.Text = "";
                    Properties.Settings.Default.Data_account = "";
                    Properties.Settings.Default.Save();
                }
                
                if (ok == true)
                {
                    send_login();
                }
                
            }

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
            data.Add("token", schoolsync.token);
            data.Add("sql", string.Format("select * from accounts where (username = '{0}' or email = '{0}') and password = '{1}'", username, pswd));

            dynamic task = await multiple_class.PostRequestAsync(url, data);
            if (task["message"] == "success")
            {

                if (task["0"]["verified"] == "0")
                {
                    var frm2 = new notification.error();
                    schoolsync schoolsync2 = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel2 = (Guna.UI2.WinForms.Guna2Panel)schoolsync2.Controls["guna2Panel2"];
                    panel2.Controls.Add(frm2);
                    notification.error.message = "Contul nu a fost activat! Verifica spam!";
                    frm2.BringToFront();
                }
                else
                {

                    if (guna2ToggleSwitch1.Checked == true)
                    {
                        Properties.Settings.Default.Data_account = JsonConvert.SerializeObject(accounts_user);
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        Properties.Settings.Default.Data_account = "";
                        Properties.Settings.Default.Save();
                    }
                    var frm = new navbar_home();
                    accounts_user = task["0"];
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    panel.Controls.Remove(this);
                    GC.Collect();
                }

                
            }
            else if (task["message"] == "Database no value")
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.error.message = "Email sau parola gresita!";
                frm.BringToFront();
            }
            else
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
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
