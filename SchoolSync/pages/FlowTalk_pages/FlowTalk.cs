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
    public partial class FlowTalk : UserControl
    {
        public FlowTalk()
        {
            InitializeComponent();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            navbar_home.page = "FlowTalk_adauga";
            navbar_home.use = false;
        }

        private async void FlowTalk_Load(object sender, EventArgs e)
        {
            schoolsync.show_loading();

            flowLayoutPanel1.Controls.Clear();

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from flowtalk where people like ? order by data ASC");

            var param = new Dictionary<string, string>()
            {
                {"people", "%" + Convert.ToString(login_signin.login.accounts_user["token"]) +"%"}
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);
            if (task["message"] == "success")
            {
                JObject jb = task;
                for(int i = 0; i < jb.Count - 1; i++)
                {
                    Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                    {
                        Size = new Size(273, 68),
                        BorderRadius = 15,
                        UseTransparentBackground = true,
                        FillColor = Color.FromArgb(50, 50, 60),
                        Margin = new Padding(3, 3, 3, 10),
                    };

                    string[] components = Convert.ToString(task[i.ToString()]["color"]).Split(',');
                    int red = int.Parse(components[0]);
                    int green = int.Parse(components[1]);
                    int blue = int.Parse(components[2]);

                    Guna.UI2.WinForms.Guna2CirclePictureBox pct = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                    {
                        Size = new Size(40, 40),
                        Location = new Point(15, 16),
                        FillColor = Color.FromArgb(red, green,blue),
                        UseTransparentBackground = true
                    };
                    Label name = new Label()
                    {
                        Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        Location = new Point(62, 16),
                        ForeColor = Color.White,
                        Size = new Size(208, 22),
                        AutoEllipsis = true,
                        Text = task[i.ToString()]["name"]
                    };
                    Label last_mess = new Label()
                    {
                        Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        Location = new Point(62, 35),
                        ForeColor = Color.FromArgb(207, 207, 207),
                        Size = new Size(208, 24),
                        AutoEllipsis = true,
                    };

                    dynamic sub = JsonConvert.DeserializeObject(Convert.ToString(task[i.ToString()]["messages"]));

                    JObject message = sub;

                    int x = message.Count - 1;
                    if (x >= 0)
                        last_mess.Text = sub[x.ToString()]["text"];

                    pnl.Controls.Add(pct);
                    pnl.Controls.Add(name);
                    pnl.Controls.Add(last_mess);

                    flowLayoutPanel1.Controls.Add(pnl);
                }
            }

            schoolsync.hide_loading();
        }
    }
}
