using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolSync.pages
{
    public partial class home : UserControl
    {
        public home()
        {
            InitializeComponent();
        }

        private async void home_Load(object sender, EventArgs e)
        {
            multiple_class _Class = new multiple_class();

            label2.Text = login_signin.login.accounts_user["username"] + "!";
            guna2CirclePictureBox1.Image =  await _Class.IncarcaImagineAsync("https://schoolsync.nnmadalin.me/api/getfile.php?token=userfoto_" + login_signin.login.accounts_user["token"] + ".png");

        }
    }
}
