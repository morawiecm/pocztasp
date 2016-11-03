using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;


namespace SIPS
{
    public partial class MenuGlowne : Form
    {
        string login_uzytkownika, nazwa_uzytkownika, jednostka_uzytkownika, numer_wykazu;
        int id_uzytkownika,haslo_wygasnie_dni;
        Connect_Mysql conn;
        int typ_usera;

        private void MenuGlowne_Load(object sender, EventArgs e)
        {
            if ( typ_usera == 1)
            {
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = false;
                button4.Enabled = false;
            }
            if (login_uzytkownika.Equals((1).ToString())) archiwum();

        }

        public void StatusWniosku(int id_uzytkownika)
        {
            int status_wniosku =0 ;
            string select_wniosek_status = "SELECT wniosek FROM users WHERE Lp ="+id_uzytkownika;
            MySqlCommand cmd = new MySqlCommand(select_wniosek_status, conn.connection);
            MySqlDataReader reader = cmd.ExecuteReader();
           // cmd.Parameters.AddWithValue("@id_lp", id_uzytkownika);
            while (reader.Read())
            {
                status_wniosku = reader.GetInt32(0);
            }
            reader.Close();
            cmd.Dispose();
            if(status_wniosku == 1)
            {
                lblStatusWniosku.ForeColor = System.Drawing.Color.Green;
                lblStatusWniosku.Text = "Status wniosku : Zweryfikowny";
            }
            else
            {
                lblStatusWniosku.ForeColor = System.Drawing.Color.Red;
                lblStatusWniosku.Text = "Status wniosku : Brak";
            }
            
        }

        private void archiwum()
        {
            int proces = 0 , numer = 0 ;
            string select_archiwum = "SELECT Proces, Numer FROM zestawienie";
            MySqlCommand cmd = new MySqlCommand(select_archiwum, conn.connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                proces = reader.GetInt32(0);
                numer = reader.GetInt32(1);
            }
            reader.Close();
            cmd.Dispose();

            if (proces == 0)
                if (numer > 7000)
                {
                    Form nf = new Progress_Clock();
                    nf.Show();

                    string update = "UPDATE zestawienie SET Proces = 1 ";
                    cmd = new MySqlCommand(update, conn.connection);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    update = "UPDATE resort SET Bufor = 'archiwum' WHERE Bufor = @bufor";
                    cmd = new MySqlCommand(update, conn.connection);
                    for ( int i = 0 ; i < 4000; i++)
                    {
                        cmd.Parameters.AddWithValue("@bufor", "Zestawienie" + i);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    cmd.Dispose();
                }
            if (proces == 1)
                if (numer > 9000)
                {
                    Form nf = new Progress_Clock();
                    nf.Show();
                    string update = "UPDATE zestawienie SET Proces = 2 ";
                    cmd = new MySqlCommand(update, conn.connection);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    update = "UPDATE zestawienie SET Numer = 1 ";
                    cmd = new MySqlCommand(update, conn.connection);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    update = "UPDATE resort SET Bufor = 'archiwum' WHERE Bufor = @bufor";
                    cmd = new MySqlCommand(update, conn.connection);
                    for (int i = 3999; i < 6000; i++)
                    {
                        cmd.Parameters.AddWithValue("@bufor", "Zestawienie" + i);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    cmd.Dispose();
                }
            if (proces == 2)
                if (numer > 1000)
                {
                    Form nf = new Progress_Clock();
                    nf.Show();
                    string update = "UPDATE zestawienie SET Proces = 0 ";
                    cmd = new MySqlCommand(update, conn.connection);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    update = "UPDATE resort SET Bufor = 'archiwum' WHERE Bufor = @bufor";
                    cmd = new MySqlCommand(update, conn.connection);
                    for (int i = 5999; i < 9500; i++)
                    {
                        cmd.Parameters.AddWithValue("@bufor", "Zestawienie" + i);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    cmd.Dispose();
                }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.GetCurrentProcess().Kill();
            conn.CloseConnection();
            Application.Exit();
        }

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form newform = new PocztaPolska(Start.ActiveForm, login_uzytkownika, conn, nazwa_uzytkownika, jednostka_uzytkownika);
            newform.Location = Start.ActiveForm.Location;
            Start.ActiveForm.Hide();
            newform.Show();
        }

        private void lblZglosBlad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form newform = new ZglosBlad(conn, id_uzytkownika);
            newform.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form newform = new PocztaSpecjalna(login_uzytkownika, conn, nazwa_uzytkownika, jednostka_uzytkownika, Start.ActiveForm);
            newform.Location = Start.ActiveForm.Location;
           // Start.ActiveForm.Hide();
            newform.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form newform = new Serwis(conn);
            newform.Location = Start.ActiveForm.Location;
           // Start.ActiveForm.Hide();
            newform.Show();
        }

        public MenuGlowne(string lu , Connect_Mysql con , string nu , string ju , int tu,string nw, int id_lp,int haslo_dni )
        {
            this.nazwa_uzytkownika = nu;
            this.login_uzytkownika = lu;
            this.jednostka_uzytkownika = ju;
            this.conn = con;
            this.typ_usera = tu;
            this.numer_wykazu = nw;
            this.id_uzytkownika = id_lp;
            this.haslo_wygasnie_dni = haslo_dni;
            
            InitializeComponent();
            lblUzytkownik.Text = "Użytkownik: " + nazwa_uzytkownika + "(" + login_uzytkownika + ")";
            StatusWniosku(id_uzytkownika);
            if(haslo_wygasnie_dni==1)
            {
                lblHasloWygasa.Text = "Hasło wygasa za: " + haslo_wygasnie_dni + " dzień";
            }
            else
            {
                lblHasloWygasa.Text = "Hasło wygasa za: " + haslo_wygasnie_dni + " dni";
            }




        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form newform = new Resort(login_uzytkownika, conn, nazwa_uzytkownika, jednostka_uzytkownika,Start.ActiveForm, id_uzytkownika);
            newform.Location = Start.ActiveForm.Location;
            Start.ActiveForm.Hide();
            newform.Show();
        }
    }
}
