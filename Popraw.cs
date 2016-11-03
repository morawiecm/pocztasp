using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SIPS
{
    public partial class Popraw : Form
    {
        string nazwa, uwagi, adresat, list_paczka,adr1,adr2,ro,adre, nazwa_pliku;
        int rodzaj_listu,index;
        Connect_Mysql conn;
        ListView lv1;

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) checkBox2.Checked = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked) checkBox1.Checked = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = -1;
            comboBox2.Items.Clear();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox2.SelectedItem = -1;
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

        private void button1_Click(object sender, EventArgs e)
        {
            
            int numer;
            string update = "UPDATE resort SET Typ = @typ , Rodzaj = @rodzaj , Nazwa = @nazwa , Cel_numer = @cel_numer , Uwagi = @uwagi WHERE Lp = @lp";
            MySqlCommand cmd = new MySqlCommand(update, conn.connection);
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
            cmd.Parameters.AddWithValue("@lp", index);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            zamien_numer(numer);
            updatelist();
            Start.ActiveForm.Close();
        }

        private void zamien_numer(int numerekk)
        {
            
            string sel = "SELECT Nazwa FROM adresat WHERE Numer = @numer";
            MySqlCommand cmd = new MySqlCommand(sel, conn.connection);
            cmd.Parameters.AddWithValue("@numer", numerekk);
            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                adre = reader.GetString(0);
            }//koniec while reader
            reader.Close();

        }

        private void updatelist()
        {
            ListViewItem item = new ListViewItem();
            item = this.lv1.SelectedItems[0];
            item.SubItems[1].Text = textBox1.Text;
            item.SubItems[2].Text = ro;//rodzaj
            item.SubItems[3].Text = adre;//adresat
            item.SubItems[4].Text = textBox2.Text;
            lv1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lv1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

        }//koniec update list

        public Popraw(string n , string u , string a , string l , int rl , Connect_Mysql con , int ind , ListView lv)
            
        {
            this.nazwa = n;
            this.uwagi = u;
            this.adresat = a;
            this.list_paczka = l;
            this.rodzaj_listu = rl;
            this.conn = con;
            this.index = ind;
            this.lv1 = lv;
            InitializeComponent();
            wypelnij();
            
        }

        private void wypelnij()
        {
            textBox1.Text = nazwa;
            textBox2.Text = uwagi;
            if ( rodzaj_listu == 1 )
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }
            if ( list_paczka.Equals("List"))
            {
                checkBox1.Checked  = true;
                checkBox2.Checked = false;
            }
            else
            {
                checkBox1.Checked = false;
                checkBox2.Checked = true;
            }

            wypelnij_adresata();

        }//koniec wypelnij

        private void wypelnij_adresata()
        {
            string[] dziel = adresat.Split(new string[] { " - " }, StringSplitOptions.None);
            adr1 = dziel[0];
            adr2 = dziel[1]; 

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

            comboBox1.SelectedIndex = comboBox1.Items.IndexOf(adr1);
            wyp_cb2();
            comboBox2.SelectedIndex = comboBox2.Items.IndexOf(adr2);
        }//koniec wypelnij adresata

        private void wyp_cb2()
        {
            string nazwa_pliku = null;
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

        private void Popraw_Load(object sender, EventArgs e)
        {

        }
    }
}
