using System;
using System.Text;
using System.IO;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace SIPS
{
    public partial class PoprawPPwPS : Form
    {
        Connect_Mysql conn;
        ListView lv;
        int index,numer,kraj_zagranica;
        int ind_cb1;//index z combobox1
        int ind_cb2;//index z combobox2
        float kwota;
        string waga;
        public PoprawPPwPS(Connect_Mysql con, int ind, ListView list)
        {
            this.index = ind;
            this.conn = con;
            this.lv = list;
            InitializeComponent();
            pobierz();
            wypelnij();
        }

        private void wypelnij()
        {
            comboBox1.Items.Clear();
            comboBox5.Items.Clear();
            if (kraj_zagranica == 1)
            {
                try
                {
                    StreamReader sr = new StreamReader(@"C:\Program\Sources\pocztapolska.txt", Encoding.GetEncoding("ISO-8859-2"));
                    string line = sr.ReadLine();

                    while (line != null)
                    {
                        comboBox1.Items.Add(line);
                        line = sr.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd pliku" + ex.Message);
                }
                comboBox1.SelectedIndex = ind_cb1;



                if (comboBox1.SelectedIndex <= 5)
                {
                    try
                    {
                        StreamReader sr = new StreamReader(@"C:\Program\Sources\wagalistyk.txt", Encoding.GetEncoding("ISO-8859-2"));
                        string line = sr.ReadLine();

                        while (line != null)
                        {
                            comboBox5.Items.Add(line);
                            line = sr.ReadLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Błąd pliku" + ex.Message);
                    }
                }//koniec if

                if (comboBox1.SelectedIndex > 5)
                {
                    try
                    {
                        StreamReader sr = new StreamReader(@"C:\Program\Sources\wagapaczkik.txt", Encoding.GetEncoding("ISO-8859-2"));
                        string line = sr.ReadLine();

                        while (line != null)
                        {
                            comboBox5.Items.Add(line);
                            line = sr.ReadLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Błąd pliku" + ex.Message);
                    }
                }//koniec if

                comboBox5.SelectedIndex = ind_cb2;
            }
            else
            {
                MessageBox.Show("List jest zagraniczny -  nie do edycji");
                //this.Close();
            }
        }

        private void pobierz()
        {
            string select = "SELECT Rodzaj, Rodzaj_numer, Numer_Sprawy, Adresat, Adresat_Adres, Waga FROM poczta_polska WHERE Lp = @lp";
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            cmd.Parameters.AddWithValue("@lp", index);
            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                numer = Int16.Parse(reader.GetString(1));
                if ( numer >= 2000 )
                {
                    kraj_zagranica = 2;
                    label2.Text = reader.GetString(0) + " - Zagraniczny";
                }
                else
                {
                    kraj_zagranica = 1;
                    label2.Text = reader.GetString(0);
                }

                textBox1.Text = reader.GetString(2);
                textBox2.Text = reader.GetString(3);
                textBox3.Text = reader.GetString(4);
                waga = reader.GetString(5);
                zamien_numer();
            }
            reader.Close();
        }//koniec pobierz

        private void button21_Click(object sender, EventArgs e)
        {
            int gabaryt;
            string update = "UPDATE poczta_polska SET Rodzaj = @rodzaj, Rodzaj_numer = @rodzaj_numer, Numer_Sprawy = @numer_sprawy, Adresat = @adresat, Adresat_Adres = @adresat_adres, Waga = @waga, Kwota = @kwota WHERE Lp = @lp";
            MySqlCommand cmd = new MySqlCommand(update, conn.connection);
            cmd.Parameters.AddWithValue("@rodzaj", comboBox1.Text);
            if (checkBox1.Checked) gabaryt = 1; else gabaryt = 2;

            if (comboBox5.SelectedIndex == -1)
                numer = 1000 + gabaryt * 100 + comboBox1.SelectedIndex * 10;
            else
            numer = 1000 + gabaryt * 100 + comboBox1.SelectedIndex * 10 + comboBox5.SelectedIndex;
            cmd.Parameters.AddWithValue("@Rodzaj_numer", numer);
            cmd.Parameters.AddWithValue("@numer_sprawy", textBox1.Text);
            cmd.Parameters.AddWithValue("@adresat", textBox2.Text);
            cmd.Parameters.AddWithValue("@adresat_adres", textBox3.Text);
            cmd.Parameters.AddWithValue("@waga", comboBox5.Text);
            sprawdz_kwote();
            cmd.Parameters.AddWithValue("@kwota", kwota);
            cmd.Parameters.AddWithValue("@lp", index);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            updatelist();
            Start.ActiveForm.Close();

        }

        private void updatelist()
        {
            ListViewItem item = new ListViewItem();
            item = this.lv.SelectedItems[0];
            item.SubItems[1].Text = comboBox1.Text;
            item.SubItems[2].Text = comboBox5.Text;
            item.SubItems[3].Text = textBox1.Text;
            item.SubItems[4].Text = textBox2.Text;
            item.SubItems[5].Text = textBox3.Text;
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void sprawdz_kwote()
        {
            string select = "SELECT Kwota FROM cennik WHERE Lp = @lp";
            MySqlDataReader reader = null;
            MySqlCommand sel = new MySqlCommand(select, conn.connection);
            sel.Parameters.AddWithValue("@lp", numer);
            reader = sel.ExecuteReader();
            while (reader.Read())
            {
                kwota = reader.GetFloat(0);
            }
            reader.Close();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked) checkBox1.Checked = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) checkBox2.Checked = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void zamien_numer()
        {
            int aa;
            int num = numer;
            if (kraj_zagranica == 1)
            {
                button21.Enabled = true;
                num = num - 1000;
                if (num >= 200)
                {
                    checkBox2.Checked = true;
                    if (checkBox2.Checked) checkBox1.Checked = false;
                    num = num - 200;
                    aa = num;
                    num = num / 10;
                    ind_cb1 = num;
                    ind_cb2 = aa % 10;
                }
                else
                {
                    checkBox1.Checked = true;
                    if (checkBox1.Checked) checkBox2.Checked = false;
                    num = num - 100;
                    aa = num;
                    num = num / 10;
                    ind_cb1 = num;
                    ind_cb2 = aa % 10;
                    //Console.WriteLine(ind_cb1 + " + " + ind_cb2);
                }
            }
            else
            {
                button21.Enabled = false;
                MessageBox.Show("Listy zagraniczny nie jest do edycji");
            }
        }//koniec zamien numer

        private void PoprawPPwPS_Load(object sender, EventArgs e)
        {
            //button21.Enabled = false;
        }
    }
}
