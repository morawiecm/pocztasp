using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace SIPS
{
    class ZestawieniePS
    {
        Connect_Mysql conn,pol;
        Paragraph p;
        ListView lv;
        MySqlDataReader reader = null;
        string login_uzytkownika, nazwa_uzytkownika, jednostka_uzytkownika;
        string numer_wykazu,odbiorca,adresat;
        int[] tabelka;
        int numer_zestawienia, tryb_wyswietlania;
        Font normal = new Font(Font.FontFamily.HELVETICA, 10f, Font.NORMAL);
        Font bold = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD);

        public ZestawieniePS(Connect_Mysql con, string lu, string nu, string ju, string wyk, string odb, ListView lv3,int tw)
        {
            this.conn = con;
            this.login_uzytkownika = lu;
            this.nazwa_uzytkownika = nu;
            this.jednostka_uzytkownika = ju;
            this.numer_wykazu = wyk;
            this.odbiorca = odb;
            this.lv = lv3;
            this.tryb_wyswietlania = tw;//jesli 1 drukujemy , jesli 2 to puste
            tabelka = new int[lv.Items.Count];
            // odbiorca = "testowo";
            ustalenie_odbiorcy();
            wypelnienie_tabelki();//wykaz numerow LP z tabeli do wydruku i przesłania
            Pdf();
        }//koniec public zestawienieps

        private void ustalenie_odbiorcy()
        {
            if (odbiorca.Equals("Dolnośląskie")) odbiorca = "KWP Wrocław";
            if (odbiorca.Equals("Kujawsko-Pomorskie")) odbiorca = "KWP Bydgoszcz";
            if (odbiorca.Equals("Lubelskie")) odbiorca = "KWP Lublin";
            if (odbiorca.Equals("Łódzkie")) odbiorca = "KWP Łódź";
            if (odbiorca.Equals("Małopolskie")) odbiorca = "KWP Kraków";
            if (odbiorca.Equals("Mazowieckie-KGP Warszawa")) odbiorca = "KGP Warszawa";
            if (odbiorca.Equals("Mazowieckie-KSP Warszawa")) odbiorca = "KSP Warszawa";
            if (odbiorca.Equals("Mazowieckie-KWP Radom")) odbiorca = "KWP Radom";
            if (odbiorca.Equals("Opolskie")) odbiorca = "KWP Opole";
            if (odbiorca.Equals("Podkarpackie")) odbiorca = "KWP Rzeszów";
            if (odbiorca.Equals("Podlaskie")) odbiorca = "KWP Białystok";
            if (odbiorca.Equals("Pomorskie")) odbiorca = "KWP Gdańsk";
            if (odbiorca.Equals("Świętokrzyskie")) odbiorca = "KWP Kielce";
            if (odbiorca.Equals("Warmińsko-Mazurskie")) odbiorca = "KWP Olsztyn";
            if (odbiorca.Equals("Wielkopolskie")) odbiorca = "KWP Poznań";
            if (odbiorca.Equals("Zachodniopomorskie")) odbiorca = "KWP Szczecin";
            if (odbiorca.Equals("Śląskie")) odbiorca = "KWP Katowice";

        }

        private void pobierz_numer_zestawienia()
        {
            string select = "SELECT Numer FROM zestawienie";
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                numer_zestawienia = reader.GetInt16(0);
            }
            reader.Close();
            update_numer_zestawienia();
        }

        private void update_numer_zestawienia()
        {
            string update = "UPDATE zestawienie SET Numer = @numer";
            MySqlCommand cmd = new MySqlCommand(update, conn.connection);
            cmd.Parameters.AddWithValue("@numer", numer_zestawienia + 1);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        private void wypelnienie_tabelki()
        {
            int y = 0;
            foreach (ListViewItem item in lv.Items)
                if (item.Checked)
                {
                    tabelka[y] =Int32.Parse( item.SubItems[3].Text );
                    //Console.WriteLine(item.SubItems[3].Text);
                    y++;
                }
        }//koniec private void wypelnienie tabelki

        private void Pdf()
        {
            int lp;
            string wydzial, jednostka;
            Document doc = new Document(iTextSharp.text.PageSize.A4, 10f, 10f, 8f, 8f);
            FileInfo info = new FileInfo(@"c:\Program\" + login_uzytkownika + ".pdf");
            if (ReadyForReading(info) == true)
            {//tworzenie pdf-a
                PdfWriter pdf = PdfWriter.GetInstance(doc, new FileStream(@"c:\Program\" + login_uzytkownika + ".pdf", FileMode.Create));
                lp = 1;
                string[] dziel = jednostka_uzytkownika.Split(new string[] { " - " }, StringSplitOptions.None);
                wydzial = dziel[1];
                jednostka = dziel[0];
                pobierz_numer_zestawienia();

                doc.Open();

                p = new Paragraph("Data : " + DateTime.Now.ToShortDateString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_RIGHT;
                doc.Add(p);

                p = new Paragraph(wydzial, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                p = new Paragraph(jednostka, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);

                p = new Paragraph("WYKAZ Nr : " + numer_wykazu, bold);
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                p = new Paragraph("PRZESYŁEK WYDANYCH", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                p = new Paragraph(odbiorca, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12));
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                p = new Paragraph("(nazwa jednostki organizacyjnej odbierającej przesyłki)", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7));
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                p = new Paragraph(jednostka_uzytkownika, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 8));
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                p = new Paragraph("(nazwa przewoźnika)", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7));
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                doc.Add(Chunk.NEWLINE);

                PdfPTable table = new PdfPTable(6);
                table.HorizontalAlignment = 0;
                table.TotalWidth = 550f;
                table.LockedWidth = true;
                float[] widths = new float[] { 20f, 100f, 50f, 140f, 140f, 100f };
                table.SetWidths(widths);

                PdfPCell cell = new PdfPCell(new Phrase("Lp", normal));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Numer przesyłki", normal));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Rodzaj", normal));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Nadawca", normal));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Uwagi", normal));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Adresat", normal));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                
                string select = "SELECT Rodzaj, Nazwa, Uwagi, Tworca, Cel_numer FROM resort WHERE Lp = @lp";

                MySqlCommand cmd = new MySqlCommand(select, conn.connection);
                for ( int ind = 0 ; ind < lv.Items.Count ; ind++ )
                {
                    //MySqlCommand cmd = new MySqlCommand(select, conn.connection);
                    cmd.Parameters.AddWithValue("@lp", tabelka[ind]);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cell = new PdfPCell(new Phrase(lp.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(reader.GetString(1), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);
                        string rodzaj = null;
                        if (reader.GetInt16(0) == 1) rodzaj = "List"; else rodzaj = "Paczka";

                        cell = new PdfPCell(new Phrase(rodzaj, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(reader.GetString(3), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(reader.GetString(2), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);

                        if (tryb_wyswietlania == 2)
                        {
                            zamien_numer(reader.GetInt16(4));
                            cell = new PdfPCell(new Phrase(adresat, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.AddCell(cell);

                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.AddCell(cell);
                        }
                       // uaktualnij_baze(tabelka[ind]);
                        lp++;
                    }
                    reader.Close();
                    cmd.Parameters.Clear();
                    uaktualnij_baze(ind);
                }//koniec głównego for-a
                lp = lp - 1;
                doc.Add(table);

                doc.Add(Chunk.NEWLINE);

                p = new Paragraph("Numer Zestawienia : " + numer_zestawienia.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                doc.Add(Chunk.NEWLINE);

                p = new Paragraph("Ogółem przesyłek : " + lp, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                p = new Paragraph("Słownie : " + Formatowanie.LiczbaSlownie(lp), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                p = new Paragraph("Sporządził : " + nazwa_uzytkownika, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_RIGHT;
                doc.Add(p);

                doc.Add(Chunk.NEWLINE);

                p = new Paragraph("                 Data przyjęcia : ", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                p = new Paragraph("                 Podpis przyjmującego : ", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                doc.Close();

                
                System.Diagnostics.Process.Start(@"c:\Program\" + login_uzytkownika + ".pdf");



            }//koniec tworzenia pdf-a
            else
            {
                MessageBox.Show("Otwarty PDF");

                FileInfo file = new FileInfo(@"c:\Program\" + login_uzytkownika + ".pdf");
                file.Create().Close();
            }
        }

        public void uaktualnij_baze(int ind)
        {
            string update = "UPDATE resort SET Bufor = @bufor WHERE Lp = @buf ";
            MySqlCommand cmd = new MySqlCommand(update, conn.connection);
            cmd.Parameters.AddWithValue("@bufor", "Zestawienie" + numer_zestawienia);
            cmd.Parameters.AddWithValue("@buf", tabelka[ind]);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        private void zamien_numer(int numerekk)
        {
            pol = new Connect_Mysql();
            pol.connection.Open();
            string sel = "SELECT Nazwa FROM adresat WHERE Numer = @numer";
            MySqlCommand cmd = new MySqlCommand(sel, pol.connection);
            cmd.Parameters.AddWithValue("@numer", numerekk);
            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                adresat = reader.GetString(0);
            }//koniec while reader
            reader.Close();
            pol.connection.Close();


        }

        public bool ReadyForReading(FileInfo file)
        {
            try
            {
                using (new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.None)) { }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }


        }


    }
}
