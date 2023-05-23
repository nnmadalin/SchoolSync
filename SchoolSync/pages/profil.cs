﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static string token = "";
        public static string page = "";

        async void load_profil()
        {
            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("sql", string.Format("select * from accounts where token = '{0}'", token));
            schoolsync.show_loading();

            dynamic task = await _class.PostRequestAsync_norefresh(url, data);
            if(task["message"] == "success")
            {
                guna2CirclePictureBox1.Image = await _class.IncarcaImagineAsync("https://schoolsync.nnmadalin.me/api/getfile.php?token=userfoto_" + task["0"]["token_user"] + ".png");
                guna2PictureBox1.Image = await _class.IncarcaImagineBackgroundAsync("https://schoolsync.nnmadalin.me/api/getfile.php?token=userbackground_" + task["0"]["token_user"] + ".png");
                label1.Text = task["0"]["full_name"];
                label2.Text = "@" + task["0"]["username"];
                label5.Text = "Ultima conectare: " + task["0"]["last_login"];

                if (task["0"]["location"] == "")
                    label3.Text = "Nu a fost setat!";
                else
                    label3.Text = task["0"]["location"];

                label10.Text = task["0"]["description"];


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
            data.Add("sql", string.Format("select * from invataunit where token_user = '{0}'", token));
            task = await _class.PostRequestAsync_norefresh(url, data);
            JObject jb = task;
            if (task["message"] == "success")
            {
                label6.Text = "Intrebari: " + (jb.Count - 1).ToString();
            }

            url = "https://schoolsync.nnmadalin.me/api/get.php";
            data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("sql", string.Format("select * from accounts where token = '{0}'", token));
            task = await _class.PostRequestAsync_norefresh(url, data);
            if (task["message"] == "success")
            {
                string json = task["0"]["favorite_invataunit"];
                string[] spjson = json.Split(';');
                label11.Text = "Intrebari favorite: " + (spjson.Length - 1).ToString();
            }

            url = "https://schoolsync.nnmadalin.me/api/get.php";
            data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("sql", string.Format("select * from edumentor where token_user = '{0}'", token));
            task = await _class.PostRequestAsync_norefresh(url, data);
            jb = task;
            if (task["message"] == "success")
            {
                label7.Text = "Materiale adaugate: " + (jb.Count - 1).ToString();
            }

            schoolsync.hide_loading();
        }

        private async void Profil_Load(object sender, EventArgs e)
        {
            if (page == "home")
            {
                guna2CircleButton1.Visible = false;
                guna2CircleButton2.Visible = true;
                token = login_signin.login.accounts_user["token"];
            }
            load_profil();
            
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
