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

                    if (Convert.ToString(subjson[navbar_home.token_page_2]["is_homework"]) == "0")
                    {
                        label2.Visible = label3.Visible = false;
                    }
                    else
                    {
                        label2.Text = "Nota: ";
                        label3.Text = "Termen limita: " + subjson[navbar_home.token_page_2]["deadline"];
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

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            navbar_home.page = "EduClass_vizualizare";
            navbar_home.use = false;
        }

        private void richTextBox1_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            ((RichTextBox)sender).Height = e.NewRectangle.Height + 5;
        }
    }
}
