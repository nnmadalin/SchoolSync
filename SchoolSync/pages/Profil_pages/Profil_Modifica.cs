using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.IO;

namespace SchoolSync.pages
{
    public partial class Profil_Modifica : UserControl
    {
        public Profil_Modifica()
        {
            InitializeComponent();
        }

        async void preload_user()
        {
            schoolsync.show_loading();
            multiple_class _Class = new multiple_class();

            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from accounts where token = ?");

            var param = new Dictionary<string, string>()
            {
                { "token", Convert.ToString(login_signin.login.accounts_user["token"])}
            };
            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _Class.PostRequestAsync(url, data);

            if (task["message"] == "success")
            {
                guna2TextBox1.Text = task["0"]["full_name"];
                guna2TextBox2.Text = task["0"]["username"];
                guna2TextBox3.Text = task["0"]["email"];
                guna2TextBox4.Text = task["0"]["location"];

                //load skills
                string str = task["0"]["skills"];
                string[] split = str.Split(';');
                for (int i = 0; i < split.Length - 1; i++)
                {
                    Guna.UI2.WinForms.Guna2Chip gcp = new Guna.UI2.WinForms.Guna2Chip()
                    {
                        AutoRoundedCorners = false,
                        BorderRadius = 5,
                        FillColor = Color.FromArgb(234, 248, 254),
                        Text = split[i],
                        AutoSize = true,
                        BorderColor = Color.Transparent,
                        ForeColor = Color.Black,
                        TextAlign = HorizontalAlignment.Left,
                    };
                    flowLayoutPanel1.Controls.Add(gcp);
                }

                
                guna2TextBox7.Text = task["0"]["description"];

                // afisare avatar
                url = "https://schoolsync.nnmadalin.me/api/get.php";
                data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "select * from files where token_user = ? and token = ?");

                param = new Dictionary<string, string>()
                {
                    { "token_user", Convert.ToString(login_signin.login.accounts_user["token"])},
                    { "token", "user_foto"}
                };
                data.Add("params", JsonConvert.SerializeObject(param));

                task = await _Class.PostRequestAsync(url, data);
                if (task["message"] == "success")
                {
                    guna2CirclePictureBox1.Image = await _Class.IncarcaImagineAsync("https://schoolsync.nnmadalin.me/attachments/" + login_signin.login.accounts_user["token"] + "/user_foto/" + task["0"]["name"]);
                }
                // afisare backgorund
                url = "https://schoolsync.nnmadalin.me/api/get.php";
                data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "select * from files where token_user = ? and token = ?");

                param = new Dictionary<string, string>()
                {
                    { "token_user", Convert.ToString(login_signin.login.accounts_user["token"])},
                    { "token", "user_background"}
                };
                data.Add("params", JsonConvert.SerializeObject(param));

                task = await _Class.PostRequestAsync(url, data);
                if (task["message"] == "success")
                {
                    guna2PictureBox1.Image = await _Class.IncarcaImagineBackgroundAsync("https://schoolsync.nnmadalin.me/attachments/" + login_signin.login.accounts_user["token"] + "/user_background/" + task["0"]["name"]);
                }
            }
            schoolsync.hide_loading();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            (sender as Guna.UI2.WinForms.Guna2TextBox).BorderColor = Color.FromArgb(213, 218, 223);
        }

        private void Profil_Modifica_Load(object sender, EventArgs e)
        {
            preload_user();
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if(dr == DialogResult.OK)
            {
                FileInfo fl = new FileInfo(openFileDialog1.FileName);

                long fileSizeibBytes = fl.Length;
                long fileSizeibMbs = fileSizeibBytes / (1024 * 1024);
                if (fileSizeibMbs > 5)
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Fisierul: " + fl.Name.Substring(0, 20) + "..." + " are mai mult de 5 MB!";
                    frm.BringToFront();
                }
                else
                {
                    guna2CirclePictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                    change_foto1 = true;
                    ff1 = openFileDialog1.FileName;
                }
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                FileInfo fl = new FileInfo(openFileDialog1.FileName);

                long fileSizeibBytes = fl.Length;
                long fileSizeibMbs = fileSizeibBytes / (1024 * 1024);

                if (fileSizeibMbs > 5)
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Fisierul: " + fl.Name.Substring(0, 20) + "..." + " are mai mult de 5 MB!";
                    frm.BringToFront();
                }
                else
                {
                    guna2PictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                    change_foto2 = true;
                    ff2 = openFileDialog1.FileName;
                }
            }
        }

        bool change_foto1 = false, change_foto2 = false;
        string ff1 = "", ff2 = "";

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text.Trim() == "")
            {
                guna2TextBox1.BorderColor = Color.Red;
            }            
            else
            {
                schoolsync.show_loading();
                string full_name = guna2TextBox1.Text;
                string locatia = guna2TextBox4.Text;
                string pass = guna2TextBox5.Text;
                string skills = "";

                foreach (Control ctrl in flowLayoutPanel1.Controls)
                {
                    skills += (ctrl.Text + ";");
                }

                string desc = guna2TextBox7.Text;
                string url;
                Dictionary<string, string> data;
                multiple_class _Class = new multiple_class();
                
                dynamic task;
                var param = new Dictionary<string, string>();

                if (change_foto1 == true)
                {
                    url = "https://schoolsync.nnmadalin.me/api/delete_file.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("token_user", Convert.ToString(login_signin.login.accounts_user["token"]));
                    data.Add("file", "user_foto");
                    task = await _Class.PostRequestAsync(url, data);

                    //sterge background din db

                    url = "https://schoolsync.nnmadalin.me/api/delete.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "delete from files where token_user = ? and token = ?");
                    param = new Dictionary<string, string>()
                    {
                        {"token_user", Convert.ToString(login_signin.login.accounts_user["token"])},
                        {"token", "user_foto"}
                    };
                    data.Add("params", JsonConvert.SerializeObject(param));
                    task = await _Class.PostRequestAsync(url, data);

                    //incarca background pe server
                    
                    FileInfo info = new FileInfo(ff1);
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("token_user", Convert.ToString(login_signin.login.accounts_user["token"]));
                    data.Add("token_file", "user_foto");
                    data.Add("filename", info.Name);
                    await _Class.UploadFileAsync(data, info.FullName);
                }

                if (change_foto2 == true)
                {
                    url = "https://schoolsync.nnmadalin.me/api/delete_file.php";
                    data = new Dictionary<string, string>();  
                    data.Add("command", schoolsync.token);
                    data.Add("token_user", Convert.ToString(login_signin.login.accounts_user["token"]));
                    data.Add("file", "user_background");
                    task = await _Class.PostRequestAsync(url, data);
                    //sterge background din db

                    url = "https://schoolsync.nnmadalin.me/api/delete.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "delete from files where token_user = ? and token = ?");
                    param = new Dictionary<string, string>()
                    {
                        {"token_user", Convert.ToString(login_signin.login.accounts_user["token"])},
                        {"token", "user_background"}
                    };
                    data.Add("params", JsonConvert.SerializeObject(param));
                    task = await _Class.PostRequestAsync(url, data);
                    
                    //incarca background pe server

                    FileInfo info = new FileInfo(ff2);
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("token_user", Convert.ToString(login_signin.login.accounts_user["token"]));
                    data.Add("token_file", "user_background");
                    data.Add("filename", info.Name);
                    await _Class.UploadFileAsync(data, info.FullName);
                }
                
                url = "https://schoolsync.nnmadalin.me/api/put.php";
                data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "update accounts set full_name = ?, location = ?, description = ?, skills = ? where token = ?");

                param = new Dictionary<string, string>()
                {
                    { "full_name", full_name},
                    { "location", locatia},
                    { "description", desc},
                    { "skills", skills},
                    { "token", Convert.ToString( login_signin.login.accounts_user["token"])}
                };
                data.Add("params", JsonConvert.SerializeObject(param));

                task = await _Class.PostRequestAsync(url, data);
                schoolsync.hide_loading();
                if (task["message"] == "update success")
                {

                    if (pass.Trim() != "")
                    {
                        url = "https://schoolsync.nnmadalin.me/api/put.php";
                        data = new Dictionary<string, string>();
                        data.Add("token", schoolsync.token);
                        data.Add("command", "update accounts set password = ? where token = ?");
                        param = new Dictionary<string, string>()
                        {
                            { "password", login_signin.signin.passencrypt(pass)},
                            { "token", Convert.ToString( login_signin.login.accounts_user["token"])}
                        };
                        data.Add("params", JsonConvert.SerializeObject(param));
                        task = await _Class.PostRequestAsync(url, data);
                        if (task["message"] == "update success")
                        {
                            var frm = new notification.success();
                            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                            schoolsync.hide_loading();
                            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                            panel.Controls.Add(frm);
                            notification.success.message = "Cont modificat cu success!";
                            frm.BringToFront();
                            Properties.Settings.Default.Data_account = "";
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            var frm = new notification.error();
                            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                            schoolsync.hide_loading();
                            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                            panel.Controls.Add(frm);
                            notification.error.message = "Ceva nu e mers bine!";
                            frm.BringToFront();
                        }
                    }
                    else
                    {
                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        schoolsync.hide_loading();
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.success.message = "Cont modificat cu success!";
                        frm.BringToFront();
                    }
                }
                else
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    schoolsync.hide_loading();
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Ceva nu e mers bine!";
                    frm.BringToFront();
                }
            }
            navbar_home.page = "Profil";
            navbar_home.use = false;
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if(guna2TextBox6.Text.Trim() != "")
            {
                Guna.UI2.WinForms.Guna2Chip gcp = new Guna.UI2.WinForms.Guna2Chip()
                {
                    AutoRoundedCorners = false,
                    BorderRadius = 5,
                    FillColor = Color.FromArgb(234, 248, 254),
                    Text = guna2TextBox6.Text,
                    AutoSize = true,
                    BorderColor = Color.Transparent,
                    ForeColor = Color.Black,
                    TextAlign = HorizontalAlignment.Left,
                };
                flowLayoutPanel1.Controls.Add(gcp);
            }

            guna2TextBox6.Clear();
        }
    }
}
