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
using Newtonsoft.Json.Linq;

namespace SchoolSync.pages.EduClass_pages
{
    public partial class EduClass_Vizualizare_lectie : UserControl
    {
        public EduClass_Vizualizare_lectie()
        {
            InitializeComponent();
        }



        private async void EduClass_Vizualizare_lectie_Load(object sender, EventArgs e)
        {
            schoolsync.show_loading();

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from educlass where token = ?");

            var param = new Dictionary<string, string>()
            {
                {"token", navbar_home.token_page},
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);

            schoolsync.hide_loading();

            if (task["message"] == "success")
            {
                string[] admins = Convert.ToString(task["0"]["admins"]).Split(';');
                bool is_admin = false;

                for (int i = 0; i < admins.Length - 1; i++)
                {
                    if (admins[i] == Convert.ToString(login_signin.login.accounts_user["token"]))
                    {
                        is_admin = true;
                        break;
                    }
                }

                dynamic subjson = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["materials"]));

                JObject json = subjson;

                try
                {
                    label1.Text = subjson[navbar_home.token_page_2]["title"];
                    label4.Text = subjson[navbar_home.token_page_2]["created"] + " • Ultima Modificare: " + subjson[navbar_home.token_page_2]["last_edit"];
                    richTextBox1.Rtf = subjson[navbar_home.token_page_2]["description"];

                    if (Convert.ToString(subjson[navbar_home.token_page_2]["is_homework"]) == "0")
                    {
                        label2.Visible = label3.Visible = false;
                    }
                    else
                    {
                        label2.Text = "Nota: ";
                        label3.Text = "Termen limita: " + subjson[navbar_home.token_page_2]["deadline"];
                    }

                    string row = subjson[navbar_home.token_page_2]["files"];
                    string[] file_split = row.Split(';');

                    string token_user = subjson[navbar_home.token_page_2]["token_user"];
                    for (int i = 0; i < file_split.Length; i++)
                    {

                        url = "https://schoolsync.nnmadalin.me/api/get.php";
                        data = new Dictionary<string, string>();
                        data.Add("token", schoolsync.token);
                        data.Add("command", "select * from files where token_user = ? and token = ?");

                        param = new Dictionary<string, string>()
                        {
                            {"token_user", token_user},
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

                                flowLayoutPanel2.Controls.Add(flp_files_panel);
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

                                panel_file_btn.Tag = token_user + "/" + task["0"]["token"] + "/" + task["0"]["name"];
                                panel_file_btn.Click += deschide_fisier_buton;
                            }
                            catch (Exception ee) { };

                        }
                    }

                }
                catch
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Ceva nu a mers bine!";
                    frm.BringToFront();

                    navbar_home.page = "EduClass_vizualizare";
                    navbar_home.use = false;
                }

            }
            else
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.error.message = "Ceva nu a mers bine!";
                frm.BringToFront();

                navbar_home.page = "EduClass";
                navbar_home.use = false;
            }
        }

        

        private void deschide_fisier_buton(object sender, EventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            System.Diagnostics.Process.Start(@"https://schoolsync.nnmadalin.me/attachments/" + btn.Tag.ToString());
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            navbar_home.page = "EduClass_vizualizare";
            navbar_home.use = false;
        }

        private void richTextBox1_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            ((RichTextBox)sender).Height = e.NewRectangle.Height + 5;
        }
    }
}
