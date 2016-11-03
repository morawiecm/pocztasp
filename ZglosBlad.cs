using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SIPS
{
    public partial class ZglosBlad : Form
    {
        Connect_Mysql polaczenie_z_baza;
        int id_uzytkownika;
        int id_wydzialu;
        int id_jednostki;

        /// <summary>
        /// Tworzenie nowej Formy do zgłaszania Błędów 
        /// </summary>
        /// <param name="conn">polaczenie</param>
        /// <param name="id_user">id_uzytkownika</param>
        public ZglosBlad(Connect_Mysql conn, int id_user)
        {
            this.polaczenie_z_baza = conn;
            this.id_uzytkownika = id_user;
            InitializeComponent();
        }

        private void btnZamknij_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnWyslij_Click(object sender, EventArgs e)
        {
            //zmienne
            string zgloszenie_temat;
            string zgloszenie_tresc;
            DateTime data_zgloszenia;

            //przypisanie do zmiennych wartosci

            zgloszenie_temat = txtbTemat.Text;
            zgloszenie_tresc = rtxbZgloszenie.Text;

            //sprawdzenie czy nie sa puste
            bool sprawdzTemat = SprawdzCzyPuste(zgloszenie_temat);
            bool sprawdzTresc = SprawdzCzyPuste(zgloszenie_tresc);

            if (sprawdzTemat == true && sprawdzTresc == true)
            {
                //jezeli pola nie sa puste zapisz do bazy
                try
                {
                    //TODO:  napisać mechanizm zapisu zgloszenia do bazy
                    data_zgloszenia = DateTime.Now;
                    string zapytanie_dodaj_funkcje = "INSERT INTO bledy ( tytul, opis, uzytkownik_id, data_utworzenia) "
                        + " VALUES (@d_temat, @d_tresc, @d_id_user, @d_data)";
                    MySqlCommand wykonajZapytanie1 = new MySqlCommand(zapytanie_dodaj_funkcje, polaczenie_z_baza.connection);
                    wykonajZapytanie1.Parameters.AddWithValue("@d_temat", zgloszenie_temat);
                    wykonajZapytanie1.Parameters.AddWithValue("@d_tresc", zgloszenie_tresc);
                    wykonajZapytanie1.Parameters.AddWithValue("@d_id_user", id_uzytkownika);
                    wykonajZapytanie1.Parameters.AddWithValue("@d_data", data_zgloszenia);
                    wykonajZapytanie1.ExecuteNonQuery();
                    int nr_rekordu_dodanego = 0;
                    nr_rekordu_dodanego = Convert.ToInt32(wykonajZapytanie1.LastInsertedId);
                    wykonajZapytanie1.Parameters.Clear();
                    wykonajZapytanie1.Dispose();

                    //po pomyślnym dodaniu czyszczenie textboxow i blokowanie przcisku wyślij
                    MessageBox.Show("Zgłoszenie zostało zapisane. Nr zgłoszenia: " + nr_rekordu_dodanego.ToString());
                    txtbTemat.Clear();
                    rtxbZgloszenie.Clear();
                    btnWyslij.Enabled = false;
                }
                catch (Exception blad)
                {
                    MessageBox.Show(blad.Message);
                }

            }
            else
            {
                MessageBox.Show("Pola nie zostały wypełnione poprawnie");
            }
        }
        /// <summary>
        /// Funkcja sprawdzająca czy dany tekst nie jest pusty
        /// </summary>
        /// <param name="tekstDoSprawdzenia">Tekst Wejsciowy</param>
        /// <returns></returns>
        public bool SprawdzCzyPuste(string tekstDoSprawdzenia)
        {
            tekstDoSprawdzenia = tekstDoSprawdzenia.Trim();
            bool pusty = false;
            if (tekstDoSprawdzenia != "")
            {
                pusty = true;
            }
            return pusty;
        }
    }
}
