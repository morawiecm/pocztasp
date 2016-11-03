using System;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;


namespace SIPS
{

    public class Connect_start
    {
        public MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string connectionString;
        int serwerek;
        string wersja, wersja1;

        public Connect_start(int serw)
        {
            this.serwerek = serw;

            Initialize();
            //Initialize1();

        }//koniec public Connect_Mysql

        private void Initialize1()
        {
            FileStream plik = new FileStream(@"C:\Program\Konfiguracja\utill.txt", FileMode.Open);
            StreamReader odczyt = new StreamReader(plik);
            string wiersz = odczyt.ReadLine();
            {
                string[] dziel = wiersz.Split(new string[] { " - " }, StringSplitOptions.None);
                if (dziel[1].Equals("server0")) wersja = dziel[0];
                if (dziel[1].Equals("server1")) wersja1 = dziel[0];

            }

            if (serwerek == 0) server = wersja; else server = wersja1;

            //server = "192.168.1.180";
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
            uid = "policja";
            password = "policja";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                   database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";charset=utf8";

            connection = new MySqlConnection(connectionString);
        }//koniec Initialize

        public bool OpenConnection(Label labelek)
        {


            try
            {

                connection.Open();

                return true;
            }
            catch (MySqlException ex)
            {

                switch (ex.Number)
                {
                    case 1042:
                        labelek.Text = "Brak połączenia z bazą";
                        break;

                    case 0:

                        labelek.Text = "Błędny login lub hasło root";
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
