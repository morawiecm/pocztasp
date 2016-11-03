using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SIPS
{
    public partial class Popraw_zestawienie : Form
    {
        string nazwa, uwagi, adresat, list_paczka, adr1, adr2, ro, adre, nazwa_pliku;
        int rodzaj_listu, index;
        Connect_Mysql conn;
        ListView lv1;
        int cb1 = 0;
        string rodzaj;

        public Popraw_zestawienie(Connect_Mysql con, int ind, ListView lv)
        {
            this.conn = con;
            this.index = ind;
            this.lv1 = lv;
            InitializeComponent();
            wyp_pola();
        //    wypelnij();
        }

        private void wyp_pola()
        {
            string select = "SELECT Typ, Rodzaj, Nazwa, Cel_numer, Uwagi , Tworca FROM resort WHERE Lp = @lp";
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            cmd.Parameters.AddWithValue("@lp", index);
            int wyn,cb2;
            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if ( reader.GetInt32(0) == 1 )
                {
                    radioButton1.Checked = true;
                    if (radioButton1.Checked) radioButton2.Checked = false;
                }
                else
                {
                    radioButton2.Checked = true;
                    if (radioButton2.Checked) radioButton1.Checked = false;
                }

                if (reader.GetInt32(1) == 1)
                {
                    checkBox1.Checked = true;
                    if (checkBox1.Checked) checkBox2.Checked = false;
                }
                else
                {
                    checkBox2.Checked = true;
                    if (checkBox2.Checked) checkBox1.Checked = false;
                }
                textBox1.Text = reader.GetString(2);
                wyn = reader.GetInt32(3);
                cb2 = wyn % 100;
                cb1 = (wyn - cb2) / 100;
                w_cb1();
                comboBox1.SelectedIndex = cb1;
                w_cb2();
                if ( cb2 > 0)
                comboBox2.SelectedIndex = cb2; 
              //  else
                //    comboBox2.SelectedIndex = 0;
                textBox2.Text = reader.GetString(4);
                textBox3.Text = reader.GetString(5);
            }
            reader.Close();
        }
        private void w_cb2()
        {
            string nazwa_pliku = null;
            comboBox2.Items.Clear();
            if (cb1 == 0) nazwa_pliku = "kwpgorzow.txt";
            if (cb1 == 1) nazwa_pliku = "kwpgorzowinstytucje.txt";
            if (cb1 == 2) nazwa_pliku = "kmpgorzow.txt";
            if (cb1 == 3) nazwa_pliku = "kmpzielonagora.txt";
            if (cb1 == 4) nazwa_pliku = "kmpzielonagorainstytucje.txt";
            if (cb1 == 5) nazwa_pliku = "krosnoodrzanskie.txt";
            if (cb1 == 6) nazwa_pliku = "krosnoinstytucje.txt";
            if (cb1 == 7) nazwa_pliku = "nosgkrosno.txt";
            if (cb1 == 8) nazwa_pliku = "kppmiedzyrzecz.txt";
            if (cb1 == 9) nazwa_pliku = "kppnowasol.txt";
            if (cb1 == 10) nazwa_pliku = "kpskwierzyna.txt";
            if (cb1 == 11) nazwa_pliku = "skwierzynajw.txt";
            if (cb1 == 12) nazwa_pliku = "kppslubice.txt";
            if (cb1 == 13) nazwa_pliku = "kppstrzelce.txt";
            if (cb1 == 14) nazwa_pliku = "kpsulechow.txt";
            if (cb1 == 15) nazwa_pliku = "kppsulecin.txt";
            if (cb1 == 16) nazwa_pliku = "kppswiebodzin.txt";
            if (cb1 == 17) nazwa_pliku = "kppwschowa.txt";
            if (cb1 == 18) nazwa_pliku = "kppzagan.txt";
            if (cb1 == 19) nazwa_pliku = "245zagan.txt";
            if (cb1 == 20) nazwa_pliku = "kppzary.txt";
            if (cb1 == 21) nazwa_pliku = "kraj.txt";

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

        private void w_cb1()
        {
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

        private void wypelnij()
        {
            textBox1.Text = nazwa;
            textBox2.Text = uwagi;
            if (rodzaj_listu == 1)
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }
            if (list_paczka.Equals("List"))
            {
                checkBox1.Checked = true;
                checkBox2.Checked = false;
            }
            else
            {
                checkBox1.Checked = false;
                checkBox2.Checked = true;
            }

            wypelnij_adresata();

            if (rodzaj_listu == 1) rodzaj = "Jawny"; else rodzaj = "Niejawny";

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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) radioButton2.Checked = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked) radioButton1.Checked = false;
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
            comboBox2.Items.Clear();
        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

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
            string update = "UPDATE resort SET Typ = @typ , Rodzaj = @rodzaj , Nazwa = @nazwa , Cel_numer = @cel_numer , Uwagi = @uwagi , Tworca = @tworca WHERE Lp = @lp";
            MySqlCommand cmd = new MySqlCommand(update, conn.connection);
            if (radioButton1.Checked) cmd.Parameters.AddWithValue("@typ", 1);
            else cmd.Parameters.AddWithValue("@typ", 2);

            if (checkBox1.Checked) { cmd.Parameters.AddWithValue("@rodzaj", 1); ro = "List"; }
            else { cmd.Parameters.AddWithValue("@rodzaj", 2); ro = "Paczka"; }
            cmd.Parameters.AddWithValue("@nazwa", textBox1.Text);
            cmd.Parameters.AddWithValue("@uwagi", textBox2.Text);
            cmd.Parameters.AddWithValue("@tworca", textBox3.Text);
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

        private void updatelist()
        {
            ListViewItem item = new ListViewItem();
            item = this.lv1.SelectedItems[0];
            item.SubItems[1].Text = textBox1.Text;
            item.SubItems[2].Text = adre;//adresat
            item.SubItems[3].Text = ro;//rodzaj
            item.SubItems[4].Text = rodzaj;
            item.SubItems[5].Text = textBox2.Text;
            lv1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lv1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

        }//koniec update list

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

        private void Popraw_zestawienie_Load(object sender, EventArgs e)
        {

        }
    }
}
