using System;
using System.Windows.Forms;

namespace SIPS
{
    public partial class WyborDrukowania_PP_PS : Form
    {
        string nr_zestawienia, nr_wykazu, nadawca;
        Connect_Mysql conn;

        private void button1_Click(object sender, EventArgs e)
        {
            new Wykaz_Poprawiony_PP_Zwykle(nr_zestawienia, nr_wykazu, nadawca, conn);
            Start.ActiveForm.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Wykaz_Poprawiony_Polecone_PP_PS(nr_zestawienia, nr_wykazu, nadawca, conn);
            Start.ActiveForm.Close();
        }

        public WyborDrukowania_PP_PS(string t9, string t10, string t11, Connect_Mysql con)
        {
            this.nr_zestawienia = t9;
            this.nr_wykazu = t10;
            this.nadawca = t11;
            this.conn = con;
            InitializeComponent();
        }

        private void WyborDrukowania_PP_PS_Load(object sender, EventArgs e)
        {

        }
    }
}
