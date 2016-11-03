using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace SIPS
{
    public partial class Resort : Form
    {
        string login_uzytkownika;
        string nazwa_uzytkownika;
        string jednostka_uzytkownika, numer_wykazu;
        string adresat = "";
        int rodzaj_listu;
        Form lastone;
        Connect_Mysql conn;
        MySqlDataReader reader = null;
        int lp,wysw,id_uzytkownika;

        /// <summary>
        /// KOnstruktor klasy resort
        /// Moduł do wpisywania poczty resortowej jawnej i niejawnej
        /// </summary>
        /// <param name="lu">login uztkownika</param>
        /// <param name="con">polaczeni MySql</param>
        /// <param name="nu">Imie i Nazwisko</param>
        /// <param name="ju">Jednostka uzytkownika wraz z wydziałem</param>
        /// <param name="last">forma poprzednia umozliwiajaca poruszanie sie po roznych modułach aplikacji ( resort, poczta polska, poczta specjalna, serwis )</param>
        /// <param name="id_lp">Nr id w bazie uzytkownika</param>

        public Resort(string lu , Connect_Mysql con , string nu , string ju , Form last , int id_lp)
        {
            this.login_uzytkownika = lu;
            this.conn = con;
            this.nazwa_uzytkownika = nu;
            this.jednostka_uzytkownika = ju;
            this.lastone = last;
            this.id_uzytkownika = id_lp;
            //this.numer_wykazu = nw;
            numer_wykazu = PobierzOstatniNrWykazu(id_uzytkownika);
            InitializeComponent();
            label2.Text = nazwa_uzytkownika;
            wyswietl();
            //this.grupa = gr;
        }
        private const int CP_NOCLOSE_BUTTON = 0x200;//linijka od 42 do 51 - nieaktywny przycisk X do wyłaczenia programu
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }


        public string PobierzOstatniNrWykazu(int id_user)
        {
            Connect_Mysql polacz = new Connect_Mysql();
            polacz.connection.Open();
            string nr_wykazu ="";

            string zapytanie_pobierz_ostatni_nr = "SELECT Wykaz FROM users WHERE Lp = @id_uzytkownikaa ";
            MySqlCommand cmd = new MySqlCommand(zapytanie_pobierz_ostatni_nr, polacz.connection);
            cmd.Parameters.AddWithValue("@id_uzytkownikaa", id_user);

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                nr_wykazu = reader.GetString(0);
            }
            reader.Close();
            polacz.connection.Close();


            return nr_wykazu;
        }


        /// <summary>
        /// metoda wyswietlajaca obecnie wpisane listy jeszcze ni ewydrukowane przez usera
        /// </summary>

        public void wyswietl()
        {
            Connect_Mysql polacz = new Connect_Mysql();
            polacz.connection.Open();
            listView1.Items.Clear();
            string select = null;
            ListViewItem lvi = null;
            string nazwa, uwagi,list_paczka ;
            int rodzaj, cel_numer,index;
            lp = 1;
            //po uruchomieniu resortu wyswietla domyslnie wpisane ale nie wysłane listy
            //roadiobutto1 - listy jawne, radiobutton2 - listy niejawne
            //bufor - wlasciciel listu
            if (radioButton1.Checked) select = "SELECT Lp , Rodzaj , Nazwa , Uwagi , Cel_numer FROM resort WHERE Typ = 1 AND Bufor = @bufor";
            if (radioButton2.Checked) select = "SELECT Lp , Rodzaj , Nazwa , Uwagi , Cel_numer FROM resort WHERE Typ = 2 AND Bufor = @bufor";

            MySqlCommand cmd = new MySqlCommand(select, polacz.connection);
            cmd.Parameters.AddWithValue("@bufor", login_uzytkownika);

            reader = cmd.ExecuteReader();
            while (reader.Read())
                {
                index = reader.GetInt32(0);//index z tabeli mysql ( ID - Lp ) 
                rodzaj = reader.GetInt16(1); // jawny/niejawny 1/0
                nazwa  = reader.GetString(2);//nazwa listu wprowadzona przez usera
                uwagi  = reader.GetString(3);//uwagi do listu wprowadzone przez usera
                cel_numer = reader.GetInt16(4);//id pobrane z tabeli adresata ( id-numer )
                if (rodzaj == 1) list_paczka = "List"; else list_paczka = "Paczka";
                lvi = new ListViewItem(lp.ToString());//tworzenie wiersza listview
                lvi.SubItems.Add(nazwa);
                lvi.SubItems.Add(list_paczka);
                zamien_numer(cel_numer);//metoda zamieniajaca numer celu listu na jego pelna nazwe ( tabel aadresat )
                lvi.SubItems.Add(adresat);
                lvi.SubItems.Add(uwagi);
                lvi.SubItems.Add(index.ToString());
                listView1.Items.Add(lvi);

                listView1.Columns[2].TextAlign = HorizontalAlignment.Center;
                listView1.Columns[5].TextAlign = HorizontalAlignment.Right;

                lp++;
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            }
            reader.Close();
            polacz.connection.Close();


        }//koniec wyswietl

        /// <summary>
        /// metoda zamieniajaca numer celu listu na jego pelna nazwe ( tabela adresat )
        /// </summary>
        /// <param name="numerek">int z tabeli resort z kolumny cel_numer</param>

        private void zamien_numer(int numerek)
        {
            string sel = "SELECT Nazwa FROM adresat WHERE Numer = @numer";
           // Connect_Mysql
            Connect_Mysql c = new Connect_Mysql();
            c.connection.Open();
            MySqlCommand cm = new MySqlCommand(sel, c.connection);
            cm.Parameters.AddWithValue("@numer", numerek);
            MySqlDataReader r = null;
            r = cm.ExecuteReader();
            while (r.Read())
            {
                adresat = r.GetString(0);
            }//koniec while reader
            r.Close();
            c.CloseConnection();

        }

        //przycisk zamykajacy forme resort i otwierajacy forme MenuGlowne
        private void button1_Click(object sender, EventArgs e)
        {
            Start.ActiveForm.Hide();
            lastone.Show();
        }
        //zabezpieczenia przed blednymi wyborami uzytkownikow ( popraw niewybrany list - bedzie aktywny kiedy wybierzemy rekord )
        private void Resort_Load(object sender, EventArgs e)
        {
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            textBox1.Text = numer_wykazu;
            wysw = 1;
            rodzaj_listu = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {   //button wyswietl
            button4.Enabled = false;//usun
            button5.Enabled = false;//popraw
            button6.Enabled = true;//utworz wykaz
            wysw = 1;
            listView1.Items.Clear();
            wyswietl();
        }

        private void button3_Click(object sender, EventArgs e)
        {//przycisk dodaj
            wysw = 0;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            Form newform = new Dodaj(conn , login_uzytkownika , nazwa_uzytkownika , listView1 , lp , jednostka_uzytkownika);
            newform.Location = Start.ActiveForm.Location;
            newform.Show();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (wysw == 1)
            {
                button4.Enabled = true;
                button5.Enabled = true;
            }

                button6.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            rodzaj_listu = 2;//list niejawny
            wysw = 0;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            rodzaj_listu = 1;//list jawny
            wysw = 0;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {   //Button usun
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            string numer;
            ListViewItem item = listView1.SelectedItems[0];
            numer = item.SubItems[5].Text;
            string delete = "DELETE FROM resort WHERE Lp = @lp";
            MySqlCommand cmd = new MySqlCommand(delete, conn.connection);
            cmd.Parameters.AddWithValue("@lp", Int32.Parse(numer));
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.ExecuteNonQuery();

            foreach (ListViewItem lv in listView1.SelectedItems)
                lv.Remove();
            cmd.Dispose();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            wysw = 0;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            Form newform = new DodajObcy(conn, login_uzytkownika, nazwa_uzytkownika, listView1, lp, jednostka_uzytkownika);
            newform.Location = Start.ActiveForm.Location;
            newform.Show();
        }

        /// <summary>
        /// Przycisk utworz wykaz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button6_Click(object sender, EventArgs e) 
        {
            if (String.IsNullOrEmpty(textBox1.Text))
                MessageBox.Show("Brak numeru Wykazu", "Przypominacz", MessageBoxButtons.OK, MessageBoxIcon.Question);
            else
            if ( lp > 1) //warunek jezeli jest minimum 1 list to tworz wykaz , jezeli lista jest pusta info -> brak listow do wydruku
            {
                int wybor;
                if (radioButton1.Checked) wybor = 1;
                else wybor = 2;
                /*
                string update = "UPDATE users SET Wykaz = @wykaz WHERE Grupa = @login";
                MySqlCommand cmd = new MySqlCommand(update, conn.connection);
                cmd.Parameters.AddWithValue("@login", login_uzytkownika);
                cmd.Parameters.AddWithValue("@wykaz", textBox1.Text);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                */
                zapisz_wykaz(textBox1.Text);
                new WykazPDF(conn, login_uzytkownika, nazwa_uzytkownika, wybor, jednostka_uzytkownika, textBox1.Text);
                listView1.Items.Clear();
                button6.Enabled = false;
            }
            else
                MessageBox.Show("Brak Listów do Wydruku");
        }

        /// <summary>
        /// metoda zapisujaca do tabeli users ostatni wykaz podany przez uzytkownika
        /// </summary>
        /// <param name="wy">text z textbox1</param>

        private void zapisz_wykaz(string wy)
        {
            string wyk = wy;
            string update = "UPDATE users SET Wykaz = @wykaz WHERE Grupa = @login";
            MySqlCommand cmd = new MySqlCommand(update, conn.connection);
            cmd.Parameters.AddWithValue("@wykaz", wyk);
            cmd.Parameters.AddWithValue("@login", login_uzytkownika);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"c:\Program\" + login_uzytkownika + ".pdf");
            }
            catch
            {
                MessageBox.Show("Brak utworzonego pliku, lub nic nie zostało wygenerowane");
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {   //buton popraw
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            int index;
            string nazwa, uwagi,adresat,list_paczka;
            ListViewItem item = listView1.SelectedItems[0];
            index = Int32.Parse(item.SubItems[5].Text);
            nazwa = item.SubItems[1].Text;
            uwagi = item.SubItems[4].Text;
            adresat = item.SubItems[3].Text;
            list_paczka = item.SubItems[2].Text;

            Form newform = new Popraw(nazwa,uwagi,adresat,list_paczka, rodzaj_listu,conn,index,listView1);
            newform.Location = Start.ActiveForm.Location;
            newform.Show();
            
        }
    }

}
