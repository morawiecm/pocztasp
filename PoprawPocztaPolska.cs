using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SIPS
{
    public partial class PoprawPocztaPolska : Form
    {
        Connect_Mysql conn;
        ListView lvi;
        int numer;

        public PoprawPocztaPolska(Connect_Mysql con , ListView lv1 , string nu)
        {
            this.conn = con;
            this.numer = Int32.Parse(nu);
            this.lvi = lv1;
            InitializeComponent();
            
        }

        private void PoprawPocztaPolska_Load(object sender, EventArgs e)
        {
            wypelnij();
        }

        private void wypelnij()
        {
            string select = "SELECT Rodzaj,Numer_Sprawy,Adresat,Adresat_Adres,Waga FROM poczta_polska WHERE Lp = @lp";
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            cmd.Parameters.AddWithValue("@lp", numer);
            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                label6.Text = reader.GetString(0);
                label7.Text = reader.GetString(4);
                textBox1.Text = reader.GetString(1);
                textBox2.Text = reader.GetString(2);
                textBox3.Text = reader.GetString(3);
            }//koniec while reader
            reader.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string update = "UPDATE poczta_polska SET Numer_sprawy = @numer_sprawy , Adresat = @adresat , Adresat_adres = @adresat_adres WHERE Lp = @lp";
            MySqlCommand cmd = new MySqlCommand(update, conn.connection);
            cmd.Parameters.AddWithValue("@numer_sprawy", textBox1.Text);
            cmd.Parameters.AddWithValue("@adresat", textBox2.Text);
            cmd.Parameters.AddWithValue("@adresat_adres", textBox3.Text);
            cmd.Parameters.AddWithValue("@lp", numer);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            updatelist();
            Start.ActiveForm.Close();
        }

        private void updatelist()
        {
            ListViewItem item = new ListViewItem();
            item = this.lvi.SelectedItems[0];
            item.SubItems[1].Text = textBox1.Text;
            item.SubItems[3].Text = textBox2.Text;
            item.SubItems[4].Text = textBox3.Text;
            lvi.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lvi.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
    }
}
