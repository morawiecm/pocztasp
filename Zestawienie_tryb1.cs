using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SIPS
{
    class Zestawienie_tryb1
    {
        Connect_Mysql conn;
        ListView lv;
        string login_uzytkownika;
        int[] tabelka;
        int typ_zestawienia, rodzaj_zestawienia, wynik;
        string select = null;

        /// <summary>
        /// drukowanie wydzialow z kwp gorzow lub kmp zg
        /// </summary>
        /// <param name="con">polaczenie mysql</param>
        /// <param name="lv3">listview z zakładki Zestawienia</param>
        /// <param name="lu">login uzytkownika</param>
        /// <param name="tab">tablica indexow</param>
        /// <param name="tz">1 janwe , 2 - niejawne</param>
        /// <param name="rz">1 list, 2 paczka</param>
        /// <param name="wyni">numer konkretnego wydziału lub jednostki lub grupy typu cały kraj</param>

        public Zestawienie_tryb1(Connect_Mysql con, ListView lv3, string lu, int[] tab, int tz, int rz, int wyni)
        {
            this.conn = con;
            this.lv = lv3;
            this.login_uzytkownika = lu;
            this.tabelka = tab;
            this.typ_zestawienia = tz;
            this.rodzaj_zestawienia = rz;
            this.wynik = wyni;

            if ( typ_zestawienia == 1 )
                if ( rodzaj_zestawienia == 1 )
                select = "SELECT Nazwa, Rodzaj, LP FROM resort WHERE Bufor = @bufor AND Cel_numer = @cel_numer AND Typ = 1";
                else select = "SELECT Nazwa, Rodzaj, LP FROM resort WHERE Bufor = @bufor AND Cel_numer = @cel_numer AND Typ = 1 AND Rodzaj = 2";
            else
                if (rodzaj_zestawienia == 1)
                select = "SELECT Nazwa, Rodzaj, LP FROM resort WHERE Bufor = @bufor AND Cel_numer = @cel_numer AND Typ = 2";
                else select = "SELECT Nazwa, Rodzaj, LP FROM resort WHERE Bufor = @bufor AND Cel_numer = @cel_numer AND Typ = 2 AND Rodzaj = 2";

            wyswietl();
        }

        public void wyswietl()
        {
            lv.Items.Clear();
            ListViewItem lvi = null;
            int lp = 1;
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            cmd.Parameters.AddWithValue("@bufor", login_uzytkownika);
            cmd.Parameters.AddWithValue("@cel_numer", wynik);
            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lvi = new ListViewItem(lp.ToString());
                lvi.SubItems.Add(reader.GetString(0));
                if ( reader.GetInt32(1) == 1 ) lvi.SubItems.Add("List");
                else lvi.SubItems.Add("Paczka");

                lvi.SubItems.Add(reader.GetInt32(2).ToString());
                tabelka[lp - 1] = reader.GetInt32(2);
                lvi.Checked = true;
                lv.Items.Add(lvi);
                lp++;
                lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }//koniec reader
            reader.Close();
            cmd.Dispose();
        }//koniec wyswietl

    }
}
