using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SchoolSync.pages.EduClass_pages
{
    public partial class EduClass_Creaza_lectie : UserControl
    {
        public EduClass_Creaza_lectie()
        {
            InitializeComponent();
        }

        private void fontup_Click(object sender, EventArgs e)
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

        private async void adauga_fisier_Click(object sender, EventArgs e)
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

        private async void guna2Button2_Click(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text.Trim() == "")
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.error.message = "Completati caseta titlu!";
                frm.BringToFront();
                return;
            }

            schoolsync.show_loading();

            multiple_class _class = new multiple_class();

            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
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

                JObject json = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["materials"]));

                string token = _class.generate_token();

                string files = "";

                foreach (Control control in flowLayoutPanel1.Controls)
                {
                    FileInfo inf = new FileInfo(control.Tag.ToString());

                    string token_file = _class.generate_token_250();

                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("token_user", Convert.ToString(login_signin.login.accounts_user["token"]));
                    data.Add("token_file", token_file);
                    data.Add("filename", inf.Name);

                    await _class.UploadFileAsync(data, control.Tag.ToString());
                    files += (token_file + ";");

                }

                if (navbar_home.page == "EduClass_editare_curs")
                {
                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "update edumentor set category = ?, title = ?, description = ?, files = ?, reading_time = ? where token = ?");
                    

                    task = null;
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

                            if (navbar_home.page == "EduMentor_editare_->_home")
                            {
                                navbar_home.use = false;
                                navbar_home.page = "EduMentor_vizualizare_->_home";
                            }
                            else
                            {
                                navbar_home.use = false;
                                navbar_home.page = "EduMentor_vizualizare";
                            }
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
                        notification.error.message = "DIMENSIUNE PREA MARE! Nu adauga fisiere de dimensiuni foarte mari in caseta!";
                        frm.BringToFront();
                    };
                }
                else
                {
                    JObject jsonnew = new JObject();
                    jsonnew.Add("created", Convert.ToString(login_signin.login.accounts_user["username"]));
                    jsonnew.Add("token_user", Convert.ToString(login_signin.login.accounts_user["token"]));
                    jsonnew.Add("title", guna2TextBox1.Text.Trim());
                    jsonnew.Add("description", richTextBox1.Rtf.ToString());
                    jsonnew.Add("last_edit", DateTime.Now.ToShortDateString());
                    jsonnew.Add("files", files);
                    if (guna2CheckBox1.Checked == false)
                    {
                        jsonnew.Add("is_homework", "0");
                        jsonnew.Add("deadline", "-1");
                    }
                    else
                    {
                        jsonnew.Add("is_homework", "1");
                        jsonnew.Add("deadline", dateTimePicker1.Value.ToString());
                    }
                    jsonnew.Add("students_files", "");
                    jsonnew.Add("students_note", "");
                    jsonnew.Add("is_deleted", "0");
                    jsonnew.Add("is_visible", "1");

                    if (json == null)
                        json = new JObject();

                    json.Add(json.Count.ToString(), jsonnew);

                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "update educlass set materials = ? where token = ?");
                    param = new Dictionary<string, string>()
                    {
                        {"materials", JsonConvert.SerializeObject(json)},
                        {"token", navbar_home.token_page},
                    };

                    data.Add("params", JsonConvert.SerializeObject(param));
                    task = null;
                    try
                    {
                        task = await _class.PostRequestAsync(url, data);

                        if (task["message"] == "update success")
                        {
                            var frm = new notification.success();
                            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                            panel.Controls.Add(frm);

                            schoolsync.hide_loading();

                            notification.success.message = "Lectie adaugata cu succes!";
                            frm.BringToFront();

                            navbar_home.token_page_2 = (json.Count - 1).ToString();
                            navbar_home.page = "EduClass_vizualizare_lectie";
                            navbar_home.use = false;
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
                        notification.error.message = "DIMENSIUNE PREA MARE! Nu adauga fisiere de dimensiuni foarte mari in caseta!";
                        frm.BringToFront();
                    };
                }
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

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(guna2CheckBox1.Checked == true)
            {
                dateTimePicker1.Visible = true;
                label4.Visible = true;
            }
            else
            {
                dateTimePicker1.Visible = false;
                label4.Visible = false;
            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            if(guna2MessageDialog1.Show() == DialogResult.Yes)
            {
                navbar_home.page = "EduClass_vizualizare";
                navbar_home.use = false;
            }
        }
    }
}
