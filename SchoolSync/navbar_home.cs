using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace SchoolSync
{
    public partial class navbar_home : UserControl
    {
        public navbar_home()
        {
            InitializeComponent();
        }

        private void home_Load(object sender, EventArgs e)
        {
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
            var label = panel.Controls["label1"];
            label.Text = "SchoolSync | Acasa";
            GC.Collect();

            use = true;
            guna2Panel2.Controls.Clear();

            var frm = new pages.Home();
            guna2Panel2.Controls.Add(frm);

            background_color_btn();
            guna2Button1.FillColor = Color.FromArgb(66, 66, 66);
        }

        void background_color_btn()
        {
            guna2Button1.FillColor = Color.Transparent;
            guna2Button2.FillColor = Color.Transparent;
            guna2Button3.FillColor = Color.Transparent;
            guna2Button5.FillColor = Color.Transparent;
            guna2Button6.FillColor = Color.Transparent;
        }        

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            
            navbar_home.use = false;
            navbar_home.page = "Home";
        }       

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            navbar_home.use = false;
            navbar_home.page = "EduMentor";

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            navbar_home.use = false;
            navbar_home.page = "InvataUnit";
        }


        private void guna2Button5_Click(object sender, EventArgs e)
        {
            navbar_home.token_page = login_signin.login.accounts_user["token"];
            navbar_home.use = false;
            navbar_home.page = "Profil";
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
            Properties.Settings.Default.Data_account = "";
            Properties.Settings.Default.Save();

            var frm = new login_signin.login ();
            panel.Controls.Clear();
            panel.Controls.Add(frm);
        }

        public static bool use = true;
        public static string page = "", token_page = "";

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            navbar_home.use = false;
            navbar_home.page = "FlowTalk";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (page == "Home" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | Acasa";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.Home();
                guna2Panel2.Controls.Add(frm);

                background_color_btn();
                guna2Button1.FillColor = Color.FromArgb(66, 66, 66);
            }

            else if (page == "EduMentor" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduMentor";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.EduMentor();
                guna2Panel2.Controls.Add(frm);

                background_color_btn();
                guna2Button2.FillColor = Color.FromArgb(66, 66, 66);
            }
            else if (page == "EduMentor_adauga" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduMentor - Adauga Material";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.EduMentor_pages.EduMentor_Adauga();
                guna2Panel2.Controls.Add(frm);

                background_color_btn();
                guna2Button2.FillColor = Color.FromArgb(66, 66, 66);
            }
            else if (page == "EduMentor_cod" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduMentor - Vizualizare cod";
                GC.Collect();

                use = true;

                var frm = new pages.EduMentor_pages.EduMentor_Cod();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "EduMentor_vizualizare" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduMentor - Vizualizare Material";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.EduMentor_pages.EduMentor_Vizualizare();
                guna2Panel2.Controls.Add(frm);
            }
            else if (page == "EduMentor_vizualizare_->_home" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduMentor - Vizualizare Material";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();
                var frm = new pages.EduMentor_pages.EduMentor_Vizualizare();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "EduMentor_editare" && use == false)
            {

                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduMentor - Editare Material";
                GC.Collect();

                use = true;

                guna2Panel2.Controls.Clear();

                var frm = new pages.EduMentor_pages.EduMentor_Adauga();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "EduMentor_editare_->_home" && use == false)
            {

                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduMentor - Editare Material";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();
                var frm = new pages.EduMentor_pages.EduMentor_Adauga();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }

            else if (page == "InvataUnit" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | InvataUnit";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.InvataUnit();
                guna2Panel2.Controls.Add(frm);

                background_color_btn();
                guna2Button3.FillColor = Color.FromArgb(66, 66, 66);
            }
            else if (page == "InvataUnit_adauga" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | InvataUnit - Adauga Intrebare";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.InvataUnit_pages.InvataUnit_Adauga();
                guna2Panel2.Controls.Add(frm);
            }
            else if (page == "InvataUnit_editare" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | InvataUnit - Editare Intrebare";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();
                var frm = new pages.InvataUnit_pages.InvataUnit_Adauga();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "InvataUnit_vizualizare" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | InvataUnit - Vizualizare intrebare";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.InvataUnit_pages.InvataUnit_Vizualizare();
                guna2Panel2.Controls.Add(frm);

            }
            
            else if(page == "FlowTalk" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | FlowTalk";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.FlowTalk_pages.FlowTalk();
                guna2Panel2.Controls.Add(frm);
            }
            else if (page == "FlowTalk_adauga" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | FlowTalk";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.FlowTalk_pages.FlowTalk_Adauga();
                guna2Panel2.Controls.Add(frm);
            }
            else if (page == "FlowTalk_editare" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | FlowTalk";
                GC.Collect();

                use = true;

                var frm = new pages.FlowTalk_pages.FlowTalk_Adauga();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }

            else if (page == "Profil" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | Profil";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.Profil();
                guna2Panel2.Controls.Add(frm);
                background_color_btn();
                guna2Button5.FillColor = Color.FromArgb(66, 66, 66);
            }
            else if (page == "Profil_person" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | Profil";
                GC.Collect();

                use = true;

                var frm = new pages.Profil();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }

            
        }
    }
}
