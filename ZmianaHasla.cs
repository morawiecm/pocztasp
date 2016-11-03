using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace SIPS
{
    public partial class ZmianaHasla : Form
    {
        private Connect_Mysql conn;
        private string login_uzytkownika;
        private string haslo, haslo_copy;
        string nazwa_uzytkownika, numer_wykazu, jednostka_uzytkownika;
        private string grupa;
        private int typ_usera;
        private Form oldform;
        private int id_usera, haslo_wygasa_dni;
        public ZmianaHasla(Connect_Mysql con, string log, string grup, string nu, string ju, int tu, string nw, Form of,int id_lp,int haslo_dni)
        {
            conn = con;
            login_uzytkownika = log;
            grupa = grup;
            nazwa_uzytkownika = nu;
            jednostka_uzytkownika = ju;
            typ_usera = tu;
            numer_wykazu = nw;
            oldform = of;
            id_usera = id_lp;
            haslo_wygasa_dni = haslo_dni;
            InitializeComponent();


        }
        //|| 

        private void button1_Click(object sender, EventArgs e)
        {
            bool ready = false;
            haslo = textBox1.Text;
            haslo_copy = textBox2.Text;

            int countUpper = 0, countLower = 0, countNumber = 0, i;

            if (haslo.Equals(haslo_copy))
                ready = true;

            if (ready)
            {
                for (i = 0; i < haslo.Length; i++)
                {
                    if (char.IsUpper(haslo[i])) countUpper++;
                    else if (char.IsLower(haslo[i])) countLower++;
                    else if (char.IsNumber(haslo[i])) countNumber++;

                }

                if (haslo.Length < 8 || countUpper < 1 || countLower < 1 || countNumber < 1)
                {
                    labelError.Text =
                        @"Hasło nie spełnia wymogów ( małe i duże litery plus liczba/liczby oraz min. 8 znaków)";
                }
                else
                {
                    PoprawHaslo();
                    zaloguj();
                }
            }
            else
            {
                labelError.Text = @"Róźne hasła w polach";
            }
        }

        private void PoprawHaslo()
        {
            string update = "UPDATE users SET Haslo = @haslo, Data_zmiany_hasla = @dataZmiany, Ilosc_blednych_logowan = @ilosc WHERE Login = @login";
            MySqlCommand cmdupdate = new MySqlCommand(update, conn.connection);

            cmdupdate.Parameters.AddWithValue("@haslo", haslo);
            cmdupdate.Parameters.AddWithValue("@dataZmiany", DateTime.Today);
            cmdupdate.Parameters.AddWithValue("@ilosc", 0);
            cmdupdate.Parameters.AddWithValue("@login", login_uzytkownika);
            cmdupdate.ExecuteNonQuery();
            cmdupdate.Dispose();

        }

        private void zaloguj()
        {
            string data = DateTime.Now.ToString();
            string update = "UPDATE users SET Ilosc_blednych_logowan = @ilos, data_logowania = @data_log WHERE Login = @login";
            MySqlCommand cmdupdate = new MySqlCommand(update, conn.connection);
            cmdupdate.Parameters.AddWithValue("@ilos", 0);
            cmdupdate.Parameters.AddWithValue("@login", login_uzytkownika);
            cmdupdate.Parameters.AddWithValue("@data_log", data);
            cmdupdate.ExecuteNonQuery();
            cmdupdate.Dispose();

            Form nf = new MenuGlowne(grupa, conn, nazwa_uzytkownika, jednostka_uzytkownika, typ_usera, numer_wykazu,id_usera,haslo_wygasa_dni);
            nf.Location = Start.ActiveForm.Location;
            Start.ActiveForm.Close();
            oldform.Hide();
            nf.Show();
        }


    }

}
