using System;
using System.Windows.Forms;

namespace SIPS
{
    public partial class Progress_Clock : Form
    {
        public Progress_Clock()
        {
            InitializeComponent();
            button1.Enabled = false;
            timer1.Start();
        }

        private void Progress_Clock_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Increment(+1);
            if (progressBar1.Value == 99) button1.Enabled = true;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Start.ActiveForm.Close();
        }
    }
}
