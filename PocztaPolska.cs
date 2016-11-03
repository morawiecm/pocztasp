using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SIPS
{
    public partial class PocztaPolska : Form
    {
        Form lastone;
        int kraj_zagranica; // if =1 - kraj , if = 2 - zagraniczny
        string login_uzytkownika, nazwa_uzytkownika, jednostka_uzytkownika;
        Connect_Mysql conn;
        MySqlDataReader reader = null;
        int ow, cw, ra;//dane statystyczne listy poczta polska
        int gabaryt,lp;
        string waga_listu;
        int[] tabelka = new int[300]; //bufor indexow

        /// <summary>
        /// modul do obslugi poczty polskiej
        /// </summary>
        /// <param name="form1">forma menu glowne</param>
        /// <param name="lu">login uzytkownika</param>
        /// <param name="con">polaczenie mysql</param>
        /// <param name="nu">imie i nazwisko</param>
        /// <param name="ju">jednostka i wydzial</param>

        public PocztaPolska(Form form1, string lu, Connect_Mysql con , string nu, string ju)
        {
            this.lastone = form1;
            this.login_uzytkownika = lu;
            this.nazwa_uzytkownika = nu;
            this.jednostka_uzytkownika = ju;
            this.conn = con;
            InitializeComponent();
        }
        //od 39 do 49 nieaktywny przycis X
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        //zabezpieczenie admina ze przy kazdym wywolaniu tabelka jest zerowa
        private void zeruj_tabelke()
        {
            for (int i = 0; i < tabelka.Length; i++)
                tabelka[i] = 0;
        }

        /// <summary>
        /// wartosci domyslne formy czyli list polski, gabaryt A, waga do 350g
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void PocztaPolska_Load(object sender, EventArgs e)
        {
            kraj_zagranica = 1;
            gabaryt = 1;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            label7.Visible = false;
            comboBox6.Visible = false;
            comboBox8.Visible = false;
   
            comboBox7.SelectedIndex = 0;
            //wypelnij_liste();
            sprawdz_button();
        }

        private void wypelnij_liste()
        {
            ListViewItem list = null;
            lp = 1;
            string select = "SELECT Numer_Sprawy,Rodzaj,Adresat,Adresat_Adres,Lp FROM poczta_polska WHERE Tworca = @tworca";
            MySqlCommand sel = new MySqlCommand(select, conn.connection);
            sel.Parameters.AddWithValue("@tworca", login_uzytkownika);
            reader = sel.ExecuteReader();
            while (reader.Read())
            {
                list = new ListViewItem(lp.ToString());
                list.SubItems.Add(reader.GetString(0));
                list.SubItems.Add(reader.GetString(1));
                list.SubItems.Add(reader.GetString(2));
                list.SubItems.Add(reader.GetString(3));
                list.SubItems.Add(reader.GetInt32(4).ToString());
                listView1.Items.Add(list);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                lp++;
            }
            reader.Close();

        }


        private void button1_Click(object sender, EventArgs e)//przycisk zamknij forme
        {
            Start.ActiveForm.Hide();
            lastone.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            sprawdz_button();
            uzupelnij();
            if (kraj_zagranica == 1)
            {
                if (comboBox1.SelectedIndex <= 5)
                    comboBox5.SelectedIndex = comboBox5.FindStringExact("do 350g");
                else comboBox5.SelectedIndex = comboBox5.FindStringExact("do 1kg");
            }
            else
            {

            }

            comboBox5.SelectedIndex = 0;
        }


        private void sprawdz_button() // czy moze byc aktywny button dodaj adresata
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                if (comboBox1.SelectedIndex != -1)
                    if (comboBox2.SelectedIndex != -1)
                        if (comboBox3.SelectedIndex != -1)
                            if (comboBox4.SelectedIndex != -1)
                                button2.Enabled = true;
        }
        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            {
                sprawdz_button();
                button3.Enabled = false;
                button4.Enabled = false;
                comboBox1.Items.Clear();
                if ( kraj_zagranica == 1)
                try
                {
                    StreamReader sr = new StreamReader(@"C:\Program\Sources\pocztapolska.txt", Encoding.GetEncoding("ISO-8859-2"));
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
                else
                    try
                    {
                        StreamReader sr = new StreamReader(@"C:\Program\Sources\rodzajlistuzagranica.txt", Encoding.GetEncoding("ISO-8859-2"));
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

                sprawdz_button();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) checkBox2.Checked = false;
            gabaryt = 1;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked) checkBox1.Checked = false;
            gabaryt = 2;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked) checkBox4.Checked = false;
            kraj_zagranica = 1;
            label7.Visible = false;
            comboBox6.Visible = false;
            comboBox5.SelectedIndex = -1;
            checkBox2.Checked = false;
            checkBox1.Checked = true;
            checkBox2.Visible = true;
            comboBox1.SelectedIndex = -1;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            comboBox5.SelectedIndex = -1;
            if (checkBox4.Checked) checkBox3.Checked = false;
            kraj_zagranica = 2;
            label7.Visible = true;
            comboBox1.SelectedIndex = -1;
            comboBox6.Visible = true;
            uzupelnij();
            comboBox5.SelectedIndex = 0;
            checkBox2.Checked = false;
            checkBox1.Checked = true;
            checkBox2.Visible = false;

            comboBox6.Items.Clear();
            try
            {
                StreamReader sr = new StreamReader(@"C:\Program\Sources\strefa.txt", Encoding.GetEncoding("ISO-8859-2"));
                string line = sr.ReadLine();

                while (line != null)
                {
                    comboBox6.Items.Add(line);
                    line = sr.ReadLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd pliku" + ex.Message);
            }
            comboBox6.SelectedIndex = 0;

        }

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            sprawdz_button();
            button3.Enabled = false;
            button4.Enabled = false;
            comboBox2.Items.Clear();
            try
            {
                StreamReader sr = new StreamReader(@"C:\Program\Sources\celw.txt", Encoding.GetEncoding("ISO-8859-2"));
                string line = sr.ReadLine();

                while (line != null)
                {
                    comboBox2.Items.Add(line);
                    line = sr.ReadLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd pliku" + ex.Message);
            }
            sprawdz_button();
        }

        private void comboBox3_MouseClick(object sender, MouseEventArgs e)
        {
            sprawdz_button();
            button3.Enabled = false;
            button4.Enabled = false;
            comboBox3.Items.Clear();
            try
            {
                StreamReader sr = new StreamReader(@"C:\Program\Sources\celw2.txt", Encoding.GetEncoding("ISO-8859-2"));
                string line = sr.ReadLine();

                while (line != null)
                {
                    comboBox3.Items.Add(line);
                    line = sr.ReadLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd pliku" + ex.Message);
            }
            sprawdz_button();
        }

        private void comboBox4_MouseClick(object sender, MouseEventArgs e)
        {
            sprawdz_button();
            button3.Enabled = false;
            button4.Enabled = false;
            comboBox4.Items.Clear();
            try
            {
                StreamReader sr = new StreamReader(@"C:\Program\Sources\rodzaja.txt", Encoding.GetEncoding("ISO-8859-2"));
                string line = sr.ReadLine();

                while (line != null)
                {
                    comboBox4.Items.Add(line);
                    line = sr.ReadLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd pliku" + ex.Message);
            }
            sprawdz_button();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            sprawdz_button();
            if (String.IsNullOrEmpty(textBox1.Text))
                button2.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ow = comboBox2.SelectedIndex;
            cw = comboBox3.SelectedIndex;
            ra = comboBox4.SelectedIndex;
            waga_listu = comboBox5.Text;

            if (kraj_zagranica == 1) dodajlistkrajowy();
            if (kraj_zagranica == 2) dodajlistzagraniczny();


        }

        /// <summary>
        /// wywolywana jest jedna klasa DodajPP ale ze wzgledu na rodzaj listu kraj/zagranica wazna jesy suma wszystkich comboboxow + checkboxow + radiobuttonow
        /// kraj to wynik >=1000 a zagraniczny to >=2000
        /// </summary>

        private void dodajlistzagraniczny()
        {
            int wynik;
            int a = 0;
            if (comboBox5.SelectedIndex < 0)
                wynik = kraj_zagranica * 1000 + comboBox6.SelectedIndex * 1000 + gabaryt * 100 + comboBox1.SelectedIndex * 10 + a;
            else wynik = kraj_zagranica * 1000 + comboBox6.SelectedIndex * 1000 + gabaryt * 100 + comboBox1.SelectedIndex * 10 + comboBox5.SelectedIndex;
            //if (checkBox2.Checked) rodzaj_list = rodzaj_list + " Gabaryt B";
            Form newform = new DodajPP(listView1, nazwa_uzytkownika, login_uzytkownika, jednostka_uzytkownika, conn, textBox1.Text, ow, cw, ra, comboBox1.Text, wynik, comboBox5.Text);
            newform.Location = Start.ActiveForm.Location;
            newform.Show();

        }

        private void dodajlistkrajowy()
        {
            int wynik;
            string rodzaj_list = comboBox1.Text;
            if (checkBox2.Checked) rodzaj_list = rodzaj_list + " Gabaryt B";
            int a = 0;
            if (comboBox5.SelectedIndex < 0 )
            wynik = kraj_zagranica * 1000 + gabaryt * 100 + comboBox1.SelectedIndex * 10 + a;
            else wynik = kraj_zagranica * 1000 + gabaryt * 100 + comboBox1.SelectedIndex * 10 + comboBox5.SelectedIndex;
            Form newform = new DodajPP(listView1, nazwa_uzytkownika, login_uzytkownika, jednostka_uzytkownika, conn, textBox1.Text,ow,cw,ra, rodzaj_list, wynik,comboBox5.Text);
            newform.Location = Start.ActiveForm.Location;
            newform.Show();
        }

        private void button3_Click(object sender, EventArgs e)//przycisk usun list
        {
            string numer;
            ListViewItem item = listView1.SelectedItems[0];
            numer = item.SubItems[5].Text;
            string delete = "DELETE FROM poczta_polska WHERE Lp = @lp";
            MySqlCommand cmd = new MySqlCommand(delete, conn.connection);
            cmd.Parameters.AddWithValue("@lp", Int32.Parse(numer));
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            foreach (ListViewItem lv in listView1.SelectedItems)
                lv.Remove();
        
    }

        private void button4_Click(object sender, EventArgs e)//popraw list
        {
            ListViewItem item = listView1.SelectedItems[0];
            string numer = item.SubItems[5].Text;
            Form nf = new PoprawPocztaPolska(conn,listView1,numer);
            nf.Location = Start.ActiveForm.Location;
            nf.Show();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {//button drukuj wykaz
            if (String.IsNullOrEmpty(textBox2.Text))
                MessageBox.Show("Brak numeru Wykazu", "Przypominacz", MessageBoxButtons.OK, MessageBoxIcon.Question);
            else
            {
                listView1.Items.Clear();
                if (comboBox7.SelectedIndex == 1) wydruk_poczta_zwykle();
                if (comboBox7.SelectedIndex == 4) wydruk_poczta_zwykle();
                if (comboBox7.SelectedIndex == 2) wydruk_poczta_polecone();
                if (comboBox7.SelectedIndex == 5) wydruk_poczta_polecone();
                if (comboBox7.SelectedIndex == 3) wydruk_poczta_paczki();
            }
        }

        private void wydruk_poczta_zwykle()
        {
            if (lp > 1)
            {
                new WykazPocztaPDF(textBox2.Text, conn, login_uzytkownika, nazwa_uzytkownika,jednostka_uzytkownika,tabelka,lp);
            }
            else MessageBox.Show("Brak listów do wydruku", "Przypominacz", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void wydruk_poczta_polecone()
        {
            if (lp > 1)
            {
                new WykazPocztaPoleconePDF(textBox2.Text, conn, login_uzytkownika, nazwa_uzytkownika, jednostka_uzytkownika, tabelka, lp);
            }
            else MessageBox.Show("Brak listów do wydruku", "Przypominacz", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void wydruk_poczta_paczki()
        {
            if (lp > 1)
            {
                new WykazPAczkiPocztaPDF(textBox2.Text, conn, login_uzytkownika, nazwa_uzytkownika, jednostka_uzytkownika, tabelka, lp);
            }
            else MessageBox.Show("Brak listów do wydruku", "Przypominacz", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox7.SelectedIndex == 0) { zeruj_tabelke(); tryb0(); button5.Enabled = false; }
            if (comboBox7.SelectedIndex == 1) { zeruj_tabelke(); tryb1(); button5.Enabled = true; }//zwykle
            if (comboBox7.SelectedIndex == 2) { zeruj_tabelke(); tryb2(); button5.Enabled = true; }//polecone
            if (comboBox7.SelectedIndex == 3) { zeruj_tabelke(); tryb3(); button5.Enabled = true; }//paczki
            if (comboBox7.SelectedIndex == 4) { zeruj_tabelke(); tryb4(); button5.Enabled = true; }//zwykle zagraniczne
            if (comboBox7.SelectedIndex == 5) { zeruj_tabelke(); tryb5(); button5.Enabled = true; }//polecone zagraniczne

        }

        private void tryb5()
        {

            listView1.Items.Clear();
            ListViewItem list = null;
            lp = 1;
            string select = "SELECT Numer_Sprawy,Rodzaj,Adresat,Adresat_Adres,Lp,Rodzaj_numer FROM poczta_polska WHERE Tworca = @tworca";
            MySqlCommand sel = new MySqlCommand(select, conn.connection);
            sel.Parameters.AddWithValue("@tworca", login_uzytkownika);
            reader = sel.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetInt32(5) >= 2110)
                    if (reader.GetInt32(5) <= 2129)
                    {
                        list = new ListViewItem(lp.ToString());
                        list.SubItems.Add(reader.GetString(0));
                        list.SubItems.Add(reader.GetString(1));
                        list.SubItems.Add(reader.GetString(2));
                        list.SubItems.Add(reader.GetString(3));
                        list.SubItems.Add(reader.GetInt32(4).ToString());
                        listView1.Items.Add(list);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        tabelka[lp - 1] = reader.GetInt32(4);
                        lp++;
                    }

                if (reader.GetInt32(5) >= 3110)
                    if (reader.GetInt32(5) <= 3129)
                    {
                        list = new ListViewItem(lp.ToString());
                        list.SubItems.Add(reader.GetString(0));
                        list.SubItems.Add(reader.GetString(1));
                        list.SubItems.Add(reader.GetString(2));
                        list.SubItems.Add(reader.GetString(3));
                        list.SubItems.Add(reader.GetInt32(4).ToString());
                        listView1.Items.Add(list);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        tabelka[lp - 1] = reader.GetInt32(4);
                        lp++;
                    }


                if (reader.GetInt32(5) >= 4110)
                    if (reader.GetInt32(5) <= 4129)
                    {
                        list = new ListViewItem(lp.ToString());
                        list.SubItems.Add(reader.GetString(0));
                        list.SubItems.Add(reader.GetString(1));
                        list.SubItems.Add(reader.GetString(2));
                        list.SubItems.Add(reader.GetString(3));
                        list.SubItems.Add(reader.GetInt32(4).ToString());
                        listView1.Items.Add(list);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        tabelka[lp - 1] = reader.GetInt32(4);
                        lp++;
                    }

                if (reader.GetInt32(5) >= 5110)
                    if (reader.GetInt32(5) <= 5129)
                    {
                        list = new ListViewItem(lp.ToString());
                        list.SubItems.Add(reader.GetString(0));
                        list.SubItems.Add(reader.GetString(1));
                        list.SubItems.Add(reader.GetString(2));
                        list.SubItems.Add(reader.GetString(3));
                        list.SubItems.Add(reader.GetInt32(4).ToString());
                        listView1.Items.Add(list);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        tabelka[lp - 1] = reader.GetInt32(4);
                        lp++;
                    }


            }//koniec while
            reader.Close();
        }//konic tryb 5

        private void tryb4()
        {

            listView1.Items.Clear();
            ListViewItem list = null;
            lp = 1;
            string select = "SELECT Numer_Sprawy,Rodzaj,Adresat,Adresat_Adres,Lp,Rodzaj_numer FROM poczta_polska WHERE Tworca = @tworca";
            MySqlCommand sel = new MySqlCommand(select, conn.connection);
            sel.Parameters.AddWithValue("@tworca", login_uzytkownika);
            reader = sel.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetInt32(5) >= 2100)
                    if (reader.GetInt32(5) <= 2109)
                    {
                        list = new ListViewItem(lp.ToString());
                        list.SubItems.Add(reader.GetString(0));
                        list.SubItems.Add(reader.GetString(1));
                        list.SubItems.Add(reader.GetString(2));
                        list.SubItems.Add(reader.GetString(3));
                        list.SubItems.Add(reader.GetInt32(4).ToString());
                        listView1.Items.Add(list);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        tabelka[lp - 1] = reader.GetInt32(4);
                        lp++;
                    }
                    
                                        if (reader.GetInt32(5) >= 3100)
                        if (reader.GetInt32(5) <= 3109)
                        {
                            list = new ListViewItem(lp.ToString());
                            list.SubItems.Add(reader.GetString(0));
                            list.SubItems.Add(reader.GetString(1));
                            list.SubItems.Add(reader.GetString(2));
                            list.SubItems.Add(reader.GetString(3));
                            list.SubItems.Add(reader.GetInt32(4).ToString());
                            listView1.Items.Add(list);
                            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                            tabelka[lp - 1] = reader.GetInt32(4);
                            lp++;
                        }


                if (reader.GetInt32(5) >= 4100)
                    if (reader.GetInt32(5) <= 4109)
                    {
                        list = new ListViewItem(lp.ToString());
                        list.SubItems.Add(reader.GetString(0));
                        list.SubItems.Add(reader.GetString(1));
                        list.SubItems.Add(reader.GetString(2));
                        list.SubItems.Add(reader.GetString(3));
                        list.SubItems.Add(reader.GetInt32(4).ToString());
                        listView1.Items.Add(list);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        tabelka[lp - 1] = reader.GetInt32(4);
                        lp++;
                    }
                    
                                        if (reader.GetInt32(5) >= 5100)
                        if (reader.GetInt32(5) <= 5109)
                        {
                            list = new ListViewItem(lp.ToString());
                            list.SubItems.Add(reader.GetString(0));
                            list.SubItems.Add(reader.GetString(1));
                            list.SubItems.Add(reader.GetString(2));
                            list.SubItems.Add(reader.GetString(3));
                            list.SubItems.Add(reader.GetInt32(4).ToString());
                            listView1.Items.Add(list);
                            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                            tabelka[lp - 1] = reader.GetInt32(4);
                            lp++;
                        }


            }//koniec while
            reader.Close();
        }//konic tryb 4

        private void tryb3()
        {

            listView1.Items.Clear();
            ListViewItem list = null;
            lp = 1;
            string select = "SELECT Numer_Sprawy,Rodzaj,Adresat,Adresat_Adres,Lp,Rodzaj_numer FROM poczta_polska WHERE Tworca = @tworca";
            MySqlCommand sel = new MySqlCommand(select, conn.connection);
            sel.Parameters.AddWithValue("@tworca", login_uzytkownika);
            reader = sel.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetInt32(5) >= 1160)
                    if (reader.GetInt32(5) <= 1199)
                    {
                        list = new ListViewItem(lp.ToString());
                        list.SubItems.Add(reader.GetString(0));
                        list.SubItems.Add(reader.GetString(1));
                        list.SubItems.Add(reader.GetString(2));
                        list.SubItems.Add(reader.GetString(3));
                        list.SubItems.Add(reader.GetInt32(4).ToString());
                        listView1.Items.Add(list);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        tabelka[lp - 1] = reader.GetInt32(4);
                        lp++;
                    }
                    else
                                        if (reader.GetInt32(5) >= 1260)
                        if (reader.GetInt32(5) <= 1299)
                        {
                            list = new ListViewItem(lp.ToString());
                            list.SubItems.Add(reader.GetString(0));
                            list.SubItems.Add(reader.GetString(1));
                            list.SubItems.Add(reader.GetString(2));
                            list.SubItems.Add(reader.GetString(3));
                            list.SubItems.Add(reader.GetInt32(4).ToString());
                            listView1.Items.Add(list);
                            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                            tabelka[lp - 1] = reader.GetInt32(4);
                            lp++;
                        }


            }//koniec while
            reader.Close();
        }//konic tryb 3

        private void tryb2()
        {

            listView1.Items.Clear();
            ListViewItem list = null;
            lp = 1;
            string select = "SELECT Numer_Sprawy,Rodzaj,Adresat,Adresat_Adres,Lp,Rodzaj_numer FROM poczta_polska WHERE Tworca = @tworca";
            MySqlCommand sel = new MySqlCommand(select, conn.connection);
            sel.Parameters.AddWithValue("@tworca", login_uzytkownika);
            reader = sel.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetInt32(5) >= 1120)
                    if (reader.GetInt32(5) <= 1159)
                    {
                        list = new ListViewItem(lp.ToString());
                        list.SubItems.Add(reader.GetString(0));
                        list.SubItems.Add(reader.GetString(1));
                        list.SubItems.Add(reader.GetString(2));
                        list.SubItems.Add(reader.GetString(3));
                        list.SubItems.Add(reader.GetInt32(4).ToString());
                        listView1.Items.Add(list);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        tabelka[lp - 1] = reader.GetInt32(4);
                        lp++;
                    }
                    else
                                        if (reader.GetInt32(5) >= 1220)
                        if (reader.GetInt32(5) <= 1259)
                        {
                            list = new ListViewItem(lp.ToString());
                            list.SubItems.Add(reader.GetString(0));
                            list.SubItems.Add(reader.GetString(1));
                            list.SubItems.Add(reader.GetString(2));
                            list.SubItems.Add(reader.GetString(3));
                            list.SubItems.Add(reader.GetInt32(4).ToString());
                            listView1.Items.Add(list);
                            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                            tabelka[lp - 1] = reader.GetInt32(4);
                            lp++;
                        }


            }//koniec while
            reader.Close();
        }//konic tryb 2

        private void tryb1()
        {

            listView1.Items.Clear();
            ListViewItem list = null;
            lp = 1;
            string select = "SELECT Numer_Sprawy,Rodzaj,Adresat,Adresat_Adres,Lp,Rodzaj_numer FROM poczta_polska WHERE Tworca = @tworca";
            MySqlCommand sel = new MySqlCommand(select, conn.connection);
            sel.Parameters.AddWithValue("@tworca", login_uzytkownika);
            reader = sel.ExecuteReader();
            while (reader.Read())
            { 
            if (reader.GetInt32(5) >= 1100)
                if (reader.GetInt32(5) <= 1119)
                {
                    list = new ListViewItem(lp.ToString());
                    list.SubItems.Add(reader.GetString(0));
                    list.SubItems.Add(reader.GetString(1));
                    list.SubItems.Add(reader.GetString(2));
                    list.SubItems.Add(reader.GetString(3));
                    list.SubItems.Add(reader.GetInt32(4).ToString());
                    listView1.Items.Add(list);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        tabelka[lp - 1] = reader.GetInt32(4);
                        lp++;
                }
            else
                                    if (reader.GetInt32(5) >= 1200)
                        if (reader.GetInt32(5) <= 1219)
                        {
                            list = new ListViewItem(lp.ToString());
                            list.SubItems.Add(reader.GetString(0));
                            list.SubItems.Add(reader.GetString(1));
                            list.SubItems.Add(reader.GetString(2));
                            list.SubItems.Add(reader.GetString(3));
                            list.SubItems.Add(reader.GetInt32(4).ToString());
                            listView1.Items.Add(list);
                            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                            tabelka[lp - 1] = reader.GetInt32(4);
                            lp++;
                        }


            }//koniec while
            reader.Close();
        }//konic tryb 1


        private void tryb0()
        {
            
            listView1.Items.Clear();
            ListViewItem list = null;
            lp = 1;
            string select = "SELECT Numer_Sprawy,Rodzaj,Adresat,Adresat_Adres,Lp FROM poczta_polska WHERE Tworca = @tworca";
            MySqlCommand sel = new MySqlCommand(select, conn.connection);
            sel.Parameters.AddWithValue("@tworca", login_uzytkownika);
            reader = sel.ExecuteReader();
            while (reader.Read())
            {
                list = new ListViewItem(lp.ToString());
                list.SubItems.Add(reader.GetString(0));
                list.SubItems.Add(reader.GetString(1));
                list.SubItems.Add(reader.GetString(2));
                list.SubItems.Add(reader.GetString(3));
                list.SubItems.Add(reader.GetInt32(4).ToString());
                listView1.Items.Add(list);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                tabelka[lp - 1] = reader.GetInt32(4);
                lp++;
            }
            reader.Close();
        }//konic tryb 0

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button3.Enabled = true;
            button4.Enabled = true;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            sprawdz_button();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            sprawdz_button();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            sprawdz_button();
        }

        private void uzupelnij()//combobox do wag listow/paczek
        {
            comboBox5.Items.Clear();
            if (kraj_zagranica == 1)
            {
                if (comboBox1.SelectedIndex <= 5)
                {
                    try
                    {
                        StreamReader sr = new StreamReader(@"C:\Program\Sources\wagalistyk.txt", Encoding.GetEncoding("ISO-8859-2"));
                        string line = sr.ReadLine();

                        while (line != null)
                        {
                            comboBox5.Items.Add(line);
                            line = sr.ReadLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Błąd pliku" + ex.Message);
                    }
                }//koniec if

                if (comboBox1.SelectedIndex > 5)
                {
                    try
                    {
                        StreamReader sr = new StreamReader(@"C:\Program\Sources\wagapaczkik.txt", Encoding.GetEncoding("ISO-8859-2"));
                        string line = sr.ReadLine();

                        while (line != null)
                        {
                            comboBox5.Items.Add(line);
                            line = sr.ReadLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Błąd pliku" + ex.Message);
                    }
                }//koniec if
            }
            else
            {
                try
                {
                    StreamReader sr = new StreamReader(@"C:\Program\Sources\wagalistyz.txt", Encoding.GetEncoding("ISO-8859-2"));
                    string line = sr.ReadLine();

                    while (line != null)
                    {
                        comboBox5.Items.Add(line);
                        line = sr.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd pliku" + ex.Message);
                }
            }
        }

        private void comboBox5_MouseClick(object sender, MouseEventArgs e)
        {
            uzupelnij();

        }

        private void comboBox6_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox6.Items.Clear();
            try
            {
                StreamReader sr = new StreamReader(@"C:\Program\Sources\strefa.txt", Encoding.GetEncoding("ISO-8859-2"));
                string line = sr.ReadLine();

                while (line != null)
                {
                    comboBox6.Items.Add(line);
                    line = sr.ReadLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd pliku" + ex.Message);
            }
        }
    }
}
