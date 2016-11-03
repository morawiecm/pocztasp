using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace SIPS
{
    class WykazPocztaPoleconePDF
    {
        string numer_wykazu, jednostka_uzytkownika, nazwa_uzytkownika, login_uzytkownika;
        Connect_Mysql conn;
        int[] tabelka = new int[300];
        Paragraph p;
        MySqlDataReader reader = null;
        string wydzial, jednostka;
        string nazwa, rodzaj, adres, adresat, waga;
        float kwota, suma;
        int  tab_lenght;
        static int numer_zestawienia;
        string msg_suma;
        string select = "SELECT Adresat, Adresat_Adres, Numer_Sprawy, Waga, Rodzaj, Kwota FROM poczta_polska WHERE Lp = @lp";
        string update = "UPDATE poczta_polska SET Tworca = @bufor, Data = @data WHERE Lp = @buf ";
        Font normal = new Font(Font.FontFamily.HELVETICA, 10f, Font.NORMAL);
        Font bold = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD);


        /// <summary>
        /// wydruk poczta polska listy polecone oraz ZPO
        /// </summary>
        /// <param name="wyk">numer wyakzu</param>
        /// <param name="con">polaczenie mysql</param>
        /// <param name="lu">login uzytkownika</param>
        /// <param name="nu">imie i nazwisko</param>
        /// <param name="ju">jednostka i wydzial</param>
        /// <param name="tab">tabela indexow</param>
        /// <param name="tab_l">wielkosc tablicy liczby listow</param>

        public WykazPocztaPoleconePDF(string wyk, Connect_Mysql con, string lu, string nu, string ju, int[] tab, int tab_l)
        {
            this.numer_wykazu = wyk;
            this.conn = con;
            this.login_uzytkownika = lu;
            this.nazwa_uzytkownika = nu;
            this.jednostka_uzytkownika = ju;
            this.tabelka = tab;
            this.tab_lenght = tab_l;
            pobierz_numer_zestawienia();
            suma = 0;
            Document doc = new Document(iTextSharp.text.PageSize.A4, 14f, 14f, 12f, 12f);
            FileInfo info = new FileInfo(@"c:\Program\" + login_uzytkownika + ".pdf");
            if (ReadyForReading(info) == true)
            { //uruchomienie procesu tworzenia pdfa
                PdfWriter pdf = PdfWriter.GetInstance(doc, new FileStream(@"c:\Program\" + login_uzytkownika + ".pdf", FileMode.Create));
                string[] dziel = jednostka_uzytkownika.Split(new string[] { " - " }, StringSplitOptions.None);
                wydzial = dziel[1];
                jednostka = dziel[0];
              //  pobierz_numer_zestawienia();

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

                p = new Paragraph("Nr umowy: 311201/W z dnia 29.09.2015r.", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);

                p = new Paragraph("WYKAZ Nr : " + numer_wykazu, bold);
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                p = new Paragraph("PRZESYŁEK POLECONYCH", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                p = new Paragraph("DLA URZĘDU POCZTOWEGO", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                doc.Add(Chunk.NEWLINE);

                PdfPTable table = new PdfPTable(9);
                table.HorizontalAlignment = 0;
                table.TotalWidth = 550f;
                table.LockedWidth = true;
                float[] widths = new float[] { 20f, 78f, 95f, 60f, 37f, 140f, 60f , 40f , 20f };
                table.SetWidths(widths);

                PdfPCell cell = new PdfPCell(new Phrase("Lp", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.MinimumHeight = 30f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Adresat", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Dokładne miejsce przeznaczenia", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Kwota zadeklarowana (Nr przesyłki)", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Waga", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Nr Nadawczy", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Uwagi (Rodzaj przesyłki)", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Opłata", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Kwota pobrania", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 4)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                int num = 1;
                for (int y = 0; y < tab_lenght; y++)
                {
                    MySqlCommand cmd = new MySqlCommand(select, conn.connection);
                    cmd.Parameters.AddWithValue("@lp", tabelka[y]);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        adresat = reader.GetString(0);
                        adres = reader.GetString(1);
                        nazwa = reader.GetString(2);
                        waga = reader.GetString(3);
                        rodzaj = reader.GetString(4);
                        kwota = reader.GetFloat(5);

                        cell = new PdfPCell(new Phrase(num.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                        cell.MinimumHeight = 30f;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(adresat, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(adres, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(nazwa, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(waga, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(rodzaj, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 8)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(kwota.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        num++;
                        suma = suma + kwota;
                        msg_suma = String.Format("{0:0.00}", suma);

                    }//koniec while
                    reader.Close();
                    cmd.Dispose();

                    uaktualnij_baze(y);
                }//koniec for-a

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);


                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("SUMA", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(msg_suma, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                doc.Add(table);
                doc.Add(Chunk.NEWLINE);

                p = new Paragraph("Numer Zestawienia : " + numer_zestawienia.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                doc.Add(Chunk.NEWLINE);

                p = new Paragraph("Ogółem przesyłek : " + (num - 1).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                p = new Paragraph("Słownie : " + Formatowanie.LiczbaSlownie(num - 1), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                doc.Add(Chunk.NEWLINE);

                p = new Paragraph("Sporządził : " + nazwa_uzytkownika, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_RIGHT;
                doc.Add(p);

                doc.Add(Chunk.NEWLINE);

                p = new Paragraph("             Data przyjęcia : ", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                p = new Paragraph("             Podpis przyjmującego : ", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
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
        }//koniec public WykazPocztaPolecone

        public void uaktualnij_baze(int ind)
        {
            MySqlCommand cmd = new MySqlCommand(update, conn.connection);
            cmd.Parameters.AddWithValue("@bufor", "Zestawienie" + numer_zestawienia);
            cmd.Parameters.AddWithValue("@data", DateTime.Now.ToShortDateString());
            cmd.Parameters.AddWithValue("@buf", tabelka[ind]);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        private void pobierz_numer_zestawienia()
        {
            
            string select = "SELECT Numer FROM zestawienie";
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                numer_zestawienia = reader.GetInt32(0);
            }
            reader.Close();
            cmd.Dispose();
            update_numer_zestawienia();
        }

        private void update_numer_zestawienia()
        {
            int ggg;
            ggg = numer_zestawienia;
            string update = "UPDATE zestawienie SET Numer = @numer";
            MySqlCommand cmd = new MySqlCommand(update, conn.connection);
            cmd.Parameters.AddWithValue("@numer", ggg + 1);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
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
                Console.WriteLine(ex);
                return false;
            }


        }//koniec readyforreading
    }
}
