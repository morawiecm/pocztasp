using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;

namespace SIPS
{
    public partial class Dodaj : Form
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="con">polaczenie z mysql</param>
        /// <param name="lu">login uzytkownika</param>
        /// <param name="nu">imie i nazwisko</param>
        /// <param name="lv">listview z formy resort</param>
        /// <param name="l">liczba porzadkowa listy</param>
        /// <param name="ju">jednostka i wydzial</param>

        public Dodaj(Connect_Mysql con, string lu, string nu, ListView lv, int l, string ju)
        {
            this.conn = con;
            this.login_uzytkownika = lu;
            this.nazwa_uzytkownika = nu;
            this.jednostka_uzytkownika = ju;
            this.lv1 = lv;
            this.lp = l;
            InitializeComponent();
        }

        private void Dodaj_Load(object sender, EventArgs e)
        {
            uaktywnij();

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) checkBox2.Checked = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked) checkBox1.Checked = false;

        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            try
            {
                //pobieranie nazw jednostek z katalogu sources - sa to przegródki z poczty specjalnej kwp gorzow
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
                //n apodstawie wyboru z wyzej pobiera dane wydzialow/jednostek z konkretnego rejonu
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            uaktywnij();
        }
        private void uaktywnij()
        {

            if (comboBox2.SelectedIndex == -1) button1.Enabled = false;
            else button1.Enabled = true;
        }
        
        /// <summary>
        /// przycisk dodaj list aktywny kiedy jest wypelnione pole nazwa oraz wybrane sa 2 comboboxy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (String.IsNullOrEmpty(textBox1.Text))
                 MessageBox.Show("Brak numeru listu");
            else
            if (radioButton1.Checked == true)
            { //button dodaj
                int numer;
                string ro;
                insert = "INSERT INTO resort(Typ,Rodzaj,Nazwa,Uwagi,Cel_numer,Tworca,Bufor,Data) VALUES (@typ,@rodzaj,@nazwa,@uwagi,@cel_numer,@tworca,@bufor,@data)";
                MySqlCommand cmd = new MySqlCommand(insert, conn.connection);
                
                if (radioButton1.Checked) cmd.Parameters.AddWithValue("@typ", 1);//jezeli jawny to 1
                else cmd.Parameters.AddWithValue("@typ", 2);//jezeli niejawny to 2

                if (checkBox1.Checked) { cmd.Parameters.AddWithValue("@rodzaj", 1); ro = "List"; }//jezeli list to 1
                else { cmd.Parameters.AddWithValue("@rodzaj", 2); ro = "Paczka"; }//jezeli paczka to 2

                cmd.Parameters.AddWithValue("@nazwa", textBox1.Text);
                cmd.Parameters.AddWithValue("@uwagi", textBox2.Text);
                if (comboBox2.SelectedIndex == -1)
                    numer = comboBox1.SelectedIndex * 100;
                else
                numer = comboBox1.SelectedIndex * 100 + comboBox2.SelectedIndex;
                cmd.Parameters.AddWithValue("@cel_numer", numer);

                cmd.Parameters.AddWithValue("@tworca", jednostka_uzytkownika);
                cmd.Parameters.AddWithValue("@bufor", login_uzytkownika);
                cmd.Parameters.AddWithValue("@data", DateTime.Now.ToShortDateString());
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dodajDoListy(textBox1.Text, textBox2.Text, ro, numer);
                textBox1.Text = "";
                textBox2.Text = "";
                checkBox1.Checked = true;
                if (checkBox1.Checked) checkBox2.Checked = false;
   
            }
            else
            //jezeli wybrano list niejawny KONIECZNIE NALEZY WYPELNIC POLE UWAGI
                        if (radioButton2.Checked == true)
                if (String.IsNullOrEmpty(textBox2.Text))
                    MessageBox.Show("Puste Pole Uwagi");
                        else
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

                numer = comboBox1.SelectedIndex * 100 + comboBox2.SelectedIndex;
                cmd.Parameters.AddWithValue("@cel_numer", numer);

                cmd.Parameters.AddWithValue("@tworca", jednostka_uzytkownika);
                cmd.Parameters.AddWithValue("@bufor", login_uzytkownika);
                cmd.Parameters.AddWithValue("@data", DateTime.Now.ToShortDateString());
                cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    dodajDoListy(textBox1.Text, textBox2.Text, ro, numer);
                textBox1.Text = "";
                textBox2.Text = "";
                //comboBox2.SelectedIndex = -1;
                comboBox1.Items.Clear();
                comboBox2.Items.Clear();
            }

        }

        /// <summary>
        /// odswierzanie listview z resortu
        /// </summary>
        /// <param name="naz">nazwa listu z textbox1</param>
        /// <param name="uwag">pole uwagi z textbox2</param>
        /// <param name="rod">list/paczka</param>
        /// <param name="nm">numer z tabeli adresat</param>

        private void dodajDoListy(string naz , string uwag , string rod , int nm)
        {
            ListViewItem lvi = null;
            lvi = new ListViewItem(lp.ToString());
            if (radioButton2.Checked) lvi.BackColor = Color.Yellow;
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


        /// <summary>
        /// zamiana numeru na nazwe z tabeli adresat
        /// </summary>
        /// <param name="numerek"></param>
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

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = -1;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// wyszukiwarka miast z tabeli miasta - dane pobrane z pliku exel otrzymanego z poczty specjalnej kwp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button2_Click(object sender, EventArgs e)
        {
            textBox4.Clear();
            string select = "SELECT Wojewodztwo FROM miasta WHERE Nazwa = @nazwa";
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            cmd.Parameters.AddWithValue("@nazwa", (textBox3.Text).ToUpper());
            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                textBox4.Text += reader.GetString(0) + Environment.NewLine;
            }
            reader.Close();
        }
    }
}
