﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace SchoolSync.pages.EduMentor_pages
{
    public partial class EduMentor_Vizualizare : UserControl
    {
        public EduMentor_Vizualizare()
        {
            InitializeComponent();
        }

        private void deschide_fisier_buton(object sender, EventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            System.Diagnostics.Process.Start(@"https://schoolsync.nnmadalin.me/attachments/" + btn.Tag.ToString());
        }

        private async void EduMentor_Vizualizare_Load(object sender, EventArgs e)
        {
            schoolsync.show_loading();

            panel1.VerticalScroll.Visible = true;           
            richTextBox1.Height = (richTextBox1.GetLineFromCharIndex(richTextBox1.Text.Length) + 1) * richTextBox1.Font.Height + richTextBox1.Margin.Vertical;
           
            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from edumentor where token = ?");
            //navbar_home.token_page
            var param = new Dictionary<string, string>()
            {
                {"token", navbar_home.token_page},
            };
            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);
            JObject jb = task;
            if (task["message"] == "success")
            {
                label5.Text = task["0"]["title"];
                label1.Text = task["0"]["created"];
                label2.Text = task["0"]["category"] + " ● " + task["0"]["data"];
                label3.Text = task["0"]["reading_time"] + " min";
                richTextBox1.Rtf = task["0"]["description"];
                guna2Panel3.FillColor = task["0"]["color"];
                
                string[] split_fav = Convert.ToString(task["0"]["favourites"]).Split(';');
                label4.Text = (split_fav.Length - 1).ToString();

                if (task["0"]["created"] == login_signin.login.accounts_user["username"])
                {
                    guna2CircleButton2.Visible = true;
                    guna2CircleButton1.Visible = true;
                }

                //t_usr.Image = await _class.IncarcaImagineAsync("https://schoolsync.nnmadalin.me/api/getfile.php?token=userfoto_" + task["0"]["token_user"] + ".png");                

                string file = task["0"]["files"];
                string[] file_split = file.Split(';');

                for (int i = 0; i < file_split.Length - 1; i++)
                {
                    url = "https://schoolsync.nnmadalin.me/api/get.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "select * from files where token_user = ? and token = ?");
                    param = new Dictionary<string, string>()
                    {
                        {"token_user", Convert.ToString(login_signin.login.accounts_user["token"])},
                        {"token", file_split[i]}
                    };
                    data.Add("params", JsonConvert.SerializeObject(param));

                    task = await _class.PostRequestAsync(url, data);
                    if (task["message"] == "success")
                    {
                        try
                        {
                            string[] splitplit = Convert.ToString(task["0"]["name"]).Split('.');

                            Guna.UI2.WinForms.Guna2Panel flp_files_panel = new Guna.UI2.WinForms.Guna2Panel()
                            {
                                Size = new Size(200, 180),
                                FillColor = Color.FromArgb(235, 241, 244),
                                BorderRadius = 10,
                                UseTransparentBackground = true
                            };
                            Guna.UI2.WinForms.Guna2CirclePictureBox gcp = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                            {
                                Size = new Size(60, 60),
                                UseTransparentBackground = true,
                                SizeMode = PictureBoxSizeMode.CenterImage,
                                FillColor = Color.FromArgb(208, 216, 220),
                                Location = new Point(65, 15),
                            };
                            Label lbl_panel_file = new Label()
                            {
                                Location = new Point(0, 80),
                                AutoSize = true,
                                MinimumSize = new Size(190, 0),
                                Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold),
                                TextAlign = ContentAlignment.TopCenter,
                            };
                            Guna.UI2.WinForms.Guna2Button panel_file_btn = new Guna.UI2.WinForms.Guna2Button()
                            {
                                FillColor = Color.FromArgb(112, 204, 97),
                                ForeColor = Color.Black,
                                BorderRadius = 15,
                                Cursor = Cursors.Hand,
                                Text = "Download",
                                TextAlign = HorizontalAlignment.Left,
                                Image = SchoolSync.Properties.Resources.download_FILL1_wght700_GRAD0_opsz48,
                                ImageSize = new Size(15, 15),
                                ImageAlign = HorizontalAlignment.Right,
                                Size = new Size(110, 30),
                                Font = new Font("Segoe UI Semibold", 9, FontStyle.Bold),
                                Location = new Point((200 - 110) / 2, 140)
                            };

                            flowLayoutPanel1.Controls.Add(flp_files_panel);
                            flp_files_panel.Controls.Add(gcp);
                            flp_files_panel.Controls.Add(lbl_panel_file);

                            if (splitplit[0].Length >= 10)
                            {
                                lbl_panel_file.Text = splitplit[0].Substring(0, 10) + "." + splitplit[1];
                            }
                            else
                                lbl_panel_file.Text = task["0"]["name"];
                            if (splitplit[1] == "jpg" || splitplit[1] == "jpeg" || splitplit[1] == "png" || splitplit[1] == "svg" || splitplit[1] == "webp" || splitplit[1] == "bmp")
                            {
                                gcp.Image = SchoolSync.Properties.Resources.image_FILL1_wght700_GRAD0_opsz48;
                            }
                            else
                                gcp.Image = SchoolSync.Properties.Resources.description_FILL1_wght700_GRAD0_opsz48;

                            flp_files_panel.Controls.Add(panel_file_btn);

                            panel_file_btn.Tag = Convert.ToString(login_signin.login.accounts_user["token"]) + "/" + task["0"]["token"] + "/" + task["0"]["name"];
                            panel_file_btn.Click += deschide_fisier_buton;
                        }
                        catch { };
                    }
                }
            }
            else
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.error.message = "Ceva nu a functionat bine! Te rog sa dai refresh la pagina!";
                frm.BringToFront();
            }

            schoolsync.hide_loading();
        }

        private void richTextBox1_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            ((RichTextBox)sender).Height = e.NewRectangle.Height + 5;
        }

        void close_this()
        {
            if (navbar_home.page == "EduMentor_vizualizare")
            {
                navbar_home.page = "EduMentor";
                navbar_home.use = false;
            }
            else
            {
                navbar_home.page = "Home";
                navbar_home.use = false;
            }
        }

        private void guna2CircleButton3_Click(object sender, EventArgs e)
        {
            close_this();
        }

        private async void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            

            string token = navbar_home.token_page;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
            guna2MessageDialog1.Caption = "Sterge material!";
            guna2MessageDialog1.Text = "Esti sigur ca vrei sa stergi materialul?";
            DialogResult dr = guna2MessageDialog1.Show();
            if (dr == DialogResult.Yes)
            {
                schoolsync.show_loading();

                multiple_class _Class = new multiple_class();

                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "select * from edumentor where token = ?");
                //navbar_home.token_page
                var param = new Dictionary<string, string>()
                {
                    {"token", navbar_home.token_page},
                };
                data.Add("params", JsonConvert.SerializeObject(param));

                dynamic task = await _Class.PostRequestAsync(url, data);

                if (task["message"] == "success")
                {
                    string fisiere_value = task["0"]["files"];


                    url = "https://schoolsync.nnmadalin.me/api/delete.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "delete from edumentor where token = ?");

                    param = new Dictionary<string, string>()
                    {
                        {"token", token}
                    };
                    data.Add("params", JsonConvert.SerializeObject(param));

                    task = await _Class.PostRequestAsync(url, data);                    

                    string token_app = schoolsync.token;

                    if (task["message"] == "delete success")
                    {
                        string[] split_1 = fisiere_value.Split(';');
                        for (int i = 0; i < split_1.Length - 1; i++)
                        {
                            url = "https://schoolsync.nnmadalin.me/api/delete_file.php";
                            data = new Dictionary<string, string>();
                            data.Add("token", token_app);
                            data.Add("token_user", Convert.ToString(login_signin.login.accounts_user["token"]));
                            data.Add("file", split_1[i]);
                            task = await _Class.PostRequestAsync(url, data);

                            url = "https://schoolsync.nnmadalin.me/api/delete.php";
                            data = new Dictionary<string, string>();
                            data.Add("token", token_app);
                            data.Add("command", "delete from files where token = ?");

                            param = new Dictionary<string, string>()
                            {
                                {"token", split_1[i]}
                            };
                            data.Add("params", JsonConvert.SerializeObject(param));
                            task = await _Class.PostRequestAsync(url, data);                           
                        }

                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);

                        notification.success.message = "Material sters cu succes!";
                        frm.BringToFront();
                    }
                    else
                    {
                        var frm = new notification.error();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);

                        notification.error.message = "Ceva nu a mers bine, mai incearca!";
                        frm.BringToFront();
                    }
                    foreach (Control control in this.Controls)
                    {
                        if (control.Name == "panel_material")
                        {
                            this.Controls.Remove(control);
                            break;
                        }
                    }
                    schoolsync.hide_loading();
                    close_this();
                }
            }
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            if (navbar_home.page == "EduMentor_vizualizare_->_home")
            {
                navbar_home.page = "EduMentor_editare_->_home";
                navbar_home.use = false;
            }
            else
            {
                navbar_home.page = "EduMentor_editare";
                navbar_home.use = false;
            }
        }
    }
}