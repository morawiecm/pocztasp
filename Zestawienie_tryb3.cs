using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SIPS
{
    class Zestawienie_tryb3
    {
        Connect_Mysql conn;
        ListView lv;
        string login_uzytkownika;
        int[] tabelka;
        int typ_zestawienia, rodzaj_zestawienia;
        string select = null;

        public Zestawienie_tryb3(Connect_Mysql con, ListView lv3, string lu, int[] tab, int tz, int rz)
        {
            this.conn = con;
            this.lv = lv3;
            this.login_uzytkownika = lu;
            this.tabelka = tab;
            this.typ_zestawienia = tz;
            this.rodzaj_zestawienia = rz;

            if (typ_zestawienia == 1)
                if (rodzaj_zestawienia == 1)
                    select = "SELECT Cel_numer, Nazwa, Rodzaj, LP FROM resort WHERE Bufor = @bufor AND Typ = 1";
                else select = "SELECT Cel_numer, Nazwa, Rodzaj, LP FROM resort WHERE Bufor = @bufor AND Typ = 1 AND Rodzaj = 2";
            else
                 if (rodzaj_zestawienia == 1)
                select = "SELECT Cel_numer, Nazwa, Rodzaj, LP FROM resort WHERE Bufor = @bufor AND Typ = 2";
            else select = "SELECT Cel_numer, Nazwa, Rodzaj, LP FROM resort WHERE Bufor = @bufor AND Typ = 2 AND Rodzaj = 2";

            wyswietl();
        }//koniec public

        private void wyswietl()
        {
            lv.Items.Clear();
            ListViewItem lvi = null;
            int lp = 1;
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            cmd.Parameters.AddWithValue("@bufor", login_uzytkownika);
            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               // Console.WriteLine("ok");
                if ( reader.GetInt32(0) >= 2100 )
                    if (reader.GetInt32(0) <= 2120)
                    {
                       // Console.WriteLine("ok");
                        lvi = new ListViewItem(lp.ToString());
                        lvi.SubItems.Add(reader.GetString(1));
                        if (reader.GetInt32(2) == 1) lvi.SubItems.Add("List");
                        else lvi.SubItems.Add("Paczka");
                        lvi.SubItems.Add(reader.GetInt32(3).ToString());
                        tabelka[lp - 1] = reader.GetInt32(3);
                        lvi.Checked = true;
                        lv.Items.Add(lvi);
                        lp++;
                    }
                lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }//koniec reader
            reader.Close();
            cmd.Dispose();

        }//koniec wyswietl
    }
}
