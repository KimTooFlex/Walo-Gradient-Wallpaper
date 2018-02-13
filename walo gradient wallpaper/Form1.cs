using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace walo_gradient_wallpaper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        Image wp = null;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            wp = core.GenerateImage(bunifuColorTransition1,coltop.BackColor , colBottom.BackColor,colleft.BackColor, colRight.BackColor);
            core.SetWallPaper(wp);
 
 
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            panelImg.BackgroundImage= wp;
            timer1.Start();
        }

        int i = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            if(i==0)
            {
                coltop.BackColor = core.getRandomColor();
                i++;
            }
            else if (i == 1)
            {
                colBottom.BackColor = core.getRandomColor();
                i++;
            }
            else if (i == 2)
            {
                colleft.BackColor = core.getRandomColor();
                i++;
            }
            else if (i == 3)
            {
                colRight.BackColor = core.getRandomColor();
                i = 0;
            }




       
          
         
         

            try
            {
                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception)
            {
            }
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Visible = !this.Visible;
        }
    }
}
