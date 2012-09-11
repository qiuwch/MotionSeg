using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zap
{
    public partial class CustomPlayer : Form
    {
        public CustomPlayer()
        {
            InitializeComponent();
            // this.movieScreen1.OpenFile("mv2_001.avi");
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            this.movieScreen1.Play();
        }

        private void btn_pause_Click(object sender, EventArgs e)
        {
            this.movieScreen1.Pause();
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            this.movieScreen1.Stop();
        }


    }
}
