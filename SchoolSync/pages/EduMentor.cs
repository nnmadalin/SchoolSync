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
            Label title = new Label() 
            { 
                Text = "Adauga un material nou!",
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold),
                Location = new Point(60, 20),
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
            };

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
            };

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

            pnl.Controls.Add(title);

            pnl.Controls.Add(lbl_titlu);
            pnl.Controls.Add(txt_titlu);

            pnl.Controls.Add(lbl_descriere);
            pnl.Controls.Add(txt_descriere);

            pnl.Controls.Add(btn_fisier);
            pnl.Controls.Add(flp_fisiere);

            pnl_fullpage.Controls.Add(pnl);
          
            this.Controls.Add(pnl_fullpage);
            pnl_fullpage.BringToFront();
        }
    }
}
