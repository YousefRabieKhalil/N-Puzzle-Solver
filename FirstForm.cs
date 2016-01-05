//Viedo Form to Display The introduction
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Copy_the_N_puzzle
{
    public partial class FirstForm : MetroForm
    {
        public FirstForm()
        {
            InitializeComponent();
           
        }

        private void FirstForm_Load(object sender, EventArgs e)
        {
           
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 8)
            {
                Form1 N = new Form1(this);
                this.Visible = false;
                N.Show();

            }
        }
    }
}
