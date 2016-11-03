using System;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;


namespace SIPS
{

    public class Connect_Mysql
    {
        public MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string connectionString;
        string wersja ;


        /// <summary>
        /// Konstruktor klasy Connect_mYsql
        /// </summary>
        public Connect_Mysql()
        {


            //Initialize();
            Initialize1();

        }//koniec public Connect_Mysql
        
        
        /// <summary>
        /// Funkcja tworzaca polacznie MySQL 
        /// aby stworzyc połącznie wymagane są 2 dane
        /// plik zawierajacy ip serwera oraz dane dostępowe do MySQL
        /// </summary>
        private void Initialize1()
        {
            
            FileStream plik = new FileStream(@"C:\Program\Konfiguracja\utill_start.txt", FileMode.Open); // plik zawierający IP serwera MySql w formacie "ip" spacja mysnik spacja "serwer" 
            StreamReader odczyt = new StreamReader(plik);
            string wiersz = odczyt.ReadLine();
            {
                string[] dziel = wiersz.Split(new string[] { " - " }, StringSplitOptions.None);
             wersja = dziel[0];
             
                
            }
            odczyt.Close();
            plik.Close();

            
            //dane dostępowe do serwera MySql
            server = wersja;
            database = "policja";
            uid = "poczta";
            password = "Kcffc6UzVab3ErSX";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                   database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";charset=utf8";

            connection = new MySqlConnection(connectionString);
        }//koniec Initialize

        private void Initialize()
        {
            server = "127.0.0.1";
            database = "policja";
            uid = "root";
            password = "usbw";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                   database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";charset=utf8" + ";Convert Zero Datetime=True";

            connection = new MySqlConnection(connectionString);
        }//koniec Initialize

        /// <summary>
        /// Metoda odpowiedzialna za sprawdzenie połaczenia oraz sprawdzenie poprawnych danych do logowania
        /// </summary>
        /// <param name="labelek">Etykieta Labela z Form1</param>
        /// <returns>zwraca błąd w przypadku braku połączenia z bazą MySql lub błędne dane logowania</returns>
        public bool  OpenConnection(Label labelek)
        {
            

            try
            {

                connection.Open();

                return true;
            }
            catch (MySqlException ex)
            {
               
                switch ( ex.Number)
                {
                    case 1042:
                        labelek.Text = "Brak połączenia z bazą5";
                        break;
                        
                    case 0:

                        labelek.Text = "Błędny login lub hasło";
                        break;
                        
                }
                return false;
            }
        }//koniec OpenConnection

        public void CloseConnection()
        {
            try
            {
                connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }//koniec CloseConnection


    }

}
