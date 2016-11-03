using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace SIPS
{
    public class Logowanie
    {
        string login_uzytkownika, haslo_uzytkownika, jednostka_uzytkownika;
        int id_usera,haslo_dni;
        Label labelek;
        Connect_Mysql conn;
        MySqlDataReader reader = null;
        string nazwa_uzytkownika, numer_wykazu;
        int typ_usera;

        /// <summary>
        /// konstruktor klasy logowanie
        /// </summary>
        /// <param name="login">text pobrany z textbox1</param>
        /// <param name="haslo">text pobrany z textbox2</param>
        /// <param name="con">przekazanie połączenia MySql</param>
        /// <param name="label3">Etykieta label3 wyswietlająca błedy logowania lub braku połączenia z bazą MySql</param>

        public Logowanie(string login , string haslo , Connect_Mysql con , Label label3 )
        {
            this.login_uzytkownika = login;
            this.haslo_uzytkownika = haslo;
            this.labelek = label3;
            this.conn = con;
            int spr = 0;
            int p = 0;
            string select = "SELECT Haslo,Nazwisko,Jednostka,Typ_usera,Wykaz FROM users WHERE Login = @login ";
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            cmd.Parameters.AddWithValue("@login", login_uzytkownika);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {

                if (haslo_uzytkownika.Equals(reader.GetString(0)))
                {
                    spr = 1;//warunek do uruchomienia programu
                    p = 1; // jezeli wystapil błąd logowania to p ni ejest zmieniane. Domyślnie p=0
                    nazwa_uzytkownika = reader.GetString(1);
                    jednostka_uzytkownika = reader.GetString(2);
                    typ_usera = reader.GetInt16(3); //1 - user standardowy - dostep do resortu i poczty polskiej , 0 - user z pelnym dostepem
                    numer_wykazu = reader.GetString(4);//generowane przez userow
                }
                else labelek.Text = "Błędny login lub hasło";
            }
            reader.Close();

            if ( p == 0) labelek.Text = "Błędny login lub hasło";

            if ( spr == 1)
            {
                spr = 0;
                p = 0;
                Form newformstart = new MenuGlowne(login_uzytkownika, conn, nazwa_uzytkownika, jednostka_uzytkownika,typ_usera, numer_wykazu,id_usera,haslo_dni);
                newformstart.Location = Start.ActiveForm.Location;
                Start.ActiveForm.Hide();
                newformstart.Show();
            }

        }
    }




}
