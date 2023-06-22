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

namespace SchoolSync.pages.FlowTalk_pages
{
    public partial class FlowTalk_Adauga : UserControl
    {
        public FlowTalk_Adauga()
        {
            InitializeComponent();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            navbar_home.page = "FlowTalk";
            navbar_home.use = false;
        }

        private async void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            string nume_cautat = guna2TextBox2.Text;

           
        }

        private async  void FlowTalk_Adauga_Load(object sender, EventArgs e)
        {
            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from accounts");
            dynamic task = await _class.PostRequestAsync(url, data);
            if (task["message"] == "success")
            {
                AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
                JObject jb = task;
                for (int i = 0; i < jb.Count - 1; i++)
                {
                    string username = Convert.ToString(task[i.ToString()]["username"]);
                    collection.Add(username);

                }
                guna2TextBox2.AutoCompleteCustomSource = collection;
            }
        }

        async void add_user()
        {
            if (guna2TextBox2.Text.Trim() != "")
            {
                Guna.UI2.WinForms.Guna2Chip chip = new Guna.UI2.WinForms.Guna2Chip()
                {
                    Size = new Size(200, 50),
                    IsClosable = true,
                    AutoSize = true,
                    TextAlign = HorizontalAlignment.Left,
                    FillColor = Color.Transparent,
                    BorderColor = Color.FromArgb(25, 133, 255),
                    ForeColor = Color.Black,
                    BorderThickness = 2,
                    AutoRoundedCorners = false,
                    BorderRadius = 5,
                };
                multiple_class _class = new multiple_class();
                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                var data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "select * from accounts where username like ?");

                var param = new Dictionary<string, string>()
                {
                    {"username", "%" + guna2TextBox2.Text +"%"}
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                dynamic task = await _class.PostRequestAsync(url, data);
                if (task["message"] == "success")
                {
                    chip.Tag = task["0"]["token"];
                    chip.Text = task["0"]["username"];

                    if (task["0"]["token"] == login_signin.login.accounts_user["token"])
                    {
                        var frm = new notification.error();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.error.message = "Nu poti sa te adaugi pe tine :) !";
                        frm.BringToFront();
                    }
                    else
                    {
                        bool ok = false;
                        foreach (Control ctrl in flowLayoutPanel1.Controls)
                        {
                            if (ctrl.Tag.ToString() == Convert.ToString(task["0"]["token"]))
                            {
                                var frm = new notification.error();
                                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                                panel.Controls.Add(frm);
                                notification.error.message = "Ai adaugat deja aceasta persoana!";
                                frm.BringToFront();
                                ok = true;
                                break;
                            }
                        }
                        if (ok == false)
                        {
                            flowLayoutPanel1.Controls.Add(chip);
                            guna2TextBox2.Clear();
                        }
                    }
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            add_user();
            
        }

        private async void guna2Button2_Click(object sender, EventArgs e)
        {
            schoolsync.show_loading();
            string name_group = guna2TextBox1.Text;
            string persoane_token = Convert.ToString(login_signin.login.accounts_user["token"]) + ";";
            if(guna2TextBox1.Text.Trim() == "")
            {
                name_group = "";
                foreach (Control ctrl in flowLayoutPanel1.Controls)
                {
                    if(name_group == "")
                    {
                        name_group = ctrl.Text.ToString();
                    }
                    else
                    {
                        name_group += ("& " + ctrl.Text.ToString());
                    }
                }
            }
            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                persoane_token += (ctrl.Tag.ToString() + ";");
            }

            Guna.UI2.WinForms.Guna2Chip chip = new Guna.UI2.WinForms.Guna2Chip()
            {
                Size = new Size(200, 50),
                IsClosable = true,
                AutoSize = true,
                TextAlign = HorizontalAlignment.Left,
                FillColor = Color.Transparent,
                BorderColor = Color.FromArgb(25, 133, 255),
                BorderThickness = 2,
                AutoRoundedCorners = false,
                BorderRadius = 5,
            };

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/post.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "insert into flowtalk(token, token_user, created, name, color, people, messages, seen, admins) values (?, ?, ?, ?, ?, ? ,?, ?, ?)");

            string token = _class.generate_token();

            JObject sub_json = new JObject();
            JObject jbo = new JObject();

            sub_json.Add("root", "1");
            sub_json.Add("user", "");
            sub_json.Add("user_token", "");
            sub_json.Add("date", DateTime.Now.ToString());
            sub_json.Add("file", "");
            sub_json.Add("text", Convert.ToString(login_signin.login.accounts_user["username"]) + " a creat grupul: '" + name_group + "'");
            sub_json.Add("is_deleted", "0");
            jbo.Add("0", sub_json);

            Random random = new Random();
            string randomColor = random.Next(256).ToString() + ", " + random.Next(256).ToString() + ", " + random.Next(256).ToString();

            var param = new Dictionary<string, string>()
            {
                {"token", token},
                {"token_user", Convert.ToString(login_signin.login.accounts_user["token"])},
                {"created", Convert.ToString(login_signin.login.accounts_user["username"])},
                {"name", name_group},
                {"color", randomColor.ToString()},
                {"people", persoane_token},
                {"messages", JsonConvert.SerializeObject(jbo)},
                {"seen", ""},
                {"admins", Convert.ToString(login_signin.login.accounts_user["token"]) + ";"},
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);
            schoolsync.hide_loading();
            if (task["message"] == "insert success")
            {
                var frm = new notification.success();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.success.message = "Conversatie creata cu succes!";
                frm.BringToFront();
                navbar_home.page = "FlowTalk";
                navbar_home.use = false;
            }
            else
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.error.message = "Ceva nu mers bine, mai incearca!";
                frm.BringToFront();
            }
        }
    }
}
