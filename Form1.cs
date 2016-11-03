using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace SIPS
{
    public partial class Start : Form
    {
        String login, haslo;
        public Connect_Mysql connect;
        public Logowanie logg;
        public NoweLogowanie loggg;
        private const int CP_NOCLOSE_BUTTON = 0x200;
        string adres_ip;
        int wersja_programu;

        public Start()
        {
              
            InitializeComponent();
            connect = new Connect_Mysql();
            connect.OpenConnection(label3);
            //PobierzAdresIP();
            pobierz();
            //lblAdresIP.Text = adres_ip+" "+wersja_programu;
            adres_ip = "Nie da rady";
        }

        
        private void pobierz()
        {
            
                FileStream plik = new FileStream(@"c:\Program\Konfiguracja\konfiguracja.txt", FileMode.Open);
                StreamReader odczyt = new StreamReader(plik);
                string wiersz = odczyt.ReadLine();

                string[] dziel = wiersz.Split(new string[] { " - " }, StringSplitOptions.None);
                wersja_programu = Int16.Parse(dziel[0]);

                odczyt.Close();
                plik.Close();
            

        }

        private void button1_Click(object sender, EventArgs e)        {
            //Przycisk logowania do systemu

            login = textBox1.Text;
            haslo = textBox2.Text;
            // logg = new Logowanie(login , haslo , connect , label3 );
            loggg = new NoweLogowanie(login, haslo, connect, label3, label4, Start.ActiveForm,adres_ip,wersja_programu);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            connect.CloseConnection();

            Application.Exit();
        }

        private void Start_Load(object sender, EventArgs e)
        {

        }

        private void PobierzAdresIP()
        {
            try
            {
                adres_ip = Dns.GetHostAddresses(Dns.GetHostName()).First(a => a.AddressFamily == AddressFamily.InterNetwork).ToString();
            }
            catch
            {
                adres_ip = "Nie moge pobrac adresu IP";
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form newformstart = new Sztab_Start();
            newformstart.Location = Start.ActiveForm.Location;
            Start.ActiveForm.Hide();
            newformstart.Show();
        }

      
        protected override CreateParams CreateParams
        { //dezaktywacja przycisku zamknij
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }


    }
}
