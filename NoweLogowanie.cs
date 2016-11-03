using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Net;

namespace SIPS
{
    public class NoweLogowanie
    {
        string login_uzytkownika, haslo_uzytkownika, jednostka_uzytkownika;
        Label labelek, lab2;
        Connect_Mysql conn;
        MySqlDataReader reader = null;
        MySqlDataReader reader1 = null;
        string nazwa_uzytkownika, numer_wykazu,adres_ip;
        private string grupa;
        int typ_usera, blokada, ilosc_blednych_logowan, bledne_logowania_baza,wniosek, wersja_programu;
        private DateTime data_zmiany_hasla, data_dzisiaj, data_blokady;
        private bool lFailed = true;
        private Form oldform;
        int id_uzytkownika, haslo_wygasnie_dni =0;

        
        /// <summary>
        /// konstruktor klasy logowanie
        /// </summary>
        /// <param name="login">text pobrany z textbox1</param>
        /// <param name="haslo">text pobrany z textbox2</param>
        /// <param name="con">przekazanie połączenia MySql</param>
        /// <param name="label3">Etykieta label3 wyswietlająca błedy logowania lub braku połączenia z bazą MySql</param>
        public NoweLogowanie(string login, string haslo, Connect_Mysql con, Label label3, Label label4, Form of, string ip_a,int pro_versja)
        {
            
            login_uzytkownika = login;
            haslo_uzytkownika = haslo;
            adres_ip = ip_a;
            wersja_programu = pro_versja;
            labelek = label3;
            lab2 = label4;
            conn = con;
            oldform = of;
            string select =
                "SELECT Nazwisko, Jednostka, Typ_usera, Wykaz, Grupa, Blokada, Data_zmiany_hasla, Ilosc_blednych_logowan, Lp, wniosek FROM users WHERE Login = @login AND Haslo = @haslo";

            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            cmd.Parameters.AddWithValue("@login", login_uzytkownika);
            cmd.Parameters.AddWithValue("@haslo", haslo_uzytkownika);

            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (SqlException oError)
            {
                lFailed = false;
                MessageBox.Show(oError.ToString());
            }

            if (lFailed)
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nazwa_uzytkownika = reader.GetString(0);
                        jednostka_uzytkownika = reader.GetString(1);
                        typ_usera = reader.GetInt16(2);
                        numer_wykazu = reader.GetString(3);
                        grupa = reader.GetString(4);
                        blokada = reader.GetInt16(5);
                        data_zmiany_hasla = reader.GetDateTime(6);
                        ilosc_blednych_logowan = reader.GetInt16(7);
                        id_uzytkownika = reader.GetInt32(8);
                        wniosek = reader.GetInt32(9);
                    }
                    reader.Close();
                    SprawdzenieLogowania();
                }
                else
                {
                    reader.Close();
                    lFailed = false;
                }
            }

            if (!lFailed)
            {
                select = "SELECT Ilosc_blednych_logowan FROM users WHERE Login = @login";
                cmd = new MySqlCommand(select, conn.connection);
                cmd.Parameters.AddWithValue("@login", login_uzytkownika);
                try
                {
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ilosc_blednych_logowan = reader.GetInt16(0);
                        }
                        reader.Close();
                        DodajBledneLogowanie();
                    }
                    else
                    {
                        reader.Close();
                        labelek.Text = "Błędny login lub hasło. Przy pierwszym logowaniu LOGIN i HASŁO to IDENTYFIKATOR PRACOWNIKA";
                        lab2.Text = "Przy pierwszym logowaniu LOGIN i HASŁO to IDENTYFIKATOR PRACOWNIKA";
                    }
                }
                catch (SqlException oError)
                {
                    MessageBox.Show(oError.ToString());
                }
            }

        }//koniec public logowanie


       

        private void SprawdzenieLogowania()
        {
            if (blokada == 0)
            {
                if ((DateTime.Now.Date - data_zmiany_hasla).Days >= 30)
                {
                    zmienhaslo();

                }
                else
                {
                    //sprawdzenie czy wplynal wniosek 
                   if(wniosek == 1)
                    {
                        haslo_wygasnie_dni = 30 - (DateTime.Now.Date - data_zmiany_hasla).Days;
                        zaloguj();
                    }
                   //jezlei brak wniosku nie loguj
                    else
                    {
                        data_dzisiaj = DateTime.Today;
                        //data zablokowania kont bez wniosku
                        data_blokady = new DateTime(2016, 11, 1);

                        //porownaj date blokady z data dzisiejsza
                        if (data_dzisiaj >= data_blokady)
                        {
                            MessageBox.Show("Brak wniosku o nadanie uprawnień. Konto zostanie odblokowane w moemencie wpłynięcia wniosku o nadanie uprawień");
                            labelek.Text = "Brak wniosku, logowanie niemożliwe";
                        }
                        //jezeli data jest mniejsza niz data blokady dopusc uzytkownika
                        else
                        {
                            haslo_wygasnie_dni = 30 - (DateTime.Now.Date - data_zmiany_hasla).Days;
                            zaloguj();
                        }
                    }
                   
                }

            }
            else labelek.Text = "Konto zablokowane";
        }

        private void zmienhaslo()
        {
            Form newformstart = new ZmianaHasla(conn, login_uzytkownika, grupa, nazwa_uzytkownika, jednostka_uzytkownika, typ_usera, numer_wykazu, oldform,id_uzytkownika,haslo_wygasnie_dni);
            newformstart.Location = Start.ActiveForm.Location;
            newformstart.Show();
        }

        private void pobierz()
        {
            try
            {
                FileStream plik = new FileStream(@"C:\Program\Konfiguracja\konfiguracja.txt", FileMode.Open);
                StreamReader odczyt = new StreamReader(plik);
                string wiersz = odczyt.ReadLine();

                string[] dziel = wiersz.Split(new string[] { " - " }, StringSplitOptions.None);
                wersja_programu = Int16.Parse(dziel[0]);

                odczyt.Close();
                plik.Close();
            }
            catch
            {
                MessageBox.Show("Blad odczytu pliku koniguracja.txt. Nalezy zgłosić Informatykowi");
                //ActiveForm.Close();

            }

        }

        private void zaloguj()
        {
            string data = DateTime.Now.ToString();
            string update = "UPDATE users SET Ilosc_blednych_logowan = @ilos, data_logowania = @data_log, ip_adres = @ip_adr, wersja = @version WHERE Login = @login";
            MySqlCommand cmdupdate = new MySqlCommand(update, conn.connection);
            cmdupdate.Parameters.AddWithValue("@ilos", 0);
            cmdupdate.Parameters.AddWithValue("@login", login_uzytkownika);
            cmdupdate.Parameters.AddWithValue("@data_log", data);
            cmdupdate.Parameters.AddWithValue("@ip_adr", adres_ip);
            cmdupdate.Parameters.AddWithValue("@version", wersja_programu);
            cmdupdate.ExecuteNonQuery();
            cmdupdate.Dispose();

            Form newformstart = new MenuGlowne(grupa, conn, nazwa_uzytkownika, jednostka_uzytkownika, typ_usera, numer_wykazu,id_uzytkownika,haslo_wygasnie_dni);
            newformstart.Location = Start.ActiveForm.Location;
            Start.ActiveForm.Hide();
            newformstart.Show();
        }

        private void DodajBledneLogowanie()
        {
            if (ilosc_blednych_logowan >= 2)
            {
                string update = "UPDATE users SET Blokada = @ilos WHERE Login = @login";
                MySqlCommand cmdupdate = new MySqlCommand(update, conn.connection);
                ilosc_blednych_logowan++;
                cmdupdate.Parameters.AddWithValue("@ilos", 1);
                cmdupdate.Parameters.AddWithValue("@login", login_uzytkownika);
                cmdupdate.ExecuteNonQuery();
                cmdupdate.Dispose();
                labelek.Text = "Konto zablokowane";
            }
            else
            {
                string update = "UPDATE users SET Ilosc_blednych_logowan = @ilos WHERE Login = @login";
                MySqlCommand cmdupdate = new MySqlCommand(update, conn.connection);
                ilosc_blednych_logowan++;
                cmdupdate.Parameters.AddWithValue("@ilos", ilosc_blednych_logowan);
                cmdupdate.Parameters.AddWithValue("@login", login_uzytkownika);
                cmdupdate.ExecuteNonQuery();
                cmdupdate.Dispose();
                labelek.Text = "Błędny login lub hasło";
            }
        }
    }
}
