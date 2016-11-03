using System;

using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace SIPS
{
    public partial class Serwis : Form
    {
        Connect_Mysql conn;
        string Jednostka , poczatek , koniec;
        ListViewItem item;
        int OrganW,indeks;

        private void button2_Click(object sender, EventArgs e)
        {

            string wejscie, wyjscie=null,we1,we2=null;
            wejscie = textBox1.Text;
            string[] dziel = wejscie.Split(new string[] { "." }, StringSplitOptions.None);
            we1 = dziel[0];
            we2 = dziel[1];
            int b = 0,i;

            textBox2.Text = wyjscie;
        }

        public Serwis(Connect_Mysql con)
        {
            this.conn = con;
            InitializeComponent();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            ListViewItem lvi = null;
            string select = "SELECT Nazwisko, Jednostka, Lp FROM users WHERE Nazwisko like @nazwisko";
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            cmd.Parameters.Add("@nazwisko", MySqlDbType.VarChar, 200).Value = "%" + textBox1.Text + "%";
            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lvi = new ListViewItem(reader.GetString(0));
                lvi.SubItems.Add(reader.GetString(1));
                lvi.SubItems.Add(reader.GetString(2));
                listView1.Items.Add(lvi);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            reader.Close();
            cmd.Dispose();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem itemek in listView1.SelectedItems) 
            {
                item = listView1.SelectedItems[0];
                indeks = Int32.Parse(item.SubItems[2].Text);
                string sel = "SELECT Login, Haslo, typ_usera, Nazwisko, Jednostka FROM users WHERE Lp = @lp";
                MySqlCommand cmd = new MySqlCommand(sel, conn.connection);
                cmd.Parameters.AddWithValue("@lp", indeks);
                MySqlDataReader reader = null;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                  /*  if (reader.GetInt32(0) == 1) { checkBox5.Checked = true; if (checkBox5.Checked == true) checkBox6.Checked = false; }
                    else { checkBox6.Checked = true; if (checkBox6.Checked == true) checkBox5.Checked = false; }

                    textBox7.Text = reader.GetString(1);
                    zamien_numerek(reader.GetInt32(2));
                    comboBox5.SelectedIndex = comboBox5.Items.IndexOf(adr1);
                    wyp_cb6();
                    comboBox6.SelectedIndex = comboBox6.Items.IndexOf(adr2);
                    textBox13.Text = reader.GetString(3);*/

                }//koniec while
                reader.Close();
            }
        }

        private void Serwis_Load(object sender, EventArgs e)
        {
           // comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // RodzajListu = comboBox1.Text;
            OrganW = comboBox2.SelectedIndex;
            OrganW = OrganW - 1;
            Jednostka = comboBox3.Text;
 
            DateTime result = dateTimePicker1.Value;
            DateTime result2 = dateTimePicker2.Value;

            poczatek = result.ToShortDateString();
            koniec = result2.ToShortDateString();

            new StatystykiPDF(conn , OrganW , Jednostka , poczatek , koniec);
        }
    }
}
