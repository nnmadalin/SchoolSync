using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;

namespace SchoolSync.login_signin
{
    public partial class signin : UserControl
    {
        public signin()
        {
            InitializeComponent();
        }

        private void signin_Load(object sender, EventArgs e)
        {
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
            var label = panel.Controls["label1"];
            label.Text = "SchoolSync | Inregistrare";
            GC.Collect();
            add_icon_error();
            
        }

        bool check_trim()
        {
            bool ok = true;
            if (guna2TextBox1.Text.Trim() == "")
            {
                errorProvider1.SetError(guna2TextBox1, "Completeaza caseta!");                
                guna2TextBox1.BorderColor = Color.FromArgb(203, 25, 39);
                ok = false;
            }
            if (guna2TextBox2.Text.Trim() == "")
            {
                errorProvider2.SetError(guna2TextBox2, "Completeaza caseta!");
                guna2TextBox2.BorderColor = Color.FromArgb(203, 25, 39);
                ok = false;
            }
            if (guna2TextBox3.Text.Trim() == "")
            {
                errorProvider3.SetError(guna2TextBox3, "Completeaza caseta!");
                guna2TextBox3.BorderColor = Color.FromArgb(203, 25, 39);
                ok = false;
            }
            if (guna2TextBox4.Text.Trim() == "")
            {
                errorProvider4.SetError(guna2TextBox4, "Completeaza caseta!");
                guna2TextBox4.BorderColor = Color.FromArgb(203, 25, 39);
                ok = false;
            }
            if (guna2TextBox5.Text.Trim() == "")
            {
                errorProvider5.SetError(guna2TextBox5, "Completeaza caseta!");
                guna2TextBox5.BorderColor = Color.FromArgb(203, 25, 39);
                ok = false;
            }

            return ok;
        }

        void add_icon_error()
        {
            errorProvider1.Icon = SchoolSync.Properties.Resources.icons8_high_risk_48;
            errorProvider2.Icon = SchoolSync.Properties.Resources.icons8_high_risk_48;
            errorProvider3.Icon = SchoolSync.Properties.Resources.icons8_high_risk_48;
            errorProvider4.Icon = SchoolSync.Properties.Resources.icons8_high_risk_48;
            errorProvider5.Icon = SchoolSync.Properties.Resources.icons8_high_risk_48;
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var frm = new login();

            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
            panel.Controls.Add(frm);
            frm.Show();
            panel.Controls.Remove(this);
            GC.Collect();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private async void guna2Button1_Click_1(object sender, EventArgs e)
        {
            if (check_trim())
            {
                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("token", "W!WSAnXZLOhyQ6lpt=adAhsOaF5QrI6eN4!1p/PWi7y8A9gTwKiD6DO6kmwdmcUHFeG?v99ihZYAeiLtf7NdT2MHCnzy=mvdI1MnmZLEtVOus2O0qYFo4oDfVyB7QeLBFo5SrzqueDvwtMFVBpRcLygr3Jxg-GhmOZ07IPsBpmZ8P0bhBUegmskNsTKk!x!bc2yT-LOrCwk!XU!!2I10=SLFfsf0s-OGCcmS-f=4l3X8u3lL/nsnY8vjSQ0jn13H");
                data.Add("sql", string.Format("select * from accounts where username = {0}", guna2TextBox2.Text));

                var multiple_class = new multiple_class();

                dynamic task = await multiple_class.PostRequestAsync(url, data);
                if (task["message"] == "success")
                {
                    ///afisare eroare
                }
                else if (task["message"] == "Database no value")
                {
                    ///verifica pt email .... daca totul este bine, adauga in db
                }
                else if (task["message"] == "SQL command error")
                {
                    ///afisare eroare pt admin
                }
                else if (task["message"] == "token invalid")
                {
                    ///afisare eroare pt admin
                }

            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox1.BorderColor = Color.Black;
            errorProvider1.Dispose();
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox2.BorderColor = Color.Black;
            errorProvider2.Dispose();
        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox3.BorderColor = Color.Black;
            errorProvider3.Dispose();
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox4.BorderColor = Color.Black;
            errorProvider4.Dispose();
        }

        private void guna2TextBox5_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox5.BorderColor = Color.Black;
            errorProvider5.Dispose();
        }
    }
}
