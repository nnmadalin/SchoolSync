using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace SchoolSync.login_signin
{
    public partial class signin : UserControl
    {
        public signin()
        {
            InitializeComponent();
        }

        private void signin_Load(object sender, EventArgs e)
        {
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
            var label = panel.Controls["label1"];
            label.Text = "SchoolSync | Inregistrare";
            GC.Collect();
            add_icon_error();
            
        }

        bool check_trim()
        {
            bool ok = true;
            if (guna2TextBox1.Text.Trim() == "")
            {
                errorProvider1.SetError(guna2TextBox1, "Completeaza caseta!");                
                guna2TextBox1.BorderColor = Color.FromArgb(203, 25, 39);
                ok = false;
            }
            else if (guna2TextBox2.Text.Trim() == "")
            {
                errorProvider2.SetError(guna2TextBox2, "Completeaza caseta!");
                guna2TextBox2.BorderColor = Color.FromArgb(203, 25, 39);
                ok = false;
            }
            else if(guna2TextBox3.Text.Trim() == "")
            {
                errorProvider3.SetError(guna2TextBox3, "Completeaza caseta!");
                guna2TextBox3.BorderColor = Color.FromArgb(203, 25, 39);
                ok = false;
            }
            else if(guna2TextBox4.Text.Trim() == "")
            {
                errorProvider4.SetError(guna2TextBox4, "Completeaza caseta!");
                guna2TextBox4.BorderColor = Color.FromArgb(203, 25, 39);
                ok = false;
            }
            else if(guna2TextBox5.Text.Trim() == "")
            {
                errorProvider5.SetError(guna2TextBox5, "Completeaza caseta!");
                guna2TextBox5.BorderColor = Color.FromArgb(203, 25, 39);
                ok = false;
            }

            else if(guna2TextBox4.Text != guna2TextBox5.Text)
            {
                errorProvider4.SetError(guna2TextBox4, "Parolele nu corespund!");
                guna2TextBox4.BorderColor = Color.FromArgb(203, 25, 39);

                errorProvider5.SetError(guna2TextBox5, "Parolele nu corespund!");
                guna2TextBox5.BorderColor = Color.FromArgb(203, 25, 39);
                ok = false;
            }

            else if(guna2TextBox4.Text.Length < 5)
            {
                errorProvider4.SetError(guna2TextBox4, "Parola trebuie sa fie de minim 5 caractere!");
                guna2TextBox4.BorderColor = Color.FromArgb(203, 25, 39);

                errorProvider5.SetError(guna2TextBox5, "Parolele nu corespund!");
                guna2TextBox5.BorderColor = Color.FromArgb(203, 25, 39);
                ok = false;
            }

            return ok;
        }

        void add_icon_error()
        {
            errorProvider1.Icon = SchoolSync.Properties.Resources.icons8_high_risk_48;
            errorProvider2.Icon = SchoolSync.Properties.Resources.icons8_high_risk_48;
            errorProvider3.Icon = SchoolSync.Properties.Resources.icons8_high_risk_48;
            errorProvider4.Icon = SchoolSync.Properties.Resources.icons8_high_risk_48;
            errorProvider5.Icon = SchoolSync.Properties.Resources.icons8_high_risk_48;
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var frm = new login();

            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
            panel.Controls.Add(frm);
            frm.Show();
            panel.Controls.Remove(this);
            GC.Collect();
        }

        public static string passencrypt(string text)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));

                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        async void sign_send()
        {
            schoolsync.show_loading();
            if (check_trim())
            {
                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "select * from accounts where username = ?");

                var param = new Dictionary<string, string>()
                {
                    {"username", guna2TextBox2.Text}
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                var multiple_class = new multiple_class();

                dynamic task = await multiple_class.PostRequestAsync(url, data);
                if (task["message"] == "success")
                {
                    errorProvider2.SetError(guna2TextBox2, "Username deja in baza de date!");
                    guna2TextBox2.BorderColor = Color.FromArgb(203, 25, 39);
                }
                else if (task["message"] == "database no value")
                {
                    url = "https://schoolsync.nnmadalin.me/api/get.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "select * from accounts where email = ?");
                    param = new Dictionary<string, string>()
                    {
                        {"email", guna2TextBox3.Text}
                    };

                    data.Add("params", JsonConvert.SerializeObject(param));

                    task = await multiple_class.PostRequestAsync(url, data);
                    if (task["message"] == "success")
                    {
                        errorProvider3.SetError(guna2TextBox3, "Email deja in baza de date!");
                        guna2TextBox3.BorderColor = Color.FromArgb(203, 25, 39);
                    }
                    else if (task["message"] == "database no value")
                    {
                        url = "https://schoolsync.nnmadalin.me/api/post.php";
                        data = new Dictionary<string, string>();
                        data.Add("token", schoolsync.token);

                        multiple_class multiple_Class = new multiple_class();
                        string token = multiple_class.generate_token();
                        data.Add("command", "insert into accounts (token, full_name, username, email, password) values (?, ?, ?, ?, ?)");
                        param = new Dictionary<string, string>()
                        {
                            {"token", token},
                            {"full_name", guna2TextBox1.Text},
                            {"username", guna2TextBox2.Text},
                            {"email", guna2TextBox3.Text},
                            {"password", passencrypt(guna2TextBox4.Text)}
                        };

                        data.Add("params", JsonConvert.SerializeObject(param));

                        task = await multiple_class.PostRequestAsync(url, data);
                        string token_app = schoolsync.token;

                        if (task["message"] == "insert success")
                        {
                            url = "https://schoolsync.nnmadalin.me/api/send_email.php";
                            data = new Dictionary<string, string>();
                            data.Add("token", token_app);
                            data.Add("token_user", token);
                            data.Add("to", guna2TextBox3.Text);
                            task = await multiple_class.PostRequestAsync(url, data);

                            if (task["message"] == "success")
                            {
                                var frm = new notification.success();
                                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                                panel.Controls.Add(frm);
                                notification.success.message = "Cont creat cu succes. Intra pe email si confirma contul!";
                                frm.BringToFront();
                                guna2TextBox1.Text = guna2TextBox2.Text = guna2TextBox3.Text = guna2TextBox4.Text = guna2TextBox5.Text = "";
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
            }
            schoolsync.hide_loading();
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            sign_send();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox1.BorderColor = Color.Black;
            errorProvider1.Dispose();
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox2.BorderColor = Color.Black;
            errorProvider2.Dispose();
        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox3.BorderColor = Color.Black;
            errorProvider3.Dispose();
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox4.BorderColor = Color.Black;
            errorProvider4.Dispose();
            guna2TextBox5.BorderColor = Color.Black;
            errorProvider5.Dispose();
        }

        private void guna2TextBox5_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox4.BorderColor = Color.Black;
            errorProvider4.Dispose();
            guna2TextBox5.BorderColor = Color.Black;
            errorProvider5.Dispose();
        }

        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
                sign_send();
        }
    }
}
