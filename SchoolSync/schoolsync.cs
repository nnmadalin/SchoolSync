using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace SchoolSync
{
    public partial class schoolsync : Form
    {
        public schoolsync()
        {
            InitializeComponent();
        }


        public static string token = "DJc?hdPwPYi?QqKCD=M0KCh7tW2K?mkrOvfBGwAY=JKnWmHY9?5nS2rhUiN9vZNjWSokzOnb9wbbd!l60NeDD7nASmhIQQQQV33p2iw!WJ2hWLci7jnDP8cnVLAb6JVHaXFUvBYu/IY0F15f5VmyD4BIAsvpzg2zB2cybXRKNbex9c8rlNLyGjTlobwrRxg9I!nFoMCSVFqF6LoN8Dh=SXfY4T=RWeA/0SyBSC4euqFa?WWBt!2KO?LU7-Z!CW3K/lmq=-QY9WcIUTjegVkdn2FRXtiy4crvBXqjK8?5zPPuC6dtV3TTAI7zl9PRd!6j7jQTg9QbGMVaIKVFSsnGlztDhbDKZrtUlhes9rCXyeclHxW76GrZTuRFVP7PeBM4yoU47or=Mg04Ti8oKt6V4ujj0NE50lEPVaO4c3TZP8aeIklWHKht!2haBrO-60PwIrKurTOD?FGUYqzbLOg4nC240mgrzELFrNZ1XZOqfTJ0DinwmChOaBYzJycwMZcEjf938avdBQYEQ=VhTAlU8j18rFC731HBvaBVtwqtBcFywJ8O=IYdgC16QC4z14C823qDZ3KzTp54xgOzhdNf6S?Gohiodni5-syE?OIX91uvABuGxqje1y66gETKeDzCsrRVocTVx4desrWqj1brK3LuO-oWYiHCJ?0Zq2I=?X0lnsyVk?dSulz65f/vD/HsHHK97chTHmgw7EtRSaNJg!srN/fqnVnREjnP9=hm56Hjl3RDr8SiEqHUv?QIcMWK/rTwYQvdwxB0lF2a!JXhUw/OA0iQ?vDkwsdPuAPnbZjCwusu!9yWi1Mi7tUFPKiqnmDg=E4jOOVbiqXS92hp!4Sa8X/Se43SYYuM7qbwRCXtY4DlYc-9?dN12s01xXuy1O7SM-cEhdi1JhuPHFdnzrxCWs1L9Yf2=xjTSOSNWwOaIBVPvMZIlreklVkRlGt6Ml4VB0p?XkTgT?kYJm95BWijiNwPR?hF/XKCWoEW5SjiFK7q!G1e-IK-gX!oPYcqYWJM6I5UPm5ym!JXgwOk-retEFZ2RXaf46d1BQsyi8-6QhRhtVsXCpjCzLpVkBUy!/4Xv0bd/fI35T7am?pwG/XnAabKVBmJUl1WXBGVY9SHmr4SguRfaJv2nDv8zyIHOaMrnTBnXl4ayDPJjbMhaRFgP5CEpc/PSQUoDkgxYkg5G3S2CA52tPyUrR-m89!?zcvxdZ-7aH3Am?fJMcqOPHiGrT0dr6sZsG8ppoBEVrR/d9ZfthJeyssK3WDgmpajbtGtj-Gaj2-TYLwEFokv70/TvOZ654=o?-zNNDVEbTsJPLIoOUcsYD0Kic?pWbr4LZQ=BW21BcHgW0y6R7XEHb8AUHIGWdM5zmh4JN9iVd43sjXjS/-Oe!tg2fyyXbg8i4uUVX/G9DoPidm1-1!Z1mgZzW69Y00=IP!tYQk5KRkYKq5LNXQrR34EcAz9hfulQVPcNbANsdfi=f/7IApDmc5W?yWCdmw1f15wnXx=VG4R7/b0D0M1zfXovcCKR9NfXLYpv1QGJi3h/7XhC7gnZ!9!/YfnorHjB2Ezco=KibxYWWU6hXk!RXkEQAP02/AFTslA?dR4w6?7yS1jFssj9xS5fMUmP?8BmL/GHilYCmbkoiMoqTfDlOLEOTssfPs8PoLVBzdDRW2ejql4w8jStwaHMbwl7UYK-JZqcAg9Ufw/6cTrqK9PxS=fgiAVG?Se3YBD7j!ic3kR1z5eI-hBzfRm5CG26RGjjl?r-R/!D3AnQeGNK!hGcSoOQ7QMJ2Z1pLBn?a87Bo7fG=0DbG1hdf7MVW=Kd50EHw?yM=KTV4dNv4dMQjRR9P-ricgiO0vAcBqRdVB-VcnLoOn?kMcE9IJN42DlBpH6foL0W=pOIo7I=R4QArMHpB0ysYwSJX=Lsza7sgC/nG3b7fC3/W2dOQPmLdRPYT83fP7IPVCr9tkPuBg04XWbX?muigVGmkzD-pE=2HVi7q/FaWQfTSRiWPoFQ65Vv!O9yFFYSg?h8tH3-vcIf6Ras!qKJzfo3y2Z";
        public static bool is_loading = false;
        public static string version = "v8.11-21/7/2023";

        private async void Form1_Load(object sender, EventArgs e)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            var frm = new login_signin.login();
            guna2Panel2.Controls.Add(frm);
            frm.Show();
            frm.Location = new Point(0, 0);
            GC.Collect();
                
        }

        public static void show_loading()
        {
            if (is_loading == false)
            {
                is_loading = true;
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var pnl = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                pnl.Enabled = false;

                Guna.UI2.WinForms.Guna2WinProgressIndicator loading = new Guna.UI2.WinForms.Guna2WinProgressIndicator()
                {
                    Size = new Size(90, 90),
                    BackColor = Color.Transparent,
                    UseTransparentBackground = true,
                    AutoStart = true,
                    Location = new Point((1340 - 90) / 2, (690 - 90) / 2),
                    Name = "panel_loading"
                };
                pnl.Controls.Add(loading);
                loading.Show();
                loading.BringToFront();
            }
        }

        public static void hide_loading()
        {
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var pnl = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
            pnl.Controls.Remove(pnl.Controls["panel_loading"]);
            pnl.Enabled = true;
            is_loading = false;
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void guna2CircleButton3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void schoolsync_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
