using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace SchoolSync.pages
{
    public partial class Profil : UserControl
    {
        public Profil()
        {
            InitializeComponent();
        }

        bool finish_to_load = false;

        async void load_profil()
        {
            flowLayoutPanel1.Controls.Clear();

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from accounts where token = ?");

            var param = new Dictionary<string, string>()
            {
                { "token", navbar_home.token_page}
            };
            data.Add("params", JsonConvert.SerializeObject(param));
            schoolsync.show_loading();

            dynamic task = await _class.PostRequestAsync(url, data);
            if(task["message"] == "success")
            {
                label1.Text = task["0"]["full_name"];
                label2.Text = "@" + task["0"]["username"];
                label5.Text = "Ultima conectare: " + task["0"]["last_login"];

                if (task["0"]["location"] == "")
                    label3.Text = "Nu a fost setat!";
                else
                    label3.Text = task["0"]["location"];

                label10.Text = task["0"]["description"];

                if(Convert.ToString(task["0"]["edumentor_moderator"]) == "1")
                {
                    label11.Text = "Moderator EduMentor: DA";
                }
                else
                    label11.Text = "Moderator EduMentor: NU";

                if (Convert.ToString(task["0"]["invataunit_moderator"]) == "1")
                {
                    label12.Text = "Moderator InvataUnit: DA";
                }
                else
                    label12.Text = "Moderator InvataUnit: NU";

                if (Convert.ToString(task["0"]["administrator_app"]) == "1")
                {
                    label13.Text = "Administrator: DA";
                }
                else
                    label13.Text = "Moderator InvataUnit: NU";

                if (navbar_home.page != "Profil" && Convert.ToString(login_signin.login.accounts_user["administrator_app"]) == "1")
                {
                    if (Convert.ToString(task["0"]["edumentor_moderator"]) == "1")
                        guna2CheckBox1.Checked = true;
                    if (Convert.ToString(task["0"]["invataunit_moderator"]) == "1")
                        guna2CheckBox2.Checked = true;
                    if (Convert.ToString(task["0"]["administrator_app"]) == "1")
                        guna2CheckBox3.Checked = true;
                }


                string skills = task["0"]["skills"];
                string[] split_skills = skills.Split(';');
                for(int i = 0; i < split_skills.Length - 1; i++)
                {
                    Guna.UI2.WinForms.Guna2Chip gcp = new Guna.UI2.WinForms.Guna2Chip()
                    {
                        AutoRoundedCorners = false,
                        ForeColor = Color.Black,
                        BorderThickness = 0,
                        BorderRadius = 5,
                        FillColor = Color.FromArgb(234, 248, 254),
                        AutoSize = true,
                        Text = split_skills[i],
                        IsClosable = false,
                    };
                    flowLayoutPanel1.Controls.Add(gcp);
                }                
            }

            url = "https://schoolsync.nnmadalin.me/api/get.php";
            data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from invataunit where token_user = ?");
             param = new Dictionary<string, string>()
            {
                { "token_user", navbar_home.token_page}
            };
            data.Add("params", JsonConvert.SerializeObject(param));
            task = await _class.PostRequestAsync(url, data);
            JObject jb = task;
            if (task["message"] == "success")
            {
                label6.Text = "Intrebari: " + (jb.Count - 1).ToString();
            }

            url = "https://schoolsync.nnmadalin.me/api/get.php";
            data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from edumentor where token_user = ?");
            param = new Dictionary<string, string>()
            {
                { "token_user", navbar_home.token_page}
            };
            data.Add("params", JsonConvert.SerializeObject(param));
            task = await _class.PostRequestAsync(url, data);
            jb = task;
            if (task["message"] == "success")
            {
                label7.Text = "Materiale adaugate: " + (jb.Count - 1).ToString();
            }

            // afisare avatar
            url = "https://schoolsync.nnmadalin.me/api/get.php";
            data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from files where token_user = ? and token = ?");

            param = new Dictionary<string, string>()
                {
                    { "token_user", navbar_home.token_page},
                    { "token", "user_foto"}
                };
            data.Add("params", JsonConvert.SerializeObject(param));

            task = await _class.PostRequestAsync(url, data);
            if (task["message"] == "success")
            {
                guna2CirclePictureBox1.Image = await _class.IncarcaImagineAsync("https://schoolsync.nnmadalin.me/attachments/" + navbar_home.token_page + "/user_foto/" + task["0"]["name"]);
            }
            // afisare backgorund
            url = "https://schoolsync.nnmadalin.me/api/get.php";
            data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from files where token_user = ? and token = ?");

            param = new Dictionary<string, string>()
                {
                    { "token_user", navbar_home.token_page},
                    { "token", "user_background"}
                };
            data.Add("params", JsonConvert.SerializeObject(param));

            task = await _class.PostRequestAsync(url, data);
            if (task["message"] == "success")
            {
                guna2PictureBox1.Image = await _class.IncarcaImagineBackgroundAsync("https://schoolsync.nnmadalin.me/attachments/" + navbar_home.token_page + "/user_background/" + task["0"]["name"]);
            }

            schoolsync.hide_loading();
            finish_to_load = true;
        }

        private async void Profil_Load(object sender, EventArgs e)
        {
            if (navbar_home.page == "Profil")
            {
                guna2CircleButton1.Visible = false;
                guna2CircleButton2.Visible = true;
            }
            else if(Convert.ToString(login_signin.login.accounts_user["administrator_app"]) == "1" && Convert.ToString(login_signin.login.accounts_user["token"]) != navbar_home.token_page)
            {
                guna2CheckBox1.Visible = guna2CheckBox2.Visible = guna2CheckBox3.Visible = true;
            }

            load_profil();
            
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {

            this.Dispose();
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            var frm = new pages.Profil_Modifica();
            this.Controls.Add(frm);
            frm.BringToFront();
        }

        private async void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (finish_to_load == true)
            {

                multiple_class _class = new multiple_class();
                string url = "https://schoolsync.nnmadalin.me/api/put.php";
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);

                if (((Control)sender).Text == "Mod. EduMentor?")
                {
                    string val = "0";
                    if (guna2CheckBox1.Checked == true)
                        val = "1";
                    else
                        val = "0";

                    data.Add("command", "update accounts set edumentor_moderator = ? where token = ?");

                    var param = new Dictionary<string, string>()
                    {
                        { "edumentor_moderator", val},
                        { "token", navbar_home.token_page},
                    };
                    data.Add("params", JsonConvert.SerializeObject(param));


                }
                else if (((Control)sender).Text == "Mod. InvataUnit?")
                {
                    string val = "0";
                    if (guna2CheckBox2.Checked == true)
                        val = "1";
                    else
                        val = "0";

                    data.Add("command", "update accounts set invataunit_moderator = ? where token = ?");

                    var param = new Dictionary<string, string>()
                    {
                        { "invataunit_moderator", val},
                        { "token", navbar_home.token_page},
                    };
                    data.Add("params", JsonConvert.SerializeObject(param));
                }
                else if (((Control)sender).Text == "Administrator")
                {
                    string val = "0";
                    if (guna2CheckBox3.Checked == true)
                        val = "1";
                    else
                        val = "0";

                    data.Add("command", "update accounts set administrator_app = ? where token = ?");

                    var param = new Dictionary<string, string>()
                    {
                        { "administrator_app", val},
                        { "token", navbar_home.token_page},
                    };
                    data.Add("params", JsonConvert.SerializeObject(param));
                }

                dynamic task = await _class.PostRequestAsync(url, data);

                if (task["message"] == "update success")
                {
                    var frm = new notification.success();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.success.message = "Profil actualizat!";
                    frm.BringToFront();

                    finish_to_load = false;
                    load_profil();
                }
                else
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Ceva nu a mers bine!";
                    frm.BringToFront();
                }
            }
        }
    }
}
