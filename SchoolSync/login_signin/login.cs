using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
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

        private async void login_Load(object sender, EventArgs e)
        {
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
            var label = panel.Controls["label1"];
            label.Text = "SchoolSync | Autentificare";
            GC.Collect();

            schoolsync.show_loading();
            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from version");
            dynamic task = await _class.PostRequestAsync(url, data);

            if (task["message"] == "success")
            {
                if (task["0"]["required_version_app"] == schoolsync.version)
                {
                    ; ;
                }
                else
                {
                    guna2MessageDialog1.Caption = "Versiune trecuta!";
                    guna2MessageDialog1.Text = "Ai o versiune prea veche. Te rog sa instalezi versiunea noua! \r\nschoolsync.nnmadalin.me \r\nVersiune actuala: " + schoolsync.version + " ;Versiune necesara: " + task["0"]["required_version_app"];
                    guna2MessageDialog1.Show();
                    Application.Exit();
                }
            }
            else
            {
                guna2MessageDialog1.Caption = "Eroare!";
                guna2MessageDialog1.Text = "Ceva nu a mers bine! Te rog sa redeschizi aplicatia!";
                guna2MessageDialog1.Show();
                Application.Exit();
            }

            if (Properties.Settings.Default.Data_account != "")
            {
                guna2ToggleSwitch1.Checked = true;
                dynamic json = JsonConvert.DeserializeObject(Properties.Settings.Default.Data_account);
                bool ok = false;
                try
                {
                    guna2TextBox1.Text = json["username"];
                    guna2TextBox2.Text = json["password"];
                    ok = true;
                }
                catch
                {
                    guna2TextBox2.Text = guna2TextBox1.Text = "";
                    Properties.Settings.Default.Data_account = "";
                    Properties.Settings.Default.Save();
                    schoolsync.hide_loading();
                }
                
                if (ok == true)
                {
                    send_login();
                }                
            }
            

        }

        public static dynamic accounts_user;

        bool VerifyPassword(string hashedPassword, string storedHash)
        {
            return storedHash.Equals(hashedPassword, StringComparison.OrdinalIgnoreCase);
        }

        async void send_login()
        {
            schoolsync.show_loading();
            string username = guna2TextBox1.Text;
            string pswd = guna2TextBox2.Text;

            pswd = signin.passencrypt(pswd);

            var multiple_class = new multiple_class();

            string tkn = schoolsync.token;

            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from accounts where username = ? or email = ?");

            var param = new Dictionary<string, string>()
            {
                {"username", username},
                {"email", username},
            };
            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await multiple_class.PostRequestAsync(url, data);

            string pswd_db = task["0"]["password"];

            if (task["message"] == "success" && VerifyPassword(pswd, pswd_db))
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
                    accounts_user = task["0"];
                    accounts_user["password"] = guna2TextBox2.Text;
                    
                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", tkn);
                    data.Add("command", "update accounts set last_login = now() where token = ?");
                    param = new Dictionary<string, string>()
                    {
                        {"token", Convert.ToString(accounts_user["token"])}
                    };

                    data.Add("params", JsonConvert.SerializeObject(param));

                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];

                    task = await multiple_class.PostRequestAsync(url, data);
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

                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    
                    panel.Controls.Add(frm);
                    panel.Controls.Remove(this);
                    GC.Collect();
                    schoolsync.hide_loading();
                }
            }
            else if (task["message"] == "Database no value" || !VerifyPassword(pswd, pswd_db))
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
                notification.error.message = "Eroare API: " + task["message"];
                frm.BringToFront();
            }
            schoolsync.hide_loading();
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
