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

namespace SchoolSync.pages
{
    public partial class EduMentor : UserControl
    {
        public EduMentor()
        {
            InitializeComponent();
        }

        async void load_panel()
        {
            flowLayoutPanel1.Controls.Clear();
        }

        private void EduMentor_Load(object sender, EventArgs e)
        {
            load_panel();
        }
        private void adauga_fisier(object sender, EventArgs e)
        {
            if (this.Controls["pnl_fullpage"].Controls["pnl"].Controls["flp_fisiere"].Controls.Count < 5)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.FileName = "";
                opf.Filter = "Files (*.jpg; *.jpeg; *.png; *.svg; *.webp; *.bmp; *.doc; *.docx; *.ppt; *.pptx; *.xlsx; *.xls; *.txt; *.pdf) " +
                    "| *.jpg; *.jpeg; *.png; *.svg; *.webp; *.bmp; *.doc; *.docx; *.ppt; *.pptx; *.xlsx; *.xls; *.txt; *.pdf";
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
                            BorderColor = Color.White,
                            ForeColor = Color.Black,
                            Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                            AutoRoundedCorners = false,
                            BorderRadius = 10,
                            TextAlign = HorizontalAlignment.Left,
                            Size = new Size(160, 35),                            
                            Tag = opf.FileName.ToString()
                        };
                        if (opf.FileName.Length > 16)
                            guna2Chip.Text = System.IO.Path.GetFileName(opf.FileName).Substring(0, 16) + "...";
                        else
                            guna2Chip.Text = opf.FileName;

                        this.Controls["pnl_fullpage"].Controls["pnl"].Controls["flp_fisiere"].Controls.Add(guna2Chip);
                    }
                }
            }
            else
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                frm.Location = new Point(840, 50);
                frm.Show();
                notification.error.message = "Poti adauga maxim 5 fisiere!";
                frm.BringToFront();
            }
        }
        
        private void inchide_panel_adauga_material(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if(control.Name == "pnl_fullpage")
                {
                    this.Controls.Remove(control);
                    break;
                }
            }
            load_panel();
        }

        private void border_none(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2TextBox btn = sender as Guna.UI2.WinForms.Guna2TextBox;
            btn.BorderColor = Color.FromArgb(213, 218, 223);
        }

        private async void trimite_material_api(object sender, EventArgs e)
        {
            if (this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_titlu"].Text.Trim() == "")
            {
                ((Guna.UI2.WinForms.Guna2TextBox)this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_titlu"]).BorderColor = Color.Red;
            }
            else if (this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_descriere"].Text.Trim() == "")
            {
                ((Guna.UI2.WinForms.Guna2TextBox)this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_descriere"]).BorderColor = Color.Red;
            }
            else
            {
                schoolsync.show_loading();

                multiple_class _class = new multiple_class();
                string token = _class.generate_token();


                Control combobox = this.Controls["pnl_fullpage"].Controls["pnl"].Controls["combobox_materii"];
                Control txt_title = this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_titlu"];
                Control txt_descriere = this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_descriere"];
                Control gnu = this.Controls["pnl_fullpage"].Controls["pnl"].Controls["gnu"];

                string files = "";

                Random rand = new Random();
                string random_color = rand.Next(256) + ", " + rand.Next(256) + ", " + rand.Next(256);

                foreach (Control control in this.Controls["pnl_fullpage"].Controls["pnl"].Controls["flp_fisiere"].Controls)
                {
                    string token_file = await _class.UploadFileAsync(control.Tag.ToString());
                    if (token_file != null)
                        files += (token_file + ";");
                }

                string url = "https://schoolsync.nnmadalin.me/api/post.php";
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("sql", string.Format("insert into edumentor(token, token_user, created, category, color, title, description, files, reading_time, users_hearts) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')",
                    token, login_signin.login.accounts_user["token"], login_signin.login.accounts_user["username"], ((ComboBox)combobox).SelectedItem, random_color, txt_title.Text.Trim(), txt_descriere.Text.Trim(), files, ((Guna.UI2.WinForms.Guna2NumericUpDown)gnu).Value, ""));

                dynamic task = await _class.PostRequestAsync(url, data);
                if (task["message"] == "insert success")
                {
                    var frm = new notification.success();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);

                    notification.success.message = "Material salvat cu succes!";
                    frm.BringToFront();
                    foreach (Control control in this.Controls)
                    {
                        if (control.Name == "pnl_fullpage")
                        {
                            this.Controls.Remove(control);
                            load_panel();
                            break;
                        }
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
        }

        private void adauga_material(object sender, EventArgs e)
        {
            Panel pnl_fullpage = new Panel()
            {
                Size = new Size(1192, 690),
                BackColor = Color.FromArgb(133, 135, 237),
                Name = "pnl_fullpage"
            };
            Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
            {
                Size = new Size(950, 650),
                Location = new Point(120, 20),
                FillColor = Color.FromArgb(93, 94, 165),
                BorderRadius = 20,
                UseTransparentBackground = true,
                Name = "pnl"
            };

            Guna.UI2.WinForms.Guna2CircleButton btn_inchide_panel = new Guna.UI2.WinForms.Guna2CircleButton()
            {
                Size = new Size(35, 35),
                BorderColor = Color.White,
                BorderThickness = 2,
                UseTransparentBackground = true,
                FillColor = Color.White,
                Image = SchoolSync.Properties.Resources.close_FILL1_wght700_GRAD0_opsz48,
                ImageAlign = HorizontalAlignment.Center,
                Location = new Point(900, 20),
                Cursor = Cursors.Hand
            };
            btn_inchide_panel.Click += inchide_panel_adauga_material;

            Label title = new Label() 
            { 
                Text = "Adauga un material nou!",
                Font = new Font("Segoe UI Semibold", 17, FontStyle.Bold),
                Location = new Point(40, 20),
                ForeColor = Color.White,
                AutoSize = true,
            };

            Label lbl_titlu = new Label()
            {
                Text = "Titlu:",
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                Location = new Point(20, 70),
                ForeColor = Color.White,
                AutoSize = true,
            };
            Guna.UI2.WinForms.Guna2TextBox txt_titlu = new Guna.UI2.WinForms.Guna2TextBox()
            {
                Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                Size = new Size(900, 40),
                Location = new Point(20, 100),
                BorderRadius = 10,
                PlaceholderText = "Adauga titlu materialului!",
                ForeColor = Color.Black,
                Name = "txt_titlu",
            };
            txt_titlu.TextChanged += border_none;

            Label lbl_descriere = new Label()
            {
                Text = "Descriere:",
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                Location = new Point(20, 150),
                ForeColor = Color.White,
                AutoSize = true,
            };
            Guna.UI2.WinForms.Guna2TextBox txt_descriere = new Guna.UI2.WinForms.Guna2TextBox()
            {
                Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                Size = new Size(900, 250),
                Location = new Point(20, 180),
                BorderRadius = 10,
                PlaceholderText = "Adauga o descriere materialului!",
                ForeColor = Color.Black,
                Multiline = true, 
                AutoScroll = true, 
                ScrollBars = ScrollBars.Vertical,
                Name = "txt_descriere",
            };
            txt_descriere.TextChanged += border_none;

            Guna.UI2.WinForms.Guna2CircleButton btn_fisier = new Guna.UI2.WinForms.Guna2CircleButton()
            {
                Size = new Size(35, 35),
                BorderColor = Color.White,                
                BorderThickness = 2,
                UseTransparentBackground = true,
                FillColor = Color.White,
                Image = SchoolSync.Properties.Resources.attach_file_FILL1_wght700_GRAD0_opsz48,
                ImageAlign = HorizontalAlignment.Center,
                Location = new Point(20, 450),
                Cursor = Cursors.Hand
            };

            btn_fisier.Click += adauga_fisier;

            FlowLayoutPanel flp_fisiere = new FlowLayoutPanel()
            {
                Location = new Point(60, 450),
                Size = new Size(850, 40),
                Name = "flp_fisiere"
            };

            Guna.UI2.WinForms.Guna2ComboBox cmb = new Guna.UI2.WinForms.Guna2ComboBox()
            {
                BorderRadius = 15,
                Size = new Size(200, 50),
                Location = new Point(20, 500),
                Items = { "Limba română", "Matematică", "Istorie", "Chimie", "Biologie", "Fizică", "Geografie",
                    "Studii sociale", "Informatică", "Engleza", "Franceza", "Alte limbi", "Ed. tehnologică", "Arte", "Ed. muzicală" },
                FillColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Name = "combobox_materii",
                Cursor = Cursors.Hand,
            };
            cmb.SelectedIndex = 0;

            Label lbl_time = new Label()
            {
                Text = "Cat dureaza sa fie citit materialul? (min)",
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                Location = new Point(20, 550),
                ForeColor = Color.White,
                AutoSize = true,
            };
            Guna.UI2.WinForms.Guna2NumericUpDown gnu = new Guna.UI2.WinForms.Guna2NumericUpDown()
            {
                Minimum = 2,
                Maximum = 999,
                Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                Size = new Size(90, 30),
                Location = new Point(370, 550),
                Value = 2,
                BorderRadius = 5,
                Name = "gnu"
            };

            Guna.UI2.WinForms.Guna2Button btn_trimite = new Guna.UI2.WinForms.Guna2Button()
            {
                Location = new Point(20, 600),
                Text = "Adauga material!",
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                Size = new Size(180, 40),
                FillColor = Color.FromArgb(107, 150, 108),
                BorderRadius = 10,
                Cursor = Cursors.Hand,
            };
            btn_trimite.Click += trimite_material_api;

            pnl.Controls.Add(title);
            pnl.Controls.Add(btn_inchide_panel);

            pnl.Controls.Add(lbl_titlu);
            pnl.Controls.Add(txt_titlu);

            pnl.Controls.Add(lbl_descriere);
            pnl.Controls.Add(txt_descriere);

            pnl.Controls.Add(btn_fisier);
            pnl.Controls.Add(flp_fisiere);
            
            pnl.Controls.Add(cmb);
            pnl.Controls.Add(lbl_time);
            pnl.Controls.Add(gnu);
            
            pnl.Controls.Add(btn_trimite);

            pnl_fullpage.Controls.Add(pnl);
          
            this.Controls.Add(pnl_fullpage);
            pnl_fullpage.BringToFront();
        }
    }
}
