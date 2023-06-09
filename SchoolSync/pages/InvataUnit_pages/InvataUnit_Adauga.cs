using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace SchoolSync.pages.InvataUnit_pages
{
    public partial class InvataUnit_Adauga : UserControl
    {
        public InvataUnit_Adauga()
        {
            InitializeComponent();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            navbar_home.use = false;
            navbar_home.page = "InvataUnit";
        }

        private void btn_texteditor(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;
            if (btn.Name == "fontup")
            {
                int x = Convert.ToInt32(richTextBox1.SelectionFont.Size);
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont.FontFamily, ++x, richTextBox1.SelectionFont.Style); ;
            }
            else if (btn.Name == "fontdown")
            {
                int x = Convert.ToInt32(richTextBox1.SelectionFont.Size);
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont.FontFamily, --x, richTextBox1.SelectionFont.Style); ;
            }
            else if (btn.Name == "fontbold")
            {
                if (richTextBox1.SelectionFont.Bold == false)
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Bold);
                else
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Regular);
            }
            else if (btn.Name == "fontitalic")
            {
                if (richTextBox1.SelectionFont.Italic == false)
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Italic);
                else
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Regular);
            }
            else if (btn.Name == "fontunderline")
            {
                if (richTextBox1.SelectionFont.Underline == false)
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Underline);
                else
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Regular);
            }
            else if (btn.Name == "fontleft")
            {
                richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
            }
            else if (btn.Name == "fontcenter")
            {
                richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            }
            else if (btn.Name == "fontright")
            {
                richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
            }
        }

        private void adauga_fisier_Click(object sender, EventArgs e)
        {
            try
            {
                if (flowLayoutPanel1.Controls.Count < 5)
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

                        if (fileSizeibMbs > 5)
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

                            Guna.UI2.WinForms.Guna2Chip guna2Chip = new Guna.UI2.WinForms.Guna2Chip()
                            {
                                FillColor = Color.White,
                                BorderColor = Color.FromArgb(94, 148, 255),
                                ForeColor = Color.Black,
                                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                                AutoRoundedCorners = false,
                                BorderRadius = 10,
                                TextAlign = HorizontalAlignment.Left,
                                Size = new Size(160, 35),
                                Tag = opf.FileName.ToString()
                            };
                            string fnm = Path.GetFileName(opf.FileName);
                            if (fnm.Length >= 16)
                                guna2Chip.Text = fnm.Substring(0, 16) + "...";
                            else
                                guna2Chip.Text = fnm;
                            guna2Chip.Tag = opf.FileName;
                            flowLayoutPanel1.Controls.Add(guna2Chip);
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
            if (richTextBox1.Text.Trim() == "")
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.error.message = "Adaugati intrebarea!";
                frm.BringToFront();
                return;
            }

            schoolsync.show_loading();

            multiple_class _class = new multiple_class();
            string token = _class.generate_token();

            string files = "";
                        
            Dictionary<string, string> data;

            foreach (Control control in flowLayoutPanel1.Controls)
            {
                FileInfo inf = new FileInfo(control.Tag.ToString());

                string token_file = _class.generate_token_250();

                data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("token_user", Convert.ToString(login_signin.login.accounts_user["token"]));
                data.Add("token_file", token_file);
                data.Add("filename", inf.Name);

                _= await _class.new_UploadFileAsync(data, control.Tag.ToString());
                files += (token_file + ";");

            }


            if (navbar_home.page == "InvataUnit_editare" || navbar_home.page == "InvataUnit_editare_->_home")
            {
                string url = "https://schoolsync.nnmadalin.me/api/put.php";
                data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "update invataunit set category = ?, question = ?, files = ? where token = ?");
                var param = new Dictionary<string, string>()
                {
                    {"category", guna2ComboBox2.SelectedItem.ToString()},
                    {"question", richTextBox1.Rtf},
                    {"files", files},
                    {"token", navbar_home.token_page},
                };

                data.Add("params", JsonConvert.SerializeObject(param));
                dynamic task = null;
                try
                {
                    task = await _class.PostRequestAsync(url, data);
                    Console.WriteLine(task);
                    if (task["message"] == "update success")
                    {
                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);

                        schoolsync.hide_loading();

                        notification.success.message = "Material modificat cu succes!";
                        frm.BringToFront();

                        schoolsync.hide_loading();

                        
                    }
                    else
                    {
                        var frm = new notification.error();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.error.message = "Ceva nu a functionat bine!";
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
                };
            }
            else
            {
                string url = "https://schoolsync.nnmadalin.me/api/post.php";
                data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "insert into invataunit(token, token_user, created, category, question, answers, files, favourites) values (?, ?, ?, ?, ?, ?, ?, ?)");
                var param = new Dictionary<string, string>()
                {
                    {"token", token},
                    {"token_user", Convert.ToString(login_signin.login.accounts_user["token"])},
                    {"created", Convert.ToString(login_signin.login.accounts_user["username"])},
                    {"category", guna2ComboBox2.SelectedItem.ToString()},
                    {"question", richTextBox1.Rtf},
                    {"answers", "{}"},
                    {"files", files},
                    {"favourties", ""}
                };

                data.Add("params", JsonConvert.SerializeObject(param));
                dynamic task = null;
                try
                {
                    task = await _class.PostRequestAsync(url, data);

                    if (task["message"] == "insert success")
                    {
                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);

                        schoolsync.hide_loading();

                        notification.success.message = "Material salvat cu succes!";
                        frm.BringToFront();


                        navbar_home.use = false;
                        navbar_home.page = "InvataUnit";
                    }
                    else
                    {
                        var frm = new notification.error();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.error.message = "Eroare: " + task["message"];
                        frm.BringToFront();
                    }

                }
                catch
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Eroare: " + task["message"];
                    frm.BringToFront();
                };
            }
            schoolsync.hide_loading();
        }

        private async void InvataUnit_Adauga_Load(object sender, EventArgs e)
        {
            multiple_class _class = new multiple_class();
            if (navbar_home.page == "InvataUnit_editare")
            {
                label8.Text = "Modifica intrebarea";
                guna2Button1.Text = "Modifica intrebarea";

                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                var data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "select * from invataunit where token = ?");
                var param = new Dictionary<string, string>()
                {
                    {"token", navbar_home.token_page}
                };

                data.Add("params", JsonConvert.SerializeObject(param));
                dynamic task = await _class.PostRequestAsync(url, data);
                
                if(task["message"] == "success")
                {
                    richTextBox1.Rtf = task["0"]["question"];
                    guna2ComboBox2.SelectedIndex = guna2ComboBox2.Items.IndexOf(Convert.ToString(task["0"]["category"]));
                    string[] split = Convert.ToString(task["0"]["files"]).Split(';');
                    for (int i = 0; i < split.Length - 1; i++)
                    {
                        Console.WriteLine(navbar_home.token_page);
                        url = "https://schoolsync.nnmadalin.me/api/get.php";
                        data = new Dictionary<string, string>();
                        data.Add("token", schoolsync.token);
                        data.Add("command", "select * from files where token = ? and token_user = ?");
                        param = new Dictionary<string, string>()
                        {
                            {"token", split[i]},
                            {"token_user", Convert.ToString(login_signin.login.accounts_user["token"])}                           
                        };

                        data.Add("params", JsonConvert.SerializeObject(param));
                        task = await _class.PostRequestAsync(url, data);
                        if (task["message"] == "success")
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
                                Size = new Size(160, 35),
                            };
                            string fnm = task["0"]["name"];
                            if (fnm.Length >= 16)
                                guna2Chip.Text = fnm.Substring(0, 16) + "...";
                            else
                                guna2Chip.Text = fnm;
                            guna2Chip.Tag = task["0"]["token"];
                            flowLayoutPanel1.Controls.Add(guna2Chip);
                        }
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
        }
    }
}
