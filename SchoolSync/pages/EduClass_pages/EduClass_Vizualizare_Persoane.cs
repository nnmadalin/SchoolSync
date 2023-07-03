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

namespace SchoolSync.pages.EduClass_pages
{
    public partial class EduClass_Vizualizare_Persoane : UserControl
    {
        public EduClass_Vizualizare_Persoane()
        {
            InitializeComponent();
        }

        string token_local = "";
        
        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            navbar_home.token_page = token_local;
            navbar_home.page = "EduClass_vizualizare";
            navbar_home.use = false;
        }

        async Task<string> getusername (string token)
        {
            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from accounts where token = ?");

            var param = new Dictionary<string, string>()
            {
                {"token", token},
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);

            if (task["message"] == "success")
            {
                return task["0"]["username"];
            }
            else
                return "null";
        }

        async void deschide_profil(object sender, EventArgs e)
        {
            string token = ((Control)sender).Tag.ToString();

            navbar_home.token_page = token;
            navbar_home.page = "Profil_person";
            navbar_home.use = false;
        }

        bool is_admin = false;

        private async void EduClass_Vizualizare_Persoane_Load(object sender, EventArgs e)
        {
            token_local = navbar_home.token_page;

            schoolsync.show_loading();

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from educlass where token = ?");

            var param = new Dictionary<string, string>()
            {
                {"token", token_local},
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);

            schoolsync.hide_loading();

            if (task["message"] == "success")
            {
                string[] admins = Convert.ToString(task["0"]["admins"]).Split(';');

                for (int i = 0; i < admins.Length - 1; i++)
                {
                    if (admins[i] == Convert.ToString(login_signin.login.accounts_user["token"]))
                    {
                        is_admin = true;
                        break;
                    }
                }

                if (is_admin == false)
                {
                    guna2Panel1.Visible = false;
                }

                //incarcare studenti / admini / pending

                //admini
                string[] admini = Convert.ToString(task["0"]["admins"]).Split(';');
                for(int i = 0; i < admins.Length - 1; i++)
                {
                    Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                    {
                        Size = new Size(990, 50),
                        BorderColor = Color.FromArgb(196, 196, 196),
                        BorderThickness = 1,
                        BorderRadius = 15,
                        Margin = new Padding(0, 0, 0, 10),
                        Cursor = Cursors.Hand,
                    };
                    pnl.Tag = admini[i];
                    pnl.Click += deschide_profil;

                    Guna.UI2.WinForms.Guna2CirclePictureBox gcp = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                    {
                        Size = new Size(40, 40),
                        Location = new Point(10, 5),
                        Cursor = Cursors.Hand,
                    };
                    gcp.Image = await _class.IncarcaAvatar(admini[i]);
                    gcp.Tag = admini[i];
                    gcp.Click += deschide_profil;

                    Label lbl = new Label()
                    {
                        AutoSize = false,
                        AutoEllipsis = true,
                        Size = new Size(870, 40),
                        Location = new Point(60, 5),
                        TextAlign = ContentAlignment.MiddleLeft,
                        Cursor = Cursors.Hand,
                        Font = new Font("Segoe UI", 15, FontStyle.Bold),
                    };
                    lbl.Text = await getusername(admini[i]);
                    lbl.Tag = admini[i];
                    lbl.Click += deschide_profil;

                    pnl.Controls.Add(gcp);
                    pnl.Controls.Add(lbl);

                    flowLayoutPanel2.Controls.Add(pnl);
                }

                //pending
                string[] pending = Convert.ToString(task["0"]["pending"]).Split(';');
                for (int i = 0; i < pending.Length - 1; i++)
                {
                    Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                    {
                        Size = new Size(990, 50),
                        BorderColor = Color.FromArgb(196, 196, 196),
                        BorderThickness = 1,
                        BorderRadius = 15,
                        Margin = new Padding(0, 0, 0, 10),
                        Cursor = Cursors.Hand,
                    };
                    pnl.Tag = pending[i];
                    pnl.Click += deschide_profil;

                    Guna.UI2.WinForms.Guna2CirclePictureBox gcp = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                    {
                        Size = new Size(40, 40),
                        Location = new Point(10, 5),
                        Cursor = Cursors.Hand,
                    };
                    gcp.Image = await _class.IncarcaAvatar(pending[i]);
                    gcp.Tag = pending[i];
                    gcp.Click += deschide_profil;

                    Label lbl = new Label()
                    {
                        AutoSize = false,
                        AutoEllipsis = true,
                        Size = new Size(870, 40),
                        Location = new Point(60, 5),
                        TextAlign = ContentAlignment.MiddleLeft,
                        Cursor = Cursors.Hand,
                        Font = new Font("Segoe UI", 15, FontStyle.Bold),
                    };
                    lbl.Text = await getusername(pending[i]);
                    lbl.Tag = pending[i];
                    lbl.Click += deschide_profil;

                    pnl.Controls.Add(gcp);
                    pnl.Controls.Add(lbl);

                    flowLayoutPanel3.Controls.Add(pnl);
                }

                //students
                string[] students = Convert.ToString(task["0"]["students"]).Split(';');
                for (int i = 0; i < students.Length - 1; i++)
                {
                    Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                    {
                        Size = new Size(990, 50),
                        BorderColor = Color.FromArgb(196, 196, 196),
                        BorderThickness = 1,
                        BorderRadius = 15,
                        Margin = new Padding(0, 0, 0, 10),
                        Cursor = Cursors.Hand,
                    };
                    pnl.Tag = students[i];
                    pnl.Click += deschide_profil;

                    Guna.UI2.WinForms.Guna2CirclePictureBox gcp = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                    {
                        Size = new Size(40, 40),
                        Location = new Point(10, 5),
                        Cursor = Cursors.Hand,
                    };
                    gcp.Image = await _class.IncarcaAvatar(students[i]);
                    gcp.Tag = students[i];
                    gcp.Click += deschide_profil;

                    Label lbl = new Label()
                    {
                        AutoSize = false,
                        AutoEllipsis = true,
                        Size = new Size(870, 40),
                        Location = new Point(60, 5),
                        TextAlign = ContentAlignment.MiddleLeft,
                        Cursor = Cursors.Hand,
                        Font = new Font("Segoe UI", 15, FontStyle.Bold),
                    };
                    lbl.Text = await getusername(students[i]);
                    lbl.Tag = students[i];
                    lbl.Click += deschide_profil;

                    pnl.Controls.Add(gcp);
                    pnl.Controls.Add(lbl);

                    flowLayoutPanel4.Controls.Add(pnl);
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

                navbar_home.page = "EduClass_vizualizare";
                navbar_home.use = false;
            }
        }
    }
}
