using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIPS
{
    public partial class Sztab_Start : Form
    {
        public Sztab_Start()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
           // connect.CloseConnection();

            Application.Exit();
        }

        private void Sztab_Start_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
        }
    }
}
