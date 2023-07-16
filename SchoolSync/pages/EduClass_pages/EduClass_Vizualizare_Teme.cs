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
    public partial class EduClass_Vizualizare_Teme : UserControl
    {
        public EduClass_Vizualizare_Teme()
        {
            InitializeComponent();
        }

        async Task<string> getusername(string token)
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
                return task["0"]["full_name"] + " | " + task["0"]["username"] + " | " + task["0"]["email"];
            }
            else
                return "null";
        }

        private async void trimite_nota(object sender, EventArgs e)
        {
            string token = ((Control)sender).Tag.ToString();


            /*try
            {
                jb.Remove(Convert.ToString(login_signin.login.accounts_user["token"]));
            }
                catch { }
            if (fnames != "")
                jb.Add(Convert.ToString(login_signin.login.accounts_user["token"]), fnames);*/

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
                dynamic subsubjson = subjson[navbar_home.token_page_2]["students_note"];

                JObject jb = new JObject();
                if (Convert.ToString(subsubjson) != "")
                    jb = subsubjson;

                try
                {
                    jb.Remove(token);
                }
                catch {};

                try { 
                
                    string nota = ((Guna.UI2.WinForms.Guna2NumericUpDown)flowLayoutPanel1.Controls[token].Controls["value_nota"]).Value.ToString();
                    if(nota != "0.00")
                        jb.Add(token, nota);

                    subjson[navbar_home.token_page_2]["students_note"] = jb;

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

                    if (task["message"] == "update success")
                    {
                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.success.message = "Nota trimisa!";
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
        }

        private async void EduClass_Vizualizare_Teme_Load(object sender, EventArgs e)
        {
            schoolsync.show_loading();

            flowLayoutPanel1.Controls.Clear();

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

                JObject studentsFiles = (JObject)json[navbar_home.token_page_2]["students_files"];

                if (studentsFiles != null)
                {
                    foreach (KeyValuePair<string, JToken> keyValue in studentsFiles)
                    {
                        string token_user = keyValue.Key;
                        string[] files_user = keyValue.Value.ToString().Split(';');


                        Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                        {
                            Size = new Size(1100, 50),
                            MinimumSize = new Size(1100, 0),
                            MaximumSize = new Size(1100, 0),
                            AutoSize = true,
                            BorderColor = Color.FromArgb(196, 196, 196),
                            BorderThickness = 1,
                            BorderRadius = 15,
                            Margin = new Padding(0, 0, 0, 10),
                            Name = token_user,
                            Tag = token_user,
                        };

                        Guna.UI2.WinForms.Guna2CirclePictureBox gcp = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                        {
                            Size = new Size(40, 40),
                            Location = new Point(10, 5),
                            Cursor = Cursors.Hand,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                        };
                        gcp.Image = await _class.IncarcaAvatar(token_user);

                        Label lbl = new Label()
                        {
                            AutoSize = false,
                            AutoEllipsis = true,
                            Size = new Size(870, 40),
                            Location = new Point(60, 5),
                            TextAlign = ContentAlignment.MiddleLeft,
                            Font = new Font("Segoe UI", 15, FontStyle.Bold),
                        };
                        lbl.Text = await getusername(token_user);

                        Label lbl_nota = new Label()
                        {
                            AutoSize = false,
                            AutoEllipsis = true,
                            Size = new Size(870, 40),
                            Location = new Point(60, 45),
                            TextAlign = ContentAlignment.MiddleLeft,
                            Font = new Font("Segoe UI", 15, FontStyle.Bold),
                            Text = "Nota: ",
                        };

                        Guna.UI2.WinForms.Guna2NumericUpDown gnud = new Guna.UI2.WinForms.Guna2NumericUpDown()
                        {
                            AutoRoundedCorners = true,
                            Font = new Font("Segoe UI", 11),
                            Minimum = 0,
                            Maximum = 10,
                            DecimalPlaces = 2,
                            Value = 5,
                            Size = new Size(100, 30),
                            Location = new Point(150, 50),
                            Tag = token_user,
                            Name = "value_nota",
                        };
                        //afisare nota
                        try
                        {
                            dynamic obj = json[navbar_home.token_page_2]["students_note"];
                            string nota = obj[token_user];
                            gnud.Value = Convert.ToDecimal(nota);
                        }
                        catch { };

                        Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button()
                        {
                            FillColor = Color.FromArgb(152, 152, 181),
                            ForeColor = Color.White,
                            Text = "Trimite nota",
                            Location = new Point(270, 50),
                            Size = new Size(100, 30),
                            AutoRoundedCorners = true,
                            Cursor = Cursors.Hand,
                            Tag = token_user,
                        };
                        btn.Click += trimite_nota;

                        FlowLayoutPanel flp = new FlowLayoutPanel()
                        {
                            Size = new Size(1000, 80),
                            MinimumSize = new Size(1000, 0),
                            MaximumSize = new Size(1000, 0),
                            Location = new Point(50, 90),
                            AutoSize = true,
                        };

                        try
                        {

                            if (keyValue.Value.ToString() != null)
                            {
                                string[] split = files_user;
                                if (split.Length == 0)
                                {
                                    Label lbl_no_file = new Label()
                                    {
                                        Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold),
                                        Text = "Nu a incarcat fisiere!",
                                        AutoSize = true,
                                    };
                                    flp.Controls.Add(lbl_no_file);
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
                                        IsClosable = false,
                                        Cursor = Cursors.Hand,
                                    };
                                    string item_Str = split[i].ToString();
                                    if (item_Str.Length >= 16)
                                        guna2Chip.Text = item_Str.Substring(0, 20) + "...";
                                    else
                                        guna2Chip.Text = item_Str;
                                    guna2Chip.Tag = item_Str;
                                    flp.Controls.Add(guna2Chip);
                                }
                            }
                            else
                            {
                                Label lbl_no_file = new Label()
                                {
                                    Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold),
                                    Text = "Nu a incarcat fisiere!",
                                    AutoSize = true,
                                    Name = "nofile"
                                };
                                flp.Controls.Add(lbl_no_file);
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
                            flp.Controls.Add(lbl_no_file);
                        };

                        pnl.Controls.Add(btn);
                        pnl.Controls.Add(gnud);
                        pnl.Controls.Add(lbl_nota);
                        pnl.Controls.Add(flp);
                        pnl.Controls.Add(gcp);
                        pnl.Controls.Add(lbl);

                        flowLayoutPanel1.Controls.Add(pnl);

                    }
                }

                string[] students = Convert.ToString(task["0"]["students"]).Split(';');
                for(int i = 0; i < students.Length - 1; i++)
                {
                    bool is_ok = false;
                    foreach(Control ctrl in flowLayoutPanel1.Controls)
                    {
                        if(ctrl.Tag.ToString() == students[i])
                        {
                            is_ok = true;
                            break;
                        }
                    }

                    if(is_ok == false)
                    {
                        Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                        {
                            Size = new Size(1100, 50),
                            MinimumSize = new Size(1100, 0),
                            MaximumSize = new Size(1100, 0),
                            AutoSize = true,
                            BorderColor = Color.FromArgb(196, 196, 196),
                            BorderThickness = 1,
                            BorderRadius = 15,
                            Margin = new Padding(0, 0, 0, 10),
                            Name = students[i],
                            Tag = students[i],
                        };

                        Guna.UI2.WinForms.Guna2CirclePictureBox gcp = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                        {
                            Size = new Size(40, 40),
                            Location = new Point(10, 5),
                            Cursor = Cursors.Hand,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                        };
                        gcp.Image = await _class.IncarcaAvatar(students[i]);

                        Label lbl = new Label()
                        {
                            AutoSize = false,
                            AutoEllipsis = true,
                            Size = new Size(870, 40),
                            Location = new Point(60, 5),
                            TextAlign = ContentAlignment.MiddleLeft,
                            Font = new Font("Segoe UI", 15, FontStyle.Bold),
                        };
                        lbl.Text = await getusername(students[i]);

                        Label lbl_nota = new Label()
                        {
                            AutoSize = false,
                            AutoEllipsis = true,
                            Size = new Size(870, 40),
                            Location = new Point(60, 45),
                            TextAlign = ContentAlignment.MiddleLeft,
                            Font = new Font("Segoe UI", 15, FontStyle.Bold),
                            Text = "Nota: ",
                        };

                        Guna.UI2.WinForms.Guna2NumericUpDown gnud = new Guna.UI2.WinForms.Guna2NumericUpDown()
                        {
                            AutoRoundedCorners = true,
                            Font = new Font("Segoe UI", 11),
                            Minimum = 0,
                            Maximum = 10,
                            DecimalPlaces = 2,
                            Value = 5,
                            Size = new Size(100, 30),
                            Location = new Point(150, 50),
                            Tag = students[i],
                            Name = "value_nota",
                        };
                        //afisare nota
                        try
                        {
                            dynamic obj = json[navbar_home.token_page_2]["students_note"];
                            string nota = obj[students[i]];
                            gnud.Value = Convert.ToDecimal(nota);

                        }
                        catch { };

                        Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button()
                        {
                            FillColor = Color.FromArgb(152, 152, 181),
                            ForeColor = Color.White,
                            Text = "Trimite nota",
                            Location = new Point(270, 50),
                            Size = new Size(100, 30),
                            AutoRoundedCorners = true,
                            Cursor = Cursors.Hand,
                            Tag = students[i],
                        };
                        btn.Click += trimite_nota;

                        FlowLayoutPanel flp = new FlowLayoutPanel()
                        {
                            Size = new Size(1000, 80),
                            MinimumSize = new Size(1000, 0),
                            MaximumSize = new Size(1000, 0),
                            Location = new Point(50, 90),
                            AutoSize = true,
                        };

                        Label lbl_no_file = new Label()
                        {
                            Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold),
                            Text = "Nu a incarcat fisiere!",
                            AutoSize = true,
                            Name = "nofile"
                        };
                        flp.Controls.Add(lbl_no_file);

                        pnl.Controls.Add(btn);
                        pnl.Controls.Add(gnud);
                        pnl.Controls.Add(lbl_nota);
                        pnl.Controls.Add(flp);
                        pnl.Controls.Add(gcp);
                        pnl.Controls.Add(lbl);

                        flowLayoutPanel1.Controls.Add(pnl);
                    }
                }

                students = Convert.ToString(task["0"]["admins"]).Split(';');
                for (int i = 0; i < students.Length - 1; i++)
                {
                    bool is_ok = false;
                    foreach (Control ctrl in flowLayoutPanel1.Controls)
                    {
                        if (ctrl.Tag.ToString() == students[i])
                        {
                            is_ok = true;
                            break;
                        }
                    }

                    if (is_ok == false)
                    {
                        Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                        {
                            Size = new Size(1100, 50),
                            MinimumSize = new Size(1100, 0),
                            MaximumSize = new Size(1100, 0),
                            AutoSize = true,
                            BorderColor = Color.FromArgb(196, 196, 196),
                            BorderThickness = 1,
                            BorderRadius = 15,
                            Margin = new Padding(0, 0, 0, 10),
                            Name = students[i],
                            Tag = students[i],
                        };

                        Guna.UI2.WinForms.Guna2CirclePictureBox gcp = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                        {
                            Size = new Size(40, 40),
                            Location = new Point(10, 5),
                            Cursor = Cursors.Hand,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                        };
                        gcp.Image = await _class.IncarcaAvatar(students[i]);

                        Label lbl = new Label()
                        {
                            AutoSize = false,
                            AutoEllipsis = true,
                            Size = new Size(870, 40),
                            Location = new Point(60, 5),
                            TextAlign = ContentAlignment.MiddleLeft,
                            Font = new Font("Segoe UI", 15, FontStyle.Bold),
                        };
                        lbl.Text = await getusername(students[i]);

                        Label lbl_nota = new Label()
                        {
                            AutoSize = false,
                            AutoEllipsis = true,
                            Size = new Size(870, 40),
                            Location = new Point(60, 45),
                            TextAlign = ContentAlignment.MiddleLeft,
                            Font = new Font("Segoe UI", 15, FontStyle.Bold),
                            Text = "Nota: ",
                        };

                        Guna.UI2.WinForms.Guna2NumericUpDown gnud = new Guna.UI2.WinForms.Guna2NumericUpDown()
                        {
                            AutoRoundedCorners = true,
                            Font = new Font("Segoe UI", 11),
                            Minimum = 0,
                            Maximum = 10,
                            DecimalPlaces = 2,
                            Value = 5,
                            Size = new Size(100, 30),
                            Location = new Point(150, 50),
                            Tag = students[i],
                            Name = "value_nota",
                        };
                        //afisare nota
                        try
                        {
                            dynamic obj = json[navbar_home.token_page_2]["students_note"];
                            string nota = obj[students[i]];
                            gnud.Value = Convert.ToDecimal(nota);

                        }
                        catch { };

                        Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button()
                        {
                            FillColor = Color.FromArgb(152, 152, 181),
                            ForeColor = Color.White,
                            Text = "Trimite nota",
                            Location = new Point(270, 50),
                            Size = new Size(100, 30),
                            AutoRoundedCorners = true,
                            Cursor = Cursors.Hand,
                            Tag = students[i],
                        };
                        btn.Click += trimite_nota;

                        FlowLayoutPanel flp = new FlowLayoutPanel()
                        {
                            Size = new Size(1000, 80),
                            MinimumSize = new Size(1000, 0),
                            MaximumSize = new Size(1000, 0),
                            Location = new Point(50, 90),
                            AutoSize = true,
                        };

                        Label lbl_no_file = new Label()
                        {
                            Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold),
                            Text = "Nu a incarcat fisiere!",
                            AutoSize = true,
                            Name = "nofile"
                        };
                        flp.Controls.Add(lbl_no_file);

                        pnl.Controls.Add(btn);
                        pnl.Controls.Add(gnud);
                        pnl.Controls.Add(lbl_nota);
                        pnl.Controls.Add(flp);
                        pnl.Controls.Add(gcp);
                        pnl.Controls.Add(lbl);

                        flowLayoutPanel1.Controls.Add(pnl);
                    }
                }

            }
            schoolsync.hide_loading();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
