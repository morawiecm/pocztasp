using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MySql.Data.Common;
using MySql.Data.Types;
using MySql.Data.MySqlClient;

namespace SIPS
{
    public partial class DodajObcy : Form
    {
        Connect_Mysql conn;
        string login_uzytkownika;
        string nazwa_uzytkownika;
        string jednostka_uzytkownika;
        string nazwa_pliku = "";
        string insert = "";
        string adresat = "";
        int lp;
        ListView lv1;

        public DodajObcy(Connect_Mysql con, string lu, string nu, ListView lv, int l, string ju)
        {
            this.conn = con;
            this.login_uzytkownika = lu;
            this.nazwa_uzytkownika = nu;
            this.jednostka_uzytkownika = ju;
            this.lv1 = lv;
            this.lp = l;
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) checkBox2.Checked = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked) checkBox1.Checked = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = -1;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            uaktywnij();
        }
        private void uaktywnij()
        {

            if (comboBox2.SelectedIndex == -1) button1.Enabled = false;
            else button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        { //button dodaj
            int numer;
            string ro;
            insert = "INSERT INTO resort(Typ,Rodzaj,Nazwa,Uwagi,Cel_numer,Tworca,Bufor,Data) VALUES (@typ,@rodzaj,@nazwa,@uwagi,@cel_numer,@tworca,@bufor,@data)";
            MySqlCommand cmd = new MySqlCommand(insert, conn.connection);

            if (radioButton1.Checked) cmd.Parameters.AddWithValue("@typ", 1);
            else cmd.Parameters.AddWithValue("@typ", 2);

            if (checkBox1.Checked) { cmd.Parameters.AddWithValue("@rodzaj", 1); ro = "List"; }
            else { cmd.Parameters.AddWithValue("@rodzaj", 2); ro = "Paczka"; }

            cmd.Parameters.AddWithValue("@nazwa", textBox1.Text);
            cmd.Parameters.AddWithValue("@uwagi", textBox2.Text);
            if (comboBox2.SelectedIndex == -1)
                numer = comboBox1.SelectedIndex * 100;
            else
            numer = comboBox1.SelectedIndex * 100 + comboBox2.SelectedIndex;
            cmd.Parameters.AddWithValue("@cel_numer", numer);

            cmd.Parameters.AddWithValue("@tworca", textBox3.Text);
            cmd.Parameters.AddWithValue("@bufor", login_uzytkownika);
            cmd.Parameters.AddWithValue("@data", DateTime.Now.ToShortDateString());
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            dodajDoListy(textBox1.Text, textBox2.Text, ro, numer);
            textBox1.Text = "";
            textBox2.Text = "";
          //  comboBox2.SelectedIndex = -1;
        }
        private void dodajDoListy(string naz, string uwag, string rod, int nm)
        {
            ListViewItem lvi = null;
            lvi = new ListViewItem(lp.ToString());
            lvi.SubItems.Add(naz);
            lvi.SubItems.Add(rod);
            zamien_numer(nm);
            lvi.SubItems.Add(adresat);
            lvi.SubItems.Add(uwag);
            lvi.SubItems.Add("-");
            lv1.Items.Add(lvi);
            lv1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lv1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            lp++;
        }//koniec dodaj do listy
        private void zamien_numer(int numerek)
        {
            string sel = "SELECT Nazwa FROM adresat WHERE Numer = @numer";
            MySqlCommand cmd = new MySqlCommand(sel, conn.connection);
            cmd.Parameters.AddWithValue("@numer", numerek);
            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                adresat = reader.GetString(0);
            }//koniec while reader
            reader.Close();
            //cmd.Dispose();

        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox1.Items.Clear();
            try
            {
                StreamReader sr = new StreamReader(@"C:\Program\Sources\jednostka.txt", Encoding.GetEncoding("ISO-8859-2"));
                string line = sr.ReadLine();

                while (line != null)
                {

                    comboBox1.Items.Add(line);
                    line = sr.ReadLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd pliku" + ex.Message);
            }
        }

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox2.Items.Clear();
            if (comboBox1.SelectedIndex == 0) nazwa_pliku = "kwpgorzow.txt";
            if (comboBox1.SelectedIndex == 1) nazwa_pliku = "kwpgorzowinstytucje.txt";
            if (comboBox1.SelectedIndex == 2) nazwa_pliku = "kmpgorzow.txt";
            if (comboBox1.SelectedIndex == 3) nazwa_pliku = "kmpzielonagora.txt";
            if (comboBox1.SelectedIndex == 4) nazwa_pliku = "kmpzielonagorainstytucje.txt";
            if (comboBox1.SelectedIndex == 5) nazwa_pliku = "krosnoodrzanskie.txt";
            if (comboBox1.SelectedIndex == 6) nazwa_pliku = "krosnoinstytucje.txt";
            if (comboBox1.SelectedIndex == 7) nazwa_pliku = "nosgkrosno.txt";
            if (comboBox1.SelectedIndex == 8) nazwa_pliku = "kppmiedzyrzecz.txt";
            if (comboBox1.SelectedIndex == 9) nazwa_pliku = "kppnowasol.txt";
            if (comboBox1.SelectedIndex == 10) nazwa_pliku = "kpskwierzyna.txt";
            if (comboBox1.SelectedIndex == 11) nazwa_pliku = "skwierzynajw.txt";
            if (comboBox1.SelectedIndex == 12) nazwa_pliku = "kppslubice.txt";
            if (comboBox1.SelectedIndex == 13) nazwa_pliku = "kppstrzelce.txt";
            if (comboBox1.SelectedIndex == 14) nazwa_pliku = "kpsulechow.txt";
            if (comboBox1.SelectedIndex == 15) nazwa_pliku = "kppsulecin.txt";
            if (comboBox1.SelectedIndex == 16) nazwa_pliku = "kppswiebodzin.txt";
            if (comboBox1.SelectedIndex == 17) nazwa_pliku = "kppwschowa.txt";
            if (comboBox1.SelectedIndex == 18) nazwa_pliku = "kppzagan.txt";
            if (comboBox1.SelectedIndex == 19) nazwa_pliku = "245zagan.txt";
            if (comboBox1.SelectedIndex == 20) nazwa_pliku = "kppzary.txt";
            if (comboBox1.SelectedIndex == 21) nazwa_pliku = "kraj.txt";

            try
            {
                StreamReader sr = new StreamReader(@"C:\Program\Sources\" + nazwa_pliku, Encoding.GetEncoding("ISO-8859-2"));
                string line = sr.ReadLine();
                while (line != null)
                {
                    comboBox2.Items.Add(line);
                    line = sr.ReadLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        private void DodajObcy_Load(object sender, EventArgs e)
        {

        }
    }
}
