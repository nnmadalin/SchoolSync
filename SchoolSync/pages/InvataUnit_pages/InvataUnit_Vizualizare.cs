using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace SchoolSync.pages.InvataUnit_pages
{
    public partial class InvataUnit_Vizualizare : UserControl
    {
        public InvataUnit_Vizualizare()
        {
            InitializeComponent();
        }

        private void deschide_fisier_buton(object sender, EventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            System.Diagnostics.Process.Start(@"https://schoolsync.nnmadalin.me/attachments/" + btn.Tag.ToString());
        }

        private async void InvataUnit_Vizualizare_Load(object sender, EventArgs e)
        {
            multiple_class _Class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from invataunit where token = ?");

            var param = new Dictionary<string, string>()
            {
                {"token", navbar_home.token_page},
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _Class.PostRequestAsync(url, data);
            if (task["message"] == "success")
            {
                string str = task["0"]["favourites"];
                string[] split = str.Split(';');
                for (int i = 0; i < split.Length - 1; i++)
                {
                    if (split[i] == login_signin.login.accounts_user["token"])
                    {
                        guna2Button1.Image = SchoolSync.Properties.Resources.favorite_FILL1_wght700_GRAD0_opsz48;
                        guna2Button1.Tag = "1";
                        break;
                    }
                }

                label1.Text = task["0"]["created"];
                label2.Text = task["0"]["category"] + " • " + task["0"]["data"];
                richTextBox1.Rtf = task["0"]["question"];

                if (task["0"]["files"] == "")
                {
                    label4.Visible = false;
                }
                else
                {
                    string row = task["0"]["files"].ToString();
                    string[] file_split = row.Split(';');

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

                        task = await _Class.PostRequestAsync(url, data);
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

                if (task["0"]["token_user"] == login_signin.login.accounts_user["token"])
                {
                    guna2CircleButton2.Visible = true;
                    guna2CircleButton3.Visible = true;
                }
            }
        }

        private void richTextBox1_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            ((RichTextBox)sender).Height = e.NewRectangle.Height + 5;
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            navbar_home.page = "InvataUnit";
            navbar_home.use = false;
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            navbar_home.page = "InvataUnit_editare";
            navbar_home.use = false;
        }

        private async void guna2CircleButton3_Click(object sender, EventArgs e)
        {
            string token = navbar_home.token_page;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
            guna2MessageDialog1.Caption = "Sterge intrebare!";
            guna2MessageDialog1.Text = "Esti sigur ca vrei sa stergi intrebarea?";
            DialogResult dr = guna2MessageDialog1.Show();
            if (dr == DialogResult.Yes)
            {
                schoolsync.show_loading();

                multiple_class _Class = new multiple_class();

                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "select * from invataunit where token = ?");
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
                    data.Add("command", "delete from invataunit where token = ?");

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

                        notification.success.message = "Intrebare stersa cu succes!";
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
                    schoolsync.hide_loading();
                    navbar_home.page = "InvataUnit";
                    navbar_home.use = false;
                }
            }
        }
    }
}
