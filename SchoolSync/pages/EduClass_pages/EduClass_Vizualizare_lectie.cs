using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

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
                        guna2Button3.Visible = guna2Button4.Visible = guna2Button5.Visible = guna2Button6.Visible = true;

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

                    if(richTextBox1.Text.Trim() == "")
                    {
                        guna2Panel1.Visible = false;
                    }

                    if (Convert.ToString(subjson[navbar_home.token_page_2]["is_homework"]) == "0")
                    {
                        label2.Visible = label3.Visible = false;
                        guna2Panel4.Visible = false;
                    }
                    else
                    {
                        label2.Text = "Nota: ";
                        label3.Text = "Termen limita: " + subjson[navbar_home.token_page_2]["deadline"];
                    }
                    string row = subjson[navbar_home.token_page_2]["files"];
                    string[] file_split = row.Split(';');

                    if(file_split.Length == 0 || row == "")
                    {
                        label5.Visible = false;
                    }

                    string datetime = subjson[navbar_home.token_page_2]["deadline"];
                    bool is_over_time = false;
                    if(datetime != "-1")
                    {
                        DateTime dt = Convert.ToDateTime(datetime);
                        if(dt < DateTime.Now)
                        {
                            guna2Button1.Visible = false;
                            guna2Button2.Visible = false;
                            is_over_time = true;
                        }
                    }

                    dynamic subsubjson = subjson[navbar_home.token_page_2];

                    try
                    {
                        string nota = subsubjson["students_note"][Convert.ToString(login_signin.login.accounts_user["token"])];
                        is_over_time = true;

                        if (nota == "0.00" || nota == "0.0" || nota == "0")
                            label2.Text = "Nota: Nu ai primit! :(";
                        else
                        {
                            label2.Text = "Nota: " + nota;
                            guna2Button1.Visible = false;
                            guna2Button2.Visible = false;
                            is_over_time = true;
                        }

                    }
                    catch { label2.Text = "Nota: Nu ai primit! :("; };

                    //incarcare fisiere user

                    try
                    {
                        string item = subsubjson["students_files"][Convert.ToString(login_signin.login.accounts_user["token"])];

                        if (item != null)
                        {
                            string[] split = item.Split(';');
                            if(split.Length == 0)
                            {
                                Label lbl_no_file = new Label()
                                {
                                    Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold),
                                    Text = "Nu ai incarcat fisiere!",
                                    AutoSize = true,
                                };
                                flowLayoutPanel3.Controls.Add(lbl_no_file);
                            }

                            for (int i = 0; i < split.Length - 1; i++)
                            {

                                Guna.UI2.WinForms.Guna2Chip guna2Chip = new Guna.UI2.WinForms.Guna2Chip()
                                {
                                    FillColor = Color.White,
                                    BorderColor = Color.FromArgb(94, 148, 255),
                                    ForeColor = Color.Black,
                                    Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                                    AutoRoundedCorners = false,
                                    BorderRadius = 10,
                                    TextAlign = HorizontalAlignment.Left,
                                    Size = new Size(200, 35),
                                };
                                string item_Str = split[i].ToString();
                                if (item_Str.Length >= 16)
                                    guna2Chip.Text = item_Str.Substring(0, 20) + "...";
                                else
                                    guna2Chip.Text = item_Str;
                                guna2Chip.Tag = item_Str;
                                flowLayoutPanel3.Controls.Add(guna2Chip);
                            }
                        }
                        else
                        {
                            Label lbl_no_file = new Label()
                            {
                                Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold),
                                Text = "Nu ai incarcat fisiere!",
                                AutoSize = true,
                                Name = "nofile"
                            };
                            flowLayoutPanel3.Controls.Add(lbl_no_file);
                        }
                    }
                    catch 
                    {
                        Label lbl_no_file = new Label()
                        {
                            Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold),
                            Text = "Nu ai incarcat fisiere!",
                            AutoSize = true,
                            Name = "nofile"
                        };
                        flowLayoutPanel3.Controls.Add(lbl_no_file);
                    };

                    //afisare fisiere lectie

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
                catch (Exception eee)
                {
                    Console.WriteLine(eee.Message);
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Ceva nu a mers bine! ";
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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (flowLayoutPanel3.Controls.Count < 5)
                {
                    OpenFileDialog opf = new OpenFileDialog();
                    opf.FileName = "";
                    opf.Filter = "Files (*.jpg; *.jpeg; *.png; *.svg; *.webp; *.bmp; *.doc; *.docx; *.ppt; *.pptx; *.xlsx; *.xls; *.txt; *.pdf; *.zip; *.rar) " +
                        "| *.jpg; *.jpeg; *.png; *.svg; *.webp; *.bmp; *.doc; *.docx; *.ppt; *.pptx; *.xlsx; *.xls; *.txt; *.pdf; *.zip; *.rar";
                    DialogResult dir = opf.ShowDialog();


                    if (dir == DialogResult.OK)
                    {

                        FileInfo fl = new FileInfo(opf.FileName);

                        long fileSizeibBytes = fl.Length;
                        long fileSizeibMbs = fileSizeibBytes / (1024 * 1024);

                        if (fileSizeibMbs > 10)
                        {
                            var frm = new notification.error();
                            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                            panel.Controls.Add(frm);
                            notification.error.message = "Fisierul: " + fl.Name.Substring(0, 20) + "..." + " are mai mult de 10 MB!";
                            frm.BringToFront();
                        }
                        else
                        {
                            foreach(Control ctrl in flowLayoutPanel3.Controls)
                            {
                                if(ctrl.Name == "nofile")
                                {
                                    flowLayoutPanel3.Controls.Remove(ctrl);
                                }
                            }
                            Guna.UI2.WinForms.Guna2Chip guna2Chip = new Guna.UI2.WinForms.Guna2Chip()
                            {
                                FillColor = Color.White,
                                BorderColor = Color.FromArgb(94, 148, 255),
                                ForeColor = Color.Black,
                                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                                AutoRoundedCorners = false,
                                BorderRadius = 10,
                                TextAlign = HorizontalAlignment.Left,
                                Size = new Size(200, 35),
                                Tag = opf.FileName.ToString()
                            };
                            string fnm = Path.GetFileName(opf.FileName);
                            if (fnm.Length >= 16)
                                guna2Chip.Text = fnm.Substring(0, 20) + "...";
                            else
                                guna2Chip.Text = fnm;
                            guna2Chip.Tag = opf.FileName;
                            flowLayoutPanel3.Controls.Add(guna2Chip);
                        }
                    }
                }
                else
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Poti adauga maxim 5 fisiere!";
                    frm.BringToFront();
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
            }
        }

        private async void guna2Button1_Click(object sender, EventArgs e)
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

            if (task["message"] == "success")
            {
                dynamic subjson = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["materials"]));
                dynamic subsubjson = subjson[navbar_home.token_page_2]["students_files"];

                JObject jb = new JObject();
                if (Convert.ToString(subsubjson) != "")
                    jb = subsubjson;

                string fnames = "";

                try
                {
                    string item = subsubjson[Convert.ToString(login_signin.login.accounts_user["token"])];
                    if (item != null)
                    {
                        string[] split = item.Split(';');

                        for (int i = 0; i < split.Length - 1; i++)
                        {
                            bool ok = true;
                            foreach (Control ctrl in flowLayoutPanel3.Controls)
                            {
                                if (ctrl.Tag.ToString() == split[i])
                                {
                                    ok = false;
                                    break;
                                }
                            }

                            if(ok == true)
                            {
                                url = "https://schoolsync.nnmadalin.me/api/delete.php";
                                data = new Dictionary<string, string>();
                                data.Add("token", schoolsync.token);
                                data.Add("command", "delete from files where token_user = ? and token = ?");

                                param = new Dictionary<string, string>()
                                    {
                                        {"token_user", Convert.ToString(login_signin.login.accounts_user["token"])},
                                        {"token", split[i]},
                                    };

                                data.Add("params", JsonConvert.SerializeObject(param));

                                task = await _class.PostRequestAsync(url, data);

                                url = "https://schoolsync.nnmadalin.me/api/delete_file.php";
                                data = new Dictionary<string, string>();
                                data.Add("token", schoolsync.token);
                                data.Add("token_user", Convert.ToString(login_signin.login.accounts_user["token"]));
                                data.Add("file", split[i]);

                                task = await _class.PostRequestAsync(url, data);
                            }
                        }

                    }

                }
                catch { };

                try
                {
                    string item = subsubjson[Convert.ToString(login_signin.login.accounts_user["token"])];
                    if (item == null)
                        item = "";
                    string[] split = item.Split(';');
                    foreach (Control control in flowLayoutPanel3.Controls)
                    {
                        bool is_ok = false;
                        for(int i = 0; i < split.Length - 1; i++)
                        {
                            if(split[i] == control.Tag.ToString())
                            {
                                is_ok = true;
                            }
                        }

                        if (is_ok == false)
                        {
                            FileInfo inf = new FileInfo(control.Tag.ToString());

                            string token_file = _class.generate_token_250();

                            data = new Dictionary<string, string>();
                            data.Add("token", schoolsync.token);
                            data.Add("token_user", Convert.ToString(login_signin.login.accounts_user["token"]));
                            data.Add("token_file", token_file);
                            data.Add("filename", inf.Name);

                            await _class.UploadFileAsync(data, control.Tag.ToString());
                            fnames += (token_file + ";");
                        }
                        else
                        {
                            fnames += (control.Tag + ";");
                        }
                    }
                }
                catch 
                {
                    foreach (Control control in flowLayoutPanel3.Controls)
                    {
                        FileInfo inf = new FileInfo(control.Tag.ToString());

                        string token_file = _class.generate_token_250();

                        data = new Dictionary<string, string>();
                        data.Add("token", schoolsync.token);
                        data.Add("token_user", Convert.ToString(login_signin.login.accounts_user["token"]));
                        data.Add("token_file", token_file);
                        data.Add("filename", inf.Name);

                        await _class.UploadFileAsync(data, control.Tag.ToString());
                        fnames += (token_file + ";");
                    }
                };

                try
                {
                    jb.Remove(Convert.ToString(login_signin.login.accounts_user["token"]));
                }
                catch {}
                if(fnames != "")
                    jb.Add(Convert.ToString(login_signin.login.accounts_user["token"]), fnames);

                subjson[navbar_home.token_page_2]["students_files"] = jb;

                url = "https://schoolsync.nnmadalin.me/api/put.php";
                data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "update educlass set materials = ? where token = ?");

                param = new Dictionary<string, string>()
                {
                    {"students_files", JsonConvert.SerializeObject(subjson)},
                    {"token", navbar_home.token_page},
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                task = await _class.PostRequestAsync(url, data);

                if(task["message"] == "update success")
                {
                    var frm = new notification.success();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.success.message = "Tema trimisa!";
                    frm.BringToFront();

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

            schoolsync.hide_loading();
        }

        private async void guna2Button4_Click(object sender, EventArgs e)
        {
            if (guna2MessageDialog1.Show() == DialogResult.Yes)
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

                if (task["message"] == "success")
                {
                    dynamic subjson = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["materials"]));
                    JObject json = subjson;
                    subjson[navbar_home.token_page_2]["is_deleted"] = 1;

                    subjson = json;

                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "update educlass set materials = ? where token = ?");

                    param = new Dictionary<string, string>()
                    {
                        {"materials", JsonConvert.SerializeObject(subjson)},
                        {"token", navbar_home.token_page},
                    };

                    data.Add("params", JsonConvert.SerializeObject(param));

                    task = await _class.PostRequestAsync(url, data);
                    schoolsync.hide_loading();
                    if (task["message"] == "update success")
                    {
                        navbar_home.page = "EduClass_vizualizare";
                        navbar_home.use = false;

                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.success.message = "Lectie stearsa cu succes!";
                        frm.BringToFront();
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

                schoolsync.hide_loading();
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            navbar_home.page = "EduClass_editare_lectie";
            navbar_home.use = false;
        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            navbar_home.page = "EduClass_vizualizare_teme";
            navbar_home.use = false;
        }

        private async void guna2Button6_Click(object sender, EventArgs e)
        {
            bool is_visible = false;

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

            if (task["message"] == "success")
            {
                dynamic subjson = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["materials"]));
                JObject json = subjson;
                if (subjson[navbar_home.token_page_2]["is_visible"] == "1")
                {
                    subjson[navbar_home.token_page_2]["is_visible"] = "0";
                    is_visible = false;
                }
                else
                {
                    subjson[navbar_home.token_page_2]["is_visible"] = "1";
                    is_visible = true;
                }
                subjson = json;

                url = "https://schoolsync.nnmadalin.me/api/put.php";
                data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "update educlass set materials = ? where token = ?");

                param = new Dictionary<string, string>()
                    {
                        {"materials", JsonConvert.SerializeObject(subjson)},
                        {"token", navbar_home.token_page},
                    };

                data.Add("params", JsonConvert.SerializeObject(param));

                task = await _class.PostRequestAsync(url, data);
                schoolsync.hide_loading();
                if (task["message"] == "update success")
                {
                    if(is_visible == true)
                    {
                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.success.message = "Lectie este vizibila!";
                        frm.BringToFront();
                    }
                    else
                    {
                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.success.message = "Lectie nu este vizibila!";
                        frm.BringToFront();
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
                }
            }

            schoolsync.hide_loading();
        }
    }
}
