using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SIPS
{
    public partial class DodajPP : Form
    {
        string nazwa_pliku,adresat,adr1,adr2;
        string login_uzytkownika,jednostka_uzytkownika,nazwa_uzytkownika;
        string nazwa_listu,insert,rodzajL,waga;
        int ow, cw, ra,wynik,lp;

        private void DodajPP_Load(object sender, EventArgs e)
        {

        }

        float kwota;
        MySqlDataReader reader = null;
        Connect_Mysql conn;
        ListView lvi;

        public DodajPP(ListView lv, string nu, string lu, string ju, Connect_Mysql con, string nazwaL,int a,int b,int c,string rodzL, int wyn, string wag)
        {
            this.lvi = lv;
            this.nazwa_uzytkownika = nu;
            this.login_uzytkownika = lu;
            this.jednostka_uzytkownika = ju;
            this.nazwa_listu = nazwaL;
            this.conn = con;
            this.ow = a;
            this.cw = b;
            this.ra = c;
            this.rodzajL = rodzL;
            this.wynik = wyn;
            this.waga = wag;
            if (ow == 1) rodzajL = rodzajL + " - KPK";

            InitializeComponent();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            adresat = comboBox2.Text;

            string[] dziel = adresat.Split(new string[] { " - " }, StringSplitOptions.None);
            adr1 = dziel[0];
            adr2 = dziel[1];
            textBox1.Text = adr1;
            textBox2.Text = adr2;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            insert = "INSERT INTO poczta_polska(Rodzaj,Rodzaj_numer,Numer_Sprawy,Adresat,Adresat_Adres,Waga,OW,CW,RA,Data,Jednostka,Tworca,Kwota) VALUES (@rodzaj,@rodzaj_numer,@numer_sprawy,@adresat,@adresat_adres,@waga,@ow,@cw,@ra,@data,@jednostka,@tworca,@kwota)";
            MySqlCommand cmd = new MySqlCommand(insert, conn.connection);
            cmd.Parameters.AddWithValue("@rodzaj", rodzajL);
            cmd.Parameters.AddWithValue("@rodzaj_numer", wynik);
            cmd.Parameters.AddWithValue("@numer_sprawy", nazwa_listu);
            cmd.Parameters.AddWithValue("@adresat", textBox1.Text);
            cmd.Parameters.AddWithValue("@adresat_adres", textBox2.Text);
            cmd.Parameters.AddWithValue("@waga", waga);
            cmd.Parameters.AddWithValue("@ow", ow);
            cmd.Parameters.AddWithValue("@cw", cw);
            cmd.Parameters.AddWithValue("@ra", ra);
            cmd.Parameters.AddWithValue("@data", DateTime.Now.ToShortDateString());
            cmd.Parameters.AddWithValue("@jednostka", jednostka_uzytkownika);
            cmd.Parameters.AddWithValue("@tworca", login_uzytkownika);
            sprawdz_kwote();
            cmd.Parameters.AddWithValue("@kwota", kwota);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            wypelnij_liste();
            textBox1.Text = "";
            textBox2.Text = "";
            Start.ActiveForm.Close();
        }

        private void wypelnij_liste()
        {
            lvi.Items.Clear();
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
                lvi.Items.Add(list);
                lvi.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                lvi.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                lp++;
            }
            reader.Close();
        }
        
        private void sprawdz_kwote()
        {
            string select = "SELECT Kwota FROM cennik WHERE Lp = @lp";
            MySqlCommand sel = new MySqlCommand(select, conn.connection);
            sel.Parameters.AddWithValue("@lp", wynik);
            reader = sel.ExecuteReader();
            while (reader.Read())
            {
                kwota = reader.GetFloat(0);
            }
            reader.Close();
        }

  
        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            try
            {
                StreamReader sr = new StreamReader(@"C:\Program\Sources\urzedypp.txt", Encoding.GetEncoding("ISO-8859-2"));
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
            if (comboBox1.SelectedIndex == 1) nazwa_pliku = "urzedyppzg1.txt";
            if (comboBox1.SelectedIndex == 2) nazwa_pliku = "urzedyppzg2.txt";

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
    }
}
