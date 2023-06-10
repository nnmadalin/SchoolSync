using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace SchoolSync.pages
{
    public partial class InvataUnit : UserControl
    {
        public InvataUnit()
        {
            InitializeComponent();
        }

        private void buton_sortare_materie(object sender, EventArgs e)
        {
            foreach (var control in panel_materii.Controls)
            {
                if(control is Guna.UI2.WinForms.Guna2Button)
                {
                    ((Guna.UI2.WinForms.Guna2Button)control).FillColor = Color.FromArgb(255, 255, 255);
                }
            }

            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            btn.FillColor = Color.FromArgb(225, 225, 225);
            sort = btn.Text;
            load_intrebari_panel();
        }

        private void buton_home_selectat(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2TextBox btn = sender as Guna.UI2.WinForms.Guna2TextBox;
            btn.BorderColor = Color.FromArgb(213, 218, 223);
        }       
        
        private void load_adaugare_intrebare_panel(object sender, EventArgs e)
        {
            navbar_home.use = false;
            navbar_home.page = "InvataUnit_adauga";
        }

        string sort = "";

        void load_intrebari_panel_informare()
        {
            Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
            {
                Size = new Size(925, 137),
                BorderColor = Color.FromArgb(96, 211, 153),
                BorderRadius = 15,
                BorderThickness = 5
            };
            Label lbl = new Label()
            {
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                Location = new Point(28, 9),
                Text = "InvataUnit • Raspuns",
                AutoSize = true
            };
            Label lbl_question = new Label()
            {
                Font = new Font("Segoe UI Black", 35, FontStyle.Regular),
                Location = new Point(21, 19),
                AutoSize = true,
                Text = "Ce vrei să ştii?"
            };
            Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button()
            {
                Text = "Întreabă",
                FillColor = Color.FromArgb(255, 199, 191),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                BorderRadius = 15,
                Size = new Size(187, 36),
                Location = new Point(32, 84)
            };
            btn.Click += load_adaugare_intrebare_panel;
            pnl.Controls.Add(lbl);
            pnl.Controls.Add(lbl_question);
            pnl.Controls.Add(btn);
            this.Controls["flowLayoutPanel1"].Controls.Add(pnl);
        }

        private void intrebare_cu_raspunsuri(object sender, EventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2Button;

            navbar_home.page = "InvataUnit_vizualizare";
            navbar_home.token_page = btn.Tag.ToString();
            navbar_home.use = false;
        }

        async void load_intrebari_panel()
        {
            schoolsync.show_loading();
            this.Controls["flowLayoutPanel1"].Controls.Clear();            

            sort = sort.Trim();
            string answer_sort = guna2ComboBox2.SelectedItem.ToString();

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);

            var param = new Dictionary<string, string>();

            bool use_param = false;

            if (sort.Trim() == "" || sort == "Toate materiile")
            {
                if (answer_sort == "" || answer_sort == "Toate")
                {
                    data.Add("command", "select * from invataunit");
                    if (guna2Button17.BorderThickness == 2)
                    {
                        data["command"] += " where created = ?";
                        param.Add("created", Convert.ToString(login_signin.login.accounts_user["username"]));
                        use_param = true;
                    }
                }
                else
                {
                    if (answer_sort == "Fără răspuns")
                        data.Add("command", "select * from invataunit where LENGTH(answers) = 2");
                    else
                        data.Add("command", "select * from invataunit where LENGTH(answers) > 2");
                    if (guna2Button17.BorderThickness == 2)
                    {
                        data["command"] += " and created = ?";
                        param.Add("created", Convert.ToString(login_signin.login.accounts_user["username"]));
                        use_param = true;
                    }
                }
                data["command"] += " order by data DESC";
            }
            else
            {
                if (answer_sort == "" || answer_sort == "Toate")
                {
                    data.Add("command", "select * from invataunit where category = ?");
                    param.Add("category", sort);
                    use_param = true;
                }
                else
                {
                    if (answer_sort == "Fără răspuns")
                    {
                        data.Add("command", "select * from invataunit where category = ? and LENGTH(answers) = 2");
                        param.Add("category", sort);
                        use_param = true;
                    }
                    else
                    {
                        data.Add("command", "select * from invataunit where category = ? and LENGTH(answers) > 2");
                        param.Add("category", sort);
                        use_param = true;
                    }
                }
                if (guna2Button17.BorderThickness == 2)
                {
                    data["command"] += " and created = ?";
                    param.Add("created", Convert.ToString(login_signin.login.accounts_user["username"]));
                    use_param = true;
                }
                data["command"] += " order by data DESC";
            }

            if(use_param == true)
            {
                data.Add("params", JsonConvert.SerializeObject(param));
            }

            dynamic task = await _class.PostRequestAsync(url, data);
            JObject jb = task;
            if (guna2Button17.BorderThickness != 2)
                load_intrebari_panel_informare();
            else
            {
                Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                {
                    Size = new Size(925, 90),
                    BorderColor = Color.FromArgb(96, 211, 153),
                    BorderRadius = 15,
                    BorderThickness = 5
                };
                Label lbl = new Label()
                {
                    Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                    Location = new Point(28, 9),
                    Text = "InvataUnit • Raspuns",
                    AutoSize = true
                };
                Label lbl_question = new Label()
                {
                    Font = new Font("Segoe UI Black", 35, FontStyle.Regular),
                    Location = new Point(21, 19),
                    AutoSize = true,
                    Text = "Intrebarile tale!"
                };
                pnl.Controls.Add(lbl);
                pnl.Controls.Add(lbl_question);
                this.Controls["flowLayoutPanel1"].Controls.Add(pnl);
            }

            if(task["message"] == "success")
            {
                for (int i = 0; i < jb.Count - 1; i++)
                {                    
                    Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                    {
                        Size = new Size(925, 137),
                        BorderColor = Color.FromArgb(96, 211, 153),
                        BorderRadius = 15,
                        BorderThickness = 2
                    };
                    Label lbl = new Label()
                    {
                        Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                        Location = new Point(28, 20),
                        Text = "",
                        AutoSize = true
                    };
                    Label lbl_question = new Label()
                    {
                        Font = new Font("Segoe UI Semibold", 20, FontStyle.Regular),
                        Location = new Point(25, 39),
                        MaximumSize = new Size(0, 45),
                        AutoSize = true,
                        Text = ""
                    };
                    Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button()
                    {
                        Text = "Răspunde",
                        Tag = "",
                        FillColor = Color.Transparent,
                        ForeColor = Color.Black,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        Cursor = Cursors.Hand,
                        BorderRadius = 15,
                        BorderColor = Color.Black,
                        BorderThickness = 2,
                        Size = new Size(146, 36),
                        Location = new Point(740, 88)
                    };                    

                    string date = task[i.ToString()]["data"]; DateTime dt = Convert.ToDateTime(date);

                    lbl.Text = task[i.ToString()]["created"] + " • " + task[i.ToString()]["category"] + " • " 
                        + dt.Day + "/" + dt.Month  + "/" + dt.Year + " " + Convert.ToDateTime(date).ToShortTimeString();

                    RichTextBox rich = new RichTextBox();
                    rich.Rtf = Convert.ToString(task[i.ToString()]["question"]);

                    lbl_question.Text = rich.Text;

                    if (lbl_question.Text.Length > 55)
                    {
                        lbl_question.Text = lbl_question.Text.Substring(0, 55) + "...";
                    }

                    btn.Tag = task[i.ToString()]["token"];
                    btn.Click += intrebare_cu_raspunsuri;

                    pnl.Controls.Add(lbl);
                    pnl.Controls.Add(lbl_question);
                    pnl.Controls.Add(btn);
                    this.Controls["flowLayoutPanel1"].Controls.Add(pnl);

                }
            }
            schoolsync.hide_loading();
        }

        async void load_favorite_panel()
        {
            schoolsync.show_loading();
            this.Controls["flowLayoutPanel1"].Controls.Clear();

            Guna.UI2.WinForms.Guna2Panel pnl_top = new Guna.UI2.WinForms.Guna2Panel()
            {
                Size = new Size(925, 90),
                BorderColor = Color.FromArgb(96, 211, 153),
                BorderRadius = 15,
                BorderThickness = 5
            };
            Label lbl_top = new Label()
            {
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                Location = new Point(28, 9),
                Text = "InvataUnit • Raspuns",
                AutoSize = true
            };
            Label lbl_question_top = new Label()
            {
                Font = new Font("Segoe UI Black", 35, FontStyle.Regular),
                Location = new Point(21, 19),
                AutoSize = true,
                Text = "Intrebarile tale favorite!"
            };
            pnl_top.Controls.Add(lbl_top);
            pnl_top.Controls.Add(lbl_question_top);
            this.Controls["flowLayoutPanel1"].Controls.Add(pnl_top);

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from invataunit ");

            dynamic task = await _class.PostRequestAsync(url, data);
            if(task["message"] == "success")
            {
                string favorite = task["0"]["favourites"];
                string[] split = favorite.Split(';');

                bool ok = false;

                for (int i = 0; i < split.Length - 1; i++)
                {
                    if(split[i] == Convert.ToString(login_signin.login.accounts_user["token"]))
                    {
                        ok = true;
                        break;
                    }
                }
            
                if(ok == true)
                {
                    Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                    {
                        Size = new Size(925, 137),
                        BorderColor = Color.FromArgb(96, 211, 153),
                        BorderRadius = 15,
                        BorderThickness = 2
                    };
                    Label lbl = new Label()
                    {
                        Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                        Location = new Point(28, 20),
                        Text = "",
                        AutoSize = true
                    };
                    Label lbl_question = new Label()
                    {
                        Font = new Font("Segoe UI Semibold", 20, FontStyle.Regular),
                        Location = new Point(25, 39),
                        AutoSize = true,
                        Text = ""
                    };
                    Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button()
                    {
                        Text = "Vezi intrebare",
                        Tag = "",
                        FillColor = Color.Transparent,
                        ForeColor = Color.Black,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        Cursor = Cursors.Hand,
                        BorderRadius = 15,
                        BorderColor = Color.Black,
                        BorderThickness = 2,
                        Size = new Size(146, 36),
                        Location = new Point(740, 88)
                    };

                    string date = task["0"]["data"]; DateTime dt = Convert.ToDateTime(date);

                    lbl.Text = task["0"]["created"] + " • " + task["0"]["category"] + " • "
                        + dt.Day + "/" + dt.Month + "/" + dt.Year + " " + Convert.ToDateTime(date).ToShortTimeString();

                    RichTextBox rich = new RichTextBox();
                    rich.Rtf = task["0"]["question"];

                    lbl_question.Text = rich.Text;

                    if (lbl_question.Text.Length > 55)
                    {
                        lbl_question.Text = lbl_question.Text.Substring(0, 55) + "...";
                    }

                    btn.Tag = task["0"]["token"];
                    btn.Click += intrebare_cu_raspunsuri;

                    pnl.Controls.Add(lbl);
                    pnl.Controls.Add(lbl_question);
                    pnl.Controls.Add(btn);
                    this.Controls["flowLayoutPanel1"].Controls.Add(pnl);
                }
            }
            schoolsync.hide_loading();
        }

        private void invataunit_Load(object sender, EventArgs e)
        {
            load_intrebari_panel();
        }

        private void panel_materii_Paint(object sender, PaintEventArgs e)
        {

        }

        private void combobox1(object sender, EventArgs e)
        {
            load_intrebari_panel();
        }

        private void home(object sender, EventArgs e)
        {
            guna2Button19.BorderThickness = 2;
            guna2Button17.BorderThickness = 0;
            guna2Button18.BorderThickness = 0;
            load_intrebari_panel();
        }

        private void intrebarile_tale(object sender, EventArgs e)
        {
            guna2Button19.BorderThickness = 0;
            guna2Button17.BorderThickness = 2;
            guna2Button18.BorderThickness = 0;
            load_intrebari_panel();
        }

        private void favorite(object sender, EventArgs e)
        {
            guna2Button19.BorderThickness = 0;
            guna2Button17.BorderThickness = 0;
            guna2Button18.BorderThickness = 2;
            load_favorite_panel();
        }
    }
}
