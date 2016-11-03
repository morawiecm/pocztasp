using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;

namespace SIPS
{
    public partial class PocztaSpecjalna : Form
    {
        string nazwa_pliku,aaa,bbb;
        string nazwa_uzytkownika,login_uzytkownika,jednostka_uzytkownika, zestawienie_kontrola;
        Connect_Mysql conn;
        Form old;
        int typ_przesylki;//1 - jawna 2 - niejawna
        int rodzaj_przesylki;//1 - list 2- paczka
        string t_p, r_p,adre,adr1,adr2,ro;
        string insert,typek,zestawienie;
        int numer, czy_przyjeto_wykaz;
        long numer_mysql;
        ListViewItem item;
        int indeks,wynik;
        int typ_zestawienia, rodzaj_zestawienia;
        int tryb_wyswietlania;//czy zakładka adresat na wydruku ma być pusta czy ma być drukowany adresat
        int[] tabelka = new int[500];


        /// <summary>
        /// konstruktor glownej formy poczty specjalnej
        /// </summary>
        /// <param name="lu">login uzytkownika</param>
        /// <param name="con">polaczenie mysql</param>
        /// <param name="nu">imie i nazwisko</param>
        /// <param name="ju">jednostka i wydzial</param>
        /// <param name="ol">forma startowa</param>

        public PocztaSpecjalna(string lu,Connect_Mysql con, string nu, string ju, Form ol)
        {
            this.conn = con;
            this.nazwa_uzytkownika = nu;
            this.login_uzytkownika = lu;
            this.jednostka_uzytkownika = ju;
            InitializeComponent();
            this.old = ol;
        }

        private void zeruj_tabelke()//zerowanie tabeli z podrecznymi indexami
        {
            for (int i = 0; i < tabelka.Length; i++)
                tabelka[i] = 0;
        }

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = -1;
           // comboBox2.Items.Clear();
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

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox1.Items.Clear();
            button1.Enabled = false;
            //comboBox2.SelectedItem = -1;
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

        private void tabPage1_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Start.ActiveForm.Close();
            old.Show();
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
           if ( radioButton1.Checked == true )
            radioButton2.Checked = false;
            typ_przesylki = 1;//jawne
            t_p = "Jawne";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
                radioButton1.Checked = false;
            typ_przesylki = 2;//niejawne
            t_p = "Niejawne";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           if ( checkBox1.Checked == true ) 
            checkBox2.Checked = false;//wybor paczka/list
            rodzaj_przesylki = 1;
            r_p = "List";
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
                checkBox1.Checked = false;//wybor paczka/list
            rodzaj_przesylki = 2;
            r_p = "Paczka";
        }

        /// <summary>
        /// przycisk usun list z mysql wybrany z listy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem itemek in listView1.SelectedItems)
            {
                string numer;
                ListViewItem item = listView1.SelectedItems[0];
                numer = item.SubItems[4].Text;
                string delete = "DELETE FROM resort WHERE Lp = @lp";
                MySqlCommand cmd = new MySqlCommand(delete, conn.connection);
                cmd.Parameters.AddWithValue("@lp", Int32.Parse(numer));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                foreach (ListViewItem lv in listView1.SelectedItems)
                    lv.Remove();
                cmd.Dispose();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            button2.Enabled = true;
            button3.Enabled = true;

        }

        private void listView2_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem itemek in listView2.SelectedItems)
            {
                button7.Enabled = true;
                button8.Enabled = true;
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {//button wyswietl
            button7.Enabled = false;
            button8.Enabled = false;

            {
                zestawienie = "Zestawienie" + textBox4.Text;
                string select_zestawienie = "SELECT Typ, Rodzaj, Nazwa, Cel_numer, Lp, Uwagi FROM resort WHERE Bufor = @bufor";
                listView2.Items.Clear();
                ListViewItem lvi = null;
                int lp = 1;
                MySqlCommand cmd = new MySqlCommand(select_zestawienie, conn.connection);
                cmd.Parameters.AddWithValue("@bufor", zestawienie);
                MySqlDataReader reader = null;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lvi = new ListViewItem(lp.ToString());
                    lvi.SubItems.Add(reader.GetString(2));
                    zamien_num(reader.GetInt32(3));
                    lvi.SubItems.Add(adre);

                    if (reader.GetInt32(1) == 1) lvi.SubItems.Add("List");
                    else lvi.SubItems.Add("Paczka");

                    if (reader.GetInt32(0) == 1) lvi.SubItems.Add("Jawny");
                    else lvi.SubItems.Add("Niejawny");
                    lvi.SubItems.Add(reader.GetString(5));
                    lvi.SubItems.Add(reader.GetString(4));

                    listView2.Items.Add(lvi);
                    lp++;
                    listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
                reader.Close();
                cmd.Dispose();
                czy_przyjeto_wykaz = 1;
            }
        }

        /// <summary>
        /// przycisk usun list z resortu z bazy mysql
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button8_Click(object sender, EventArgs e)

        {
            foreach (ListViewItem itemek in listView2.SelectedItems)
            {
                int indd;
                ListViewItem item = listView2.SelectedItems[0];
                indd = Int32.Parse(item.SubItems[6].Text);
                string delete = "DELETE FROM resort WHERE Lp = @lp";
                MySqlCommand cmd = new MySqlCommand(delete, conn.connection);
                cmd.Parameters.AddWithValue("@lp", indd);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();

                foreach (ListViewItem lv in listView2.SelectedItems)
                    lv.Remove();
                cmd.Dispose();
            }
        }



        private void button7_Click(object sender, EventArgs e)
        {//button popraw przyjmij zestawienie
            foreach (ListViewItem itemek in listView2.SelectedItems)
            {
                ListViewItem item = listView2.SelectedItems[0];
                int ind_popraw;
                ind_popraw = Int32.Parse(item.SubItems[6].Text);
                if (item.SubItems[4].Text.Equals("Jawny")) typ_przesylki = 1; else
                    typ_przesylki = 2;

                Form newform = new Popraw_zestawienie(conn, ind_popraw, listView2);
                newform.Location = Start.ActiveForm.Location;
                newform.Show();
            }
        }

        /// <summary>
        /// combobox w zakładfce zestawienie - wybor jednostki
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void comboBox3_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox3.Items.Clear();
            listView3.Items.Clear();
            textBox5.Text = "5/1";
            try
            {
                StreamReader sr = new StreamReader(@"C:\Program\Sources\jednostkazest.txt", Encoding.GetEncoding("ISO-8859-2"));
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
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.SelectedIndex = -1;
          //  comboBox4.Items.Clear();
            sprawdz_button9();
        }

        /// <summary>
        /// combobox zestawienie - wybor wydziału
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void comboBox4_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox4.Items.Clear();

            if (comboBox3.SelectedIndex == 0) nazwa_pliku = "kwpgorzow.txt";
            if (comboBox3.SelectedIndex == 1) nazwa_pliku = "kwpgorzowinstytucje.txt";
            if (comboBox3.SelectedIndex == 2) nazwa_pliku = "kmpgorzow.txt";
            if (comboBox3.SelectedIndex == 3) nazwa_pliku = "kmpzielonagora.txt";
            if (comboBox3.SelectedIndex == 4) nazwa_pliku = "kmpzielonagorainstytucje.txt";
            if (comboBox3.SelectedIndex == 5) nazwa_pliku = "krosnoodrzanskie.txt";
            if (comboBox3.SelectedIndex == 6) nazwa_pliku = "krosnoinstytucje.txt";
            if (comboBox3.SelectedIndex == 7) nazwa_pliku = "nosgkrosno.txt";
            if (comboBox3.SelectedIndex == 8) nazwa_pliku = "kppmiedzyrzecz.txt";
            if (comboBox3.SelectedIndex == 9) nazwa_pliku = "kppnowasol.txt";
            if (comboBox3.SelectedIndex == 10) nazwa_pliku = "kpskwierzyna.txt";
            if (comboBox3.SelectedIndex == 11) nazwa_pliku = "skwierzynajw.txt";
            if (comboBox3.SelectedIndex == 12) nazwa_pliku = "kppslubice.txt";
            if (comboBox3.SelectedIndex == 13) nazwa_pliku = "kppstrzelce.txt";
            if (comboBox3.SelectedIndex == 14) nazwa_pliku = "kpsulechow.txt";
            if (comboBox3.SelectedIndex == 15) nazwa_pliku = "kppsulecin.txt";
            if (comboBox3.SelectedIndex == 16) nazwa_pliku = "kppswiebodzin.txt";
            if (comboBox3.SelectedIndex == 17) nazwa_pliku = "kppwschowa.txt";
            if (comboBox3.SelectedIndex == 18) nazwa_pliku = "kppzagan.txt";
            if (comboBox3.SelectedIndex == 19) nazwa_pliku = "245zagan.txt";
            if (comboBox3.SelectedIndex == 20) nazwa_pliku = "kppzary.txt";
            if (comboBox3.SelectedIndex == 21) nazwa_pliku = "kraj.txt";

            try
            {
                StreamReader sr = new StreamReader(@"C:\Program\Sources\" + nazwa_pliku, Encoding.GetEncoding("ISO-8859-2"));
                string line = sr.ReadLine();
                while (line != null)
                {
                    comboBox4.Items.Add(line);
                    line = sr.ReadLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked) checkBox4.Checked = false;
            if (checkBox3.Checked == false) checkBox4.Checked = true;
            rodzaj_zestawienia = 1;
            sprawdz_button9();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked) checkBox3.Checked = false;
            if (checkBox4.Checked == false) checkBox3.Checked = true;
            rodzaj_zestawienia = 2;
            sprawdz_button9();

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked) checkBox6.Checked = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            typ_zestawienia = 1;
            t_p = "Jawne";
            sprawdz_button9();
            if (comboBox3.SelectedIndex == 3) aaa = bbb;

        }

        private void sprawdz_button9()
        {
            if (radioButton3.Checked || radioButton4.Checked)
                if (checkBox3.Checked || checkBox4.Checked)
                    if ( comboBox3.SelectedIndex > -1)
                    button9.Enabled = true; 
        }



        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            typ_zestawienia = 2;
            t_p = "Niejawne";
            sprawdz_button9();
            if (comboBox3.SelectedIndex == 3) aaa = bbb;
        }


        /// <summary>
        /// drukuj zestawienie ( generuje pdf ) z zakładki zestawienie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button10_Click(object sender, EventArgs e)
        {//drukuj wykaz
             string odbiorca = null;
            button13.Enabled = false;
            button14.Enabled = false;
            
            if (comboBox4.SelectedIndex > -1)
                odbiorca = comboBox4.Text;
            else odbiorca = comboBox3.Text;

            if (String.IsNullOrEmpty(textBox6.Text)) { }
            else odbiorca = textBox6.Text;

                if (String.IsNullOrEmpty(textBox5.Text))
                MessageBox.Show("Brak numeru Wykazu", "Przypominacz", MessageBoxButtons.OK, MessageBoxIcon.Question);
            else
            {
                if ( listView3.Items.Count < 1)
                    MessageBox.Show("Brak Listów do wydruku", "Przypominacz", MessageBoxButtons.OK, MessageBoxIcon.Question);
                else
                { //metoda drukowania wykazu zestawienia z poczty specjalnej

                    new ZestawieniePS(conn, login_uzytkownika, nazwa_uzytkownika, jednostka_uzytkownika, textBox5.Text, odbiorca, listView3, tryb_wyswietlania);
                }
            }
            textBox6.Text = "";

        }



        private void listView3_MouseClick_1(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem itemek in listView3.SelectedItems)
            {
                button13.Enabled = true;
                button14.Enabled = true;
                item = listView3.SelectedItems[0];
                indeks = Int32.Parse(item.SubItems[3].Text);
                string sel = "SELECT Rodzaj, Nazwa, Cel_numer, Tworca FROM resort WHERE Lp = @lp";
                MySqlCommand cmd = new MySqlCommand(sel, conn.connection);
                cmd.Parameters.AddWithValue("@lp", indeks);
                MySqlDataReader reader = null;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetInt32(0) == 1) { checkBox5.Checked = true; if (checkBox5.Checked == true) checkBox6.Checked = false; }
                    else { checkBox6.Checked = true; if (checkBox6.Checked == true) checkBox5.Checked = false; }

                    textBox7.Text = reader.GetString(1);
                    zamien_numerek(reader.GetInt32(2));
                    comboBox5.SelectedIndex = comboBox5.Items.IndexOf(adr1);
                    wyp_cb6();
                    comboBox6.SelectedIndex = comboBox6.Items.IndexOf(adr2);
                    textBox13.Text = reader.GetString(3);

                }//koniec while
                reader.Close();
            }
        }//koniec void listview3 mouse click

        private void wyp_cb6()
        {
            string nazwa_pliku = null;
            comboBox6.Items.Clear();
            if (comboBox5.SelectedIndex == 0) nazwa_pliku = "kwpgorzow.txt";
            if (comboBox5.SelectedIndex == 1) nazwa_pliku = "kwpgorzowinstytucje.txt";
            if (comboBox5.SelectedIndex == 2) nazwa_pliku = "kmpgorzow.txt";
            if (comboBox5.SelectedIndex == 3) nazwa_pliku = "kmpzielonagora.txt";
            if (comboBox5.SelectedIndex == 4) nazwa_pliku = "kmpzielonagorainstytucje.txt";
            if (comboBox5.SelectedIndex == 5) nazwa_pliku = "krosnoodrzanskie.txt";
            if (comboBox5.SelectedIndex == 6) nazwa_pliku = "krosnoinstytucje.txt";
            if (comboBox5.SelectedIndex == 7) nazwa_pliku = "nosgkrosno.txt";
            if (comboBox5.SelectedIndex == 8) nazwa_pliku = "kppmiedzyrzecz.txt";
            if (comboBox5.SelectedIndex == 9) nazwa_pliku = "kppnowasol.txt";
            if (comboBox5.SelectedIndex == 10) nazwa_pliku = "kpskwierzyna.txt";
            if (comboBox5.SelectedIndex == 11) nazwa_pliku = "skwierzynajw.txt";
            if (comboBox5.SelectedIndex == 12) nazwa_pliku = "kppslubice.txt";
            if (comboBox5.SelectedIndex == 13) nazwa_pliku = "kppstrzelce.txt";
            if (comboBox5.SelectedIndex == 14) nazwa_pliku = "kpsulechow.txt";
            if (comboBox5.SelectedIndex == 15) nazwa_pliku = "kppsulecin.txt";
            if (comboBox5.SelectedIndex == 16) nazwa_pliku = "kppswiebodzin.txt";
            if (comboBox5.SelectedIndex == 17) nazwa_pliku = "kppwschowa.txt";
            if (comboBox5.SelectedIndex == 18) nazwa_pliku = "kppzagan.txt";
            if (comboBox5.SelectedIndex == 19) nazwa_pliku = "245zagan.txt";
            if (comboBox5.SelectedIndex == 20) nazwa_pliku = "kppzary.txt";
            if (comboBox5.SelectedIndex == 21) nazwa_pliku = "kraj.txt";

            try
            {
                StreamReader sr = new StreamReader(@"C:\Program\Sources\" + nazwa_pliku, Encoding.GetEncoding("ISO-8859-2"));
                string line = sr.ReadLine();
                while (line != null)
                {
                    comboBox6.Items.Add(line);
                    line = sr.ReadLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        /// <summary>
        /// zamiana numeru adresata na jego nazwe pobierana z tabeli adresat
        /// </summary>
        /// <param name="numerekk">numer pobraany z wiersza listu resortowego lub poczty polskiej</param>

        private void zamien_numerek(int numerekk)
        {
            string sel = "SELECT Nazwa FROM adresat WHERE Numer = @numer";
            Connect_Mysql c = new Connect_Mysql();
            c.connection.Open();
            MySqlCommand cmd = new MySqlCommand(sel, c.connection);
            cmd.Parameters.AddWithValue("@numer", numerekk);
            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                adre = reader.GetString(0);
            }//koniec while reader
            reader.Close();
            c.CloseConnection();

            string[] dziel = adre.Split(new string[] { " - " }, StringSplitOptions.None);
            adr1 = dziel[0];
            adr2 = dziel[1];
            comboBox5.Items.Clear();
            comboBox6.Items.Clear();
            comboBox5.SelectedItem = -1;
            comboBox6.SelectedItem = -1;
            //comboBox1.SelectedItem = -1;
            try
            {
                StreamReader sr = new StreamReader(@"C:\Program\Sources\jednostka.txt", Encoding.GetEncoding("ISO-8859-2"));
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void comboBox5_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox5.Items.Clear();
            try
            {
                StreamReader sr = new StreamReader(@"C:\Program\Sources\jednostka.txt", Encoding.GetEncoding("ISO-8859-2"));
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

        private void button14_Click(object sender, EventArgs e)
        {//button popraw w zestawieniach
            foreach (ListViewItem itemek in listView3.SelectedItems)
            {
                item = listView3.SelectedItems[0];
                indeks = Int32.Parse(item.SubItems[3].Text);
                string update = "UPDATE resort SET Rodzaj = @rodzaj , Nazwa = @nazwa , Cel_numer = @cel_numer , Tworca = @tworca WHERE Lp = @lp";
                MySqlCommand cmd = new MySqlCommand(update, conn.connection);
                if (checkBox5.Checked) { cmd.Parameters.AddWithValue("@rodzaj", 1); ro = "List"; }
                else { cmd.Parameters.AddWithValue("@rodzaj", 2); ro = "Paczka"; }
                cmd.Parameters.AddWithValue("@nazwa", textBox7.Text);
                if (comboBox6.SelectedIndex == -1)
                    numer = comboBox5.SelectedIndex * 100;
                else
                    numer = comboBox5.SelectedIndex * 100 + comboBox6.SelectedIndex;
                cmd.Parameters.AddWithValue("@cel_numer", numer);
                cmd.Parameters.AddWithValue("@tworca", textBox13.Text);
                cmd.Parameters.AddWithValue("@lp", indeks);
                
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                updatelist3();
            }
        }

        /// <summary>
        /// odswiezanie listy listview
        /// </summary>

        private void updatelist3()
        {
            ListViewItem item = new ListViewItem();
            item = this.listView3.SelectedItems[0];
            item.SubItems[1].Text = textBox7.Text;//rodzaj
            item.SubItems[2].Text = ro;//adresat


            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void button13_Click(object sender, EventArgs e)
        { //button usun w zestawieniach
            foreach (ListViewItem itemek in listView3.SelectedItems)
            {
                string numer;
                ListViewItem item = listView3.SelectedItems[0];
                numer = item.SubItems[3].Text;
                string delete = "DELETE FROM resort WHERE Lp = @lp";
                MySqlCommand cmd = new MySqlCommand(delete, conn.connection);
                cmd.Parameters.AddWithValue("@lp", Int32.Parse(numer));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                foreach (ListViewItem lv in listView3.SelectedItems)
                    lv.Remove();
                cmd.Dispose();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        { //button szukaj niejawne w bazie uzytkownika czyli zakładka baza listów
            button15.Enabled = true;
            button16.Enabled = false;
            button17.Enabled = false;
            MySqlDataReader reader = null;
            ListViewItem lvi = null;
            typ_przesylki = 2;
            int lpi = 1;
            ro = "";
            string select = "SELECT Nazwa, Rodzaj, Cel_numer, Uwagi, Lp FROM resort WHERE Bufor = @bufor AND Typ = 2";
            listView4.Items.Clear();
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            cmd.Parameters.AddWithValue("@bufor", login_uzytkownika);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lvi = new ListViewItem(lpi.ToString());
                lvi.SubItems.Add(reader.GetString(0));
                if (reader.GetInt16(1) == 1) ro = "List"; else ro = "Paczka";
                lvi.SubItems.Add(ro);
                zamien_num(reader.GetInt32(2));
                lvi.SubItems.Add(adre);
                lvi.SubItems.Add(reader.GetString(3));
                lvi.SubItems.Add(reader.GetInt32(4).ToString());
                listView4.Items.Add(lvi);
                lpi++;
                listView4.Columns[2].TextAlign = HorizontalAlignment.Center;
                listView4.Columns[5].TextAlign = HorizontalAlignment.Right;
            }
            reader.Close();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if ( !String.IsNullOrEmpty(textBox8.Text) )
                {
                button16.Enabled = false;
                button17.Enabled = false;
                foreach (ListViewItem item in listView4.Items)
                {
                    
                    if (item.SubItems[1].Text.ToLower().Contains(textBox8.Text.ToLower()))
                    {
                        item.Selected = true;

                    }
                }//koniec foreach
                }//koniec if !string....
            RemoveUnselected(listView4);
            button15.Enabled = false;
        }


        /// <summary>
        /// usuniecie wybranego wiersza z listview
        /// </summary>
        /// <param name="lst">przekazanie listview</param>

        private static void RemoveUnselected(ListView lst)
    {
            int i = 0;
            while (true)

            {
                if (i >= lst.Items.Count)
            {
                    break;
            }
            
            if (lst.Items[i].Selected == false)
            {
                lst.Items[i].Remove();
                i--;
            }
            i++;
        }
    }


        /// <summary>
        /// usuniecie listu z resortu oraz z bazy mysql
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button16_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem itemek in listView4.SelectedItems)
            {
                string delete = "DELETE FROM resort WHERE Lp = @lp";
                ListViewItem item = listView4.SelectedItems[0];
                numer = Int32.Parse(item.SubItems[5].Text);
                MySqlCommand cmd = new MySqlCommand(delete, conn.connection);
                cmd.Parameters.AddWithValue("@lp", numer);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                foreach (ListViewItem lv in listView4.SelectedItems)
                    lv.Remove();
                cmd.Dispose();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {//button szukaj jawne w bazie uzytkownika
            button15.Enabled = true;
            button16.Enabled = false;
            button17.Enabled = false;
            typ_przesylki = 1;
            int lpi = 1;
            MySqlDataReader reader = null;
            ListViewItem lvi = null;
            ro = "";
            string select = "SELECT Nazwa, Rodzaj, Cel_numer, Uwagi, Lp FROM resort WHERE Bufor = @bufor AND Typ = 1";
            listView4.Items.Clear();
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            cmd.Parameters.AddWithValue("@bufor", login_uzytkownika);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lvi = new ListViewItem(lpi.ToString());
                lvi.SubItems.Add(reader.GetString(0));
                if (reader.GetInt16(1) == 1) ro = "List"; else ro = "Paczka";
                lvi.SubItems.Add(ro);
                zamien_num(reader.GetInt32(2));
                lvi.SubItems.Add(adre);
                lvi.SubItems.Add(reader.GetString(3));
                lvi.SubItems.Add(reader.GetInt32(4).ToString());
                listView4.Items.Add(lvi);
                lpi++;
                listView4.Columns[2].TextAlign = HorizontalAlignment.Center;
                listView4.Columns[5].TextAlign = HorizontalAlignment.Right;

    
                listView4.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView4.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            reader.Close();
        }

        private void listView4_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem itemek in listView4.SelectedItems)
            {
                button16.Enabled = true;
                button17.Enabled = true;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {//button wyswietl poczta polska 
            if (!String.IsNullOrEmpty(textBox9.Text))
            {
                button20.Enabled = false;
                button21.Enabled = false;
                zestawienie = "Zestawienie" + textBox9.Text;
                string select_zestawienie = "SELECT Rodzaj, Waga, Numer_Sprawy, Adresat, Adresat_Adres, Lp FROM poczta_polska WHERE Tworca = @bufor";
                listView5.Items.Clear();
                ListViewItem lvi = null;
                int lp = 1;
                MySqlCommand cmd = new MySqlCommand(select_zestawienie, conn.connection);
                cmd.Parameters.AddWithValue("@bufor", zestawienie);
                MySqlDataReader reader = null;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lvi = new ListViewItem(lp.ToString());
                    lvi.SubItems.Add(reader.GetString(0));
                    lvi.SubItems.Add(reader.GetString(1));
                    lvi.SubItems.Add(reader.GetString(2));
                    lvi.SubItems.Add(reader.GetString(3));
                    lvi.SubItems.Add(reader.GetString(4));
                    lvi.SubItems.Add(reader.GetString(5));
                    listView5.Items.Add(lvi);
                    listView5.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView5.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    lp++;
                }//koniec while
                reader.Close();
                cmd.Dispose();
            }
            else MessageBox.Show("Brak numeru zestawienia");
        }

        /// <summary>
        /// usuwanie listu poczty polskiej z mysql i listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button20_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem itemek in listView5.SelectedItems)
            {
                string numer;
                ListViewItem item = listView5.SelectedItems[0];
                numer = item.SubItems[6].Text;
                string delete = "DELETE FROM poczta_polska WHERE Lp = @lp";
                MySqlCommand cmd = new MySqlCommand(delete, conn.connection);
                cmd.Parameters.AddWithValue("@lp", Int32.Parse(numer));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                foreach (ListViewItem lv in listView5.SelectedItems)
                    lv.Remove();
                cmd.Dispose();
            }
        }

        private void listView5_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem itemek in listView5.SelectedItems)
            {
                button20.Enabled = true;
                button21.Enabled = true;
            }
        }

        /// <summary>
        /// przycisk popraw w poczta polska - otwiera nowa forme popraw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button21_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem itemek in listView5.SelectedItems)
            {
                int index;
                ListViewItem item = listView5.SelectedItems[0];
                index = Int32.Parse(item.SubItems[6].Text);
                Form newform = new PoprawPPwPS(conn, index, listView5);
                newform.Location = Start.ActiveForm.Location;
                newform.Show();
            }
        }

        /// <summary>
        /// przycisk drukuj zestawienie z zakładki poczta polska
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button19_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox9.Text))
                if (!String.IsNullOrEmpty(textBox10.Text))
                    if (!String.IsNullOrEmpty(textBox11.Text))
                    {
                        Form nf = new WyborDrukowania_PP_PS(textBox9.Text, textBox10.Text, textBox11.Text, conn);
                        nf.Location = Start.ActiveForm.Location;
                        nf.Show();
                    }
                    else MessageBox.Show("Brak Nadawcy Wykazu");
                else MessageBox.Show("Brak numeru Wykazu");
            else MessageBox.Show("Brak numeru Zestawienia");
        }

        /// <summary>
        /// przycisk przyjmij wykaz w poczta polska
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button23_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox9.Text))
            {
                string update = "UPDATE poczta_polska SET Przyjety = 1 , Data = @data WHERE Tworca = @tworca";
                MySqlCommand cmd = new MySqlCommand(update, conn.connection);
                cmd.Parameters.AddWithValue("@data", DateTime.Now.ToShortDateString());
                cmd.Parameters.AddWithValue("@tworca", "Zestawienie" + textBox9.Text);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                MessageBox.Show("Wykaz przyjęto poprawnie");
            }
            else MessageBox.Show("Brak numeru Zestawienia");
        }

        /// <summary>
        /// przycisk drukuj zestawienie zbiorcze z poczty polskiej w zaleznosci od wyboru z comboboxa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button22_Click(object sender, EventArgs e)
        {
            if ( comboBox7.SelectedIndex == 0 )new ZestawienieZbiorcze(jednostka_uzytkownika, conn, nazwa_uzytkownika);
            if ( comboBox7.SelectedIndex == 1 ) new ZestawienieZbiorczePocztaFirmowa(jednostka_uzytkownika, conn, nazwa_uzytkownika);
            if ( comboBox7.SelectedIndex == 2 ) new ZestawienieZbiorczeZagraniczne(jednostka_uzytkownika, conn, nazwa_uzytkownika);

        }

        private void comboBox6_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox6.Items.Clear();

            if (comboBox5.SelectedIndex == 0) nazwa_pliku = "kwpgorzow.txt";
            if (comboBox5.SelectedIndex == 1) nazwa_pliku = "kwpgorzowinstytucje.txt";
            if (comboBox5.SelectedIndex == 2) nazwa_pliku = "kmpgorzow.txt";
            if (comboBox5.SelectedIndex == 3) nazwa_pliku = "kmpzielonagora.txt";
            if (comboBox5.SelectedIndex == 4) nazwa_pliku = "kmpzielonagorainstytucje.txt";
            if (comboBox5.SelectedIndex == 5) nazwa_pliku = "krosnoodrzanskie.txt";
            if (comboBox5.SelectedIndex == 6) nazwa_pliku = "krosnoinstytucje.txt";
            if (comboBox5.SelectedIndex == 7) nazwa_pliku = "nosgkrosno.txt";
            if (comboBox5.SelectedIndex == 8) nazwa_pliku = "kppmiedzyrzecz.txt";
            if (comboBox5.SelectedIndex == 9) nazwa_pliku = "kppnowasol.txt";
            if (comboBox5.SelectedIndex == 10) nazwa_pliku = "kpskwierzyna.txt";
            if (comboBox5.SelectedIndex == 11) nazwa_pliku = "skwierzynajw.txt";
            if (comboBox5.SelectedIndex == 12) nazwa_pliku = "kppslubice.txt";
            if (comboBox5.SelectedIndex == 13) nazwa_pliku = "kppstrzelce.txt";
            if (comboBox5.SelectedIndex == 14) nazwa_pliku = "kpsulechow.txt";
            if (comboBox5.SelectedIndex == 15) nazwa_pliku = "kppsulecin.txt";
            if (comboBox5.SelectedIndex == 16) nazwa_pliku = "kppswiebodzin.txt";
            if (comboBox5.SelectedIndex == 17) nazwa_pliku = "kppwschowa.txt";
            if (comboBox5.SelectedIndex == 18) nazwa_pliku = "kppzagan.txt";
            if (comboBox5.SelectedIndex == 19) nazwa_pliku = "245zagan.txt";
            if (comboBox5.SelectedIndex == 20) nazwa_pliku = "kppzary.txt";
            if (comboBox5.SelectedIndex == 21) nazwa_pliku = "kraj.txt";

            try
            {
                StreamReader sr = new StreamReader(@"C:\Program\Sources\" + nazwa_pliku, Encoding.GetEncoding("ISO-8859-2"));
                string line = sr.ReadLine();
                while (line != null)
                {
                    comboBox6.Items.Add(line);
                    line = sr.ReadLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox6.SelectedIndex = -1;
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// przycisk szukaj list z zakładki marcin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button24_Click(object sender, EventArgs e)
        {
            string select = "SELECT Nazwa, Tworca, Data , Bufor FROM resort WHERE Nazwa like @nazwa AND Bufor NOT LIKE 'archiwum'";
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            cmd.Parameters.Add("@nazwa", MySqlDbType.VarChar, 200).Value = "%" + textBox12.Text + "%";
            MySqlDataReader reader = null;
            ListViewItem lvi = null;
            listView6.Items.Clear();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lvi = new ListViewItem(reader.GetString(0));
                lvi.SubItems.Add(reader.GetString(1));
                lvi.SubItems.Add(reader.GetString(2));
                lvi.SubItems.Add(reader.GetString(3));

                listView6.Items.Add(lvi);
 
                listView6.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView6.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            reader.Close();
            cmd.Dispose();
        }

        /// <summary>
        /// przycisk szukaj list z poczty polsiej z zakladki marcin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button25_Click(object sender, EventArgs e)
        {
            string select = "SELECT Adresat, Jednostka, Data , Tworca FROM poczta_polska WHERE Adresat like @nazwa AND Tworca NOT LIKE 'archiwum'";
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            cmd.Parameters.Add("@nazwa", MySqlDbType.VarChar, 200).Value = "%" + textBox12.Text + "%";
            MySqlDataReader reader = null;
            ListViewItem lvi = null;
            listView6.Items.Clear();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lvi = new ListViewItem(reader.GetString(0));
                lvi.SubItems.Add(reader.GetString(1));
                lvi.SubItems.Add(reader.GetString(2));
                lvi.SubItems.Add(reader.GetString(3));

                listView6.Items.Add(lvi);

                listView6.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView6.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            reader.Close();
            cmd.Dispose();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            string nr_zestawienia = txtbKontrola.Text;
            string data_kontrola = "";
            lblStatus.Text = "";
            zestawienie_kontrola = "";
            // string ;


            string select = "SELECT Tworca, Data FROM poczta_polska WHERE Lp =  @nr_kontrolny";
            Connect_Mysql c = new Connect_Mysql();
            c.connection.Open();
            MySqlCommand cmd = new MySqlCommand(select, c.connection);
            cmd.Parameters.AddWithValue("@nr_kontrolny", nr_zestawienia);
            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                zestawienie_kontrola = reader.GetString(0);
                data_kontrola = reader.GetString(1);
            }//koniec while reader
            reader.Close();
            c.CloseConnection();
            lblStatus.Text = zestawienie_kontrola + " z dnia: " + data_kontrola;
        }

        private void btnWyszukajResort_Click(object sender, EventArgs e)
        {
            string nr_zestawienia = txtbKontrola.Text;
            string data_kontrola = "";
            lblStatus.Text = "";
            zestawienie_kontrola = "";
            // string ;


            string select = "SELECT Bufor, Data FROM resort WHERE Lp =  @nr_kontrolny";
            Connect_Mysql c = new Connect_Mysql();
            c.connection.Open();
            MySqlCommand cmd = new MySqlCommand(select, c.connection);
            cmd.Parameters.AddWithValue("@nr_kontrolny", nr_zestawienia);
            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                zestawienie_kontrola = reader.GetString(0);
                data_kontrola = reader.GetString(1);
            }//koniec while reader
            reader.Close();
            c.CloseConnection();
            lblStatus.Text = zestawienie_kontrola +" z dnia: "+ data_kontrola;

        }


        /// <summary>
        /// przycisk popraw list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button17_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem itemek in listView4.SelectedItems)
            {
                ListViewItem item = listView4.SelectedItems[0];
                int ind_popraw;
                ind_popraw = Int32.Parse(item.SubItems[5].Text);
                Form newform = new Popraw(item.SubItems[1].Text, item.SubItems[4].Text, item.SubItems[3].Text, item.SubItems[2].Text, typ_przesylki, conn, ind_popraw, listView4);
                newform.Location = Start.ActiveForm.Location;
                newform.Show();
            }
        }

        /// <summary>
        /// dla KWP i KMP ZG - przy wyborze adresata ( jednostka, wydział ) mają domyślne numery wykazów
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( comboBox3.SelectedIndex == 21 )
            {
                if ( comboBox4.SelectedIndex == 0 ) textBox5.Text = "5/18";
                if ( comboBox4.SelectedIndex == 1 ) textBox5.Text = "5/3";
                if ( comboBox4.SelectedIndex == 2 ) textBox5.Text = "5/9";
                if ( comboBox4.SelectedIndex == 3 ) textBox5.Text = "5/10";
                if ( comboBox4.SelectedIndex == 4 ) textBox5.Text = "5/8";
                if ( comboBox4.SelectedIndex == 5 ) textBox5.Text = "5/1";
                if ( comboBox4.SelectedIndex == 6 ) textBox5.Text = "5/17";
                if ( comboBox4.SelectedIndex == 7 ) textBox5.Text = "5/14";
                if ( comboBox4.SelectedIndex == 8 ) textBox5.Text = "5/12";
                if ( comboBox4.SelectedIndex == 9 ) textBox5.Text = "5/15";
                if ( comboBox4.SelectedIndex == 10 ) textBox5.Text = "5/2";
                if ( comboBox4.SelectedIndex == 11 ) textBox5.Text = "5/4";
                if ( comboBox4.SelectedIndex == 12 ) textBox5.Text = "5/6";
                if ( comboBox4.SelectedIndex == 13 ) textBox5.Text = "5/7";
                if ( comboBox4.SelectedIndex == 14 ) textBox5.Text = "5/11";
                if ( comboBox4.SelectedIndex == 15 ) textBox5.Text = "5/13";
                if ( comboBox4.SelectedIndex == 16 ) textBox5.Text = "5/16";
            }
            if ( comboBox3.SelectedIndex == 3 )
            {
                if ( comboBox4.SelectedIndex == 0 ) textBox5.Text = "1";
                if ( comboBox4.SelectedIndex == 1 ) textBox5.Text = "2";
                if ( comboBox4.SelectedIndex == 2 ) textBox5.Text = "12";
                if ( comboBox4.SelectedIndex == 3 ) textBox5.Text = "18";
                if ( comboBox4.SelectedIndex == 4 ) textBox5.Text = "11";
                if ( comboBox4.SelectedIndex == 5 ) textBox5.Text = "24";
                if ( comboBox4.SelectedIndex == 6 ) textBox5.Text = "";
                if ( comboBox4.SelectedIndex == 7 ) textBox5.Text = "16";
                if ( comboBox4.SelectedIndex == 8 ) textBox5.Text = "15";
                if ( comboBox4.SelectedIndex == 9 ) textBox5.Text = "10";
                if ( comboBox4.SelectedIndex == 10 ) textBox5.Text = "8";
                if ( comboBox4.SelectedIndex == 11 ) textBox5.Text = "13";
                if ( comboBox4.SelectedIndex == 12 ) textBox5.Text = "9";
                if ( comboBox4.SelectedIndex == 13 ) textBox5.Text = "6";
                if ( comboBox4.SelectedIndex == 14 ) textBox5.Text = "7";
                if ( comboBox4.SelectedIndex == 15 ) textBox5.Text = "4";
                if ( comboBox4.SelectedIndex == 16 ) textBox5.Text = "14";
                if ( comboBox4.SelectedIndex == 17 ) textBox5.Text = "5";
                if ( comboBox4.SelectedIndex == 18 ) textBox5.Text = "23";
                if ( comboBox4.SelectedIndex == 19 ) textBox5.Text = "";
                if ( comboBox4.SelectedIndex == 20 ) textBox5.Text = "3";
                bbb = textBox5.Text;
                aaa = bbb;
            }
            if (comboBox3.SelectedIndex == 4)
            {

                bbb = textBox5.Text;
                aaa = bbb;
            }
        }

        /// <summary>
        /// funkcjonalnosc dla KMP ZG - dopisuje do numeru wykazu oznaczenie dla jawne/niejawne oraz wyswietlanie wybranych listow z zakreu np. wydział/jednostka/wojewodztwo 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        private void button9_Click(object sender, EventArgs e)
        {
            listView3.Items.Clear();
            button13.Enabled = false;
            button14.Enabled = false;
            if (comboBox3.SelectedIndex == 3)
            {
               aaa = bbb;
                if (radioButton3.Checked) aaa = aaa + " J"; else aaa = aaa + " N";
                textBox5.Text = aaa;
            }
                if (comboBox3.SelectedIndex > -1)
                if (comboBox4.SelectedIndex > -1)
                {
                    zeruj_tabelke();
                    wynik = comboBox3.SelectedIndex * 100 + comboBox4.SelectedIndex;
                    tryb_wyswietlania = 1;
                    new Zestawienie_tryb1(conn, listView3, login_uzytkownika, tabelka, typ_zestawienia, rodzaj_zestawienia, wynik);
                }
                else
                {
                    zeruj_tabelke();
                    wynik = comboBox3.SelectedIndex * 100;
                    tryb_wyswietlania = 2;
                    if ( wynik <= 2100 )
                    new Zestawienie_tryb2(conn, listView3, login_uzytkownika, tabelka, typ_zestawienia, rodzaj_zestawienia, wynik);
                    if ( wynik == 2200 )
                    {//zg kraj
                        new Zestawienie_tryb3(conn, listView3, login_uzytkownika, tabelka, typ_zestawienia, rodzaj_zestawienia);
                    }
                    if (wynik == 2300)
                    {//zg kwp
                        new Zestawienie_tryb4(conn, listView3, login_uzytkownika, tabelka, typ_zestawienia, rodzaj_zestawienia);
                    }

                }//koniec else


        }
                
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked) checkBox5.Checked = false;
        }

        /// <summary>
        /// przycisk przyjmij wykaz w zakladce przyjmij wykaz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        private void button6_Click(object sender, EventArgs e)
        {
            czy_przyjeto_wykaz = 0;//przyjeto
            textBox4.Text = "";
            string update = "UPDATE resort SET Bufor = @bufor WHERE Bufor = @buf";
            MySqlCommand cmd = new MySqlCommand(update, conn.connection);
            cmd.Parameters.AddWithValue("@bufor", login_uzytkownika);
            cmd.Parameters.AddWithValue("@buf", zestawienie);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            listView2.Items.Clear();
            MessageBox.Show("Wykaz przyjeto poprawnie");
        }

        private void zamien_num(int numerekk)
        {
            string sel = "SELECT Nazwa FROM adresat WHERE Numer = @numer";
            Connect_Mysql c = new Connect_Mysql();
            c.connection.Open();
            MySqlCommand cmd = new MySqlCommand(sel, c.connection);
            cmd.Parameters.AddWithValue("@numer", numerekk);
            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                adre = reader.GetString(0);
            }//koniec while reader
            reader.Close();
            c.CloseConnection();
        }

        /// <summary>
        /// button popraw list w przyjmij wykaz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem itemek in listView1.SelectedItems)
            {
                string update = "UPDATE resort SET Typ = @typ , Rodzaj = @rodzaj , Nazwa = @nazwa , Cel_numer = @cel_numer , Uwagi = @uwagi , Tworca = @tworca WHERE Lp = @lp";
                MySqlCommand cmd = new MySqlCommand(update, conn.connection);
                if (radioButton1.Checked) { cmd.Parameters.AddWithValue("@typ", 1); typek = "Jawny"; }
                else { cmd.Parameters.AddWithValue("@typ", 2); typek = "Niejawny"; }

                if (checkBox1.Checked) { cmd.Parameters.AddWithValue("@rodzaj", 1); ro = "List"; }
                else { cmd.Parameters.AddWithValue("@rodzaj", 2); ro = "Paczka"; }
                cmd.Parameters.AddWithValue("@tworca", textBox1.Text);
                cmd.Parameters.AddWithValue("@nazwa", textBox2.Text);
                cmd.Parameters.AddWithValue("@uwagi", textBox3.Text);

                if (comboBox2.SelectedIndex == -1)
                    numer = comboBox1.SelectedIndex * 100;
                else
                    numer = comboBox1.SelectedIndex * 100 + comboBox2.SelectedIndex;
                cmd.Parameters.AddWithValue("@cel_numer", numer);
                cmd.Parameters.AddWithValue("@lp", indeks);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                zamien_numer(numer);
                updatelist();
            }
        }

        private void updatelist()
        {
            foreach (ListViewItem itemek in listView1.SelectedItems)
            {
                ListViewItem item = new ListViewItem();
                item = this.listView1.SelectedItems[0];
                item.SubItems[0].Text = textBox2.Text;
                item.SubItems[1].Text = ro;//rodzaj
                item.SubItems[2].Text = adre;//adresat
                item.SubItems[3].Text = typek;//jawny/niejawny
                item.SubItems[4].Text = indeks.ToString();
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }//koniec update list

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem itemek in listView1.SelectedItems)
            { 
            int aaa = 0;
            item = listView1.SelectedItems[0];
            indeks = Int32.Parse(item.SubItems[4].Text);
            string sel = "SELECT Typ, Rodzaj, Nazwa, Cel_numer, Uwagi, Tworca FROM resort WHERE Lp = @lp";
            MySqlCommand cmd = new MySqlCommand(sel, conn.connection);
            cmd.Parameters.AddWithValue("@lp", indeks);
            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetInt32(0) == 1) { radioButton1.Checked = true; if (radioButton1.Checked == true) radioButton2.Checked = false; }
                else { radioButton2.Checked = true; if (radioButton2.Checked == true) radioButton1.Checked = false; }

                if (reader.GetInt32(1) == 1) { checkBox1.Checked = true; if (checkBox1.Checked == true) checkBox2.Checked = false; }
                else { checkBox2.Checked = true; if (checkBox2.Checked == true) checkBox1.Checked = false; }
                aaa = reader.GetInt32(3);
                // zamien_numer(reader.GetInt32(3));
                textBox1.Text = reader.GetString(5);
                textBox2.Text = reader.GetString(2);
                textBox3.Text = reader.GetString(4);
            }
            reader.Close();
            cmd.Dispose();
            zamien_numer(aaa);
        }
        }

        private void zamien_numer(int numerekk)
        {

            string sel = "SELECT Nazwa FROM adresat WHERE Numer = @numer";
            Connect_Mysql c = new Connect_Mysql();
            c.connection.Open();
            MySqlCommand cmd = new MySqlCommand(sel, c.connection);
            cmd.Parameters.AddWithValue("@numer", numerekk);
            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                adre = reader.GetString(0);
            }//koniec while reader
            reader.Close();
            c.CloseConnection();

            string[] dziel = adre.Split(new string[] { " - " }, StringSplitOptions.None);
            adr1 = dziel[0];
            adr2 = dziel[1];
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox1.SelectedItem = -1;
            comboBox2.SelectedItem = -1;
            //comboBox1.SelectedItem = -1;
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

        }

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

        /// <summary>
        /// przycisk dodaj list w zakladce nadaj przesylke
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;

            if (radioButton1.Checked) { typ_przesylki = 1; t_p = "Jawne"; }
            else { typ_przesylki = 2; t_p = "Niejawne"; }

            if (String.IsNullOrEmpty(textBox1.Text))
                MessageBox.Show("Brak nadawcy listu");
            else
                if (String.IsNullOrEmpty(textBox2.Text))
                MessageBox.Show("Brak numeru listu");
            else
            {
                insert = "INSERT INTO resort(Typ,Rodzaj,Nazwa,Uwagi,Cel_numer,Tworca,Bufor,Data) VALUES (@typ,@rodzaj,@nazwa,@uwagi,@cel_numer,@tworca,@bufor,@data)";
                MySqlCommand cmd = new MySqlCommand(insert, conn.connection);
                cmd.Parameters.AddWithValue("@typ", typ_przesylki);
                cmd.Parameters.AddWithValue("@rodzaj", rodzaj_przesylki);
                cmd.Parameters.AddWithValue("@nazwa", textBox2.Text);
                cmd.Parameters.AddWithValue("@uwagi", textBox3.Text);
               
                if ( comboBox2.SelectedIndex == -1 )
                numer = comboBox1.SelectedIndex * 100 ;
                else numer = comboBox1.SelectedIndex * 100 + comboBox2.SelectedIndex;
                cmd.Parameters.AddWithValue("@cel_numer", numer);
                cmd.Parameters.AddWithValue("@tworca", textBox1.Text);
                cmd.Parameters.AddWithValue("@bufor", login_uzytkownika);
                cmd.Parameters.AddWithValue("@data", DateTime.Now.ToShortDateString());
                cmd.ExecuteNonQuery();
                numer_mysql = cmd.LastInsertedId;
                cmd.Dispose();
                dodajDoListy();
                checkBox1.Checked = true;
                if (checkBox1.Checked) checkBox2.Checked = false;
                radioButton1.Checked = true;
                if (radioButton1.Checked) radioButton2.Checked = false;
                textBox2.Text = "";
                textBox3.Text = "";
            }//koniec els-a

        }

        private void dodajDoListy()
        {
            ListViewItem lvi = null;
            lvi = new ListViewItem(textBox2.Text);
            lvi.SubItems.Add(r_p);//rodzaj
            if ( comboBox1.Text.Equals(comboBox2.Text))
            lvi.SubItems.Add(comboBox1.Text);
            else
                lvi.SubItems.Add(comboBox1.Text + " - " + comboBox2.Text);
            lvi.SubItems.Add(t_p);
            lvi.SubItems.Add(numer_mysql.ToString());
            if (typ_przesylki == 2) lvi.BackColor = Color.Red; //kolorowanie listow niejawnych w listview
            listView1.Items.Add(lvi);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void PocztaSpecjalna_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            radioButton2.Checked = false;
            checkBox1.Checked = true;
            checkBox2.Checked = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button13.Enabled = false;
            button15.Enabled = false;
            button16.Enabled = false;
            button17.Enabled = false;
            button20.Enabled = false;
            button21.Enabled = false;
            typ_przesylki = 1;//jawne
            t_p = "Jawne";
            rodzaj_przesylki = 1;
            r_p = "List";
        }
    }
}
