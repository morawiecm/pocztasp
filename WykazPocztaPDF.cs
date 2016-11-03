using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace SIPS
{
    class WykazPocztaPDF
    {
        string numer_wykazu,jednostka_uzytkownika,nazwa_uzytkownika,login_uzytkownika, numer_kontrolny;
        Connect_Mysql conn;
        int[] tabelka = new int[300];
        Paragraph p;
        MySqlDataReader reader = null;
        string wydzial, jednostka;
        string nazwa, rodzaj,adres, adresat, waga;
        float kwota,suma;
        int numer_zestawienia,tab_lenght, nowy_numer_zestawienia, kontrola_nr;
        string msg_suma;
        string select = "SELECT Adresat, Adresat_Adres, Numer_Sprawy, Waga, Rodzaj, Kwota, Lp FROM poczta_polska WHERE Lp = @lp";
        string update = "UPDATE poczta_polska SET Tworca = @bufor, Data = @data WHERE Lp = @buf ";
        Font normal = new Font(Font.FontFamily.HELVETICA, 10f, Font.NORMAL);
        Font bold = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD);

        /// <summary>
        /// generowanie pliku pdf dla listow zwyklych
        /// </summary>
        /// <param name="wyk">numer wyakzu</param>
        /// <param name="con">polaczenie mysql</param>
        /// <param name="lu">login uzytkownika</param>
        /// <param name="nu">imie i nazwisko</param>
        /// <param name="ju">jednostka i wydzial</param>
        /// <param name="tab">tabela indexow</param>
        /// <param name="tab_l">wielkosc tablicy liczby listow</param>

        public WykazPocztaPDF(string wyk, Connect_Mysql con , string lu, string nu, string ju, int[] tab,int tab_l)
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

                p = new Paragraph("PRZESYŁEK ZWYKŁYCH", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                p = new Paragraph("DLA URZĘDU POCZTOWEGO", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12));
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                doc.Add(Chunk.NEWLINE);

                PdfPTable table = new PdfPTable(7);
                table.HorizontalAlignment = 0;
                table.TotalWidth = 550f;
                table.LockedWidth = true;
                float[] widths = new float[] { 20f, 120f, 120f, 90f, 40f , 120f , 40f };
                table.SetWidths(widths);

                PdfPCell cell = new PdfPCell(new Phrase("Lp", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Adresat", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Adres", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Nr przesyłki", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Waga", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Rodzaj Listu", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Opłata", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                int num = 1;
                MySqlCommand cmd = new MySqlCommand(select, conn.connection);
                for ( int y = 0 ; y < tab_lenght; y++ )
                {
                    
                    cmd.Parameters.AddWithValue("@lp", tabelka[y]);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())   //string select = "SELECT Adresat, Adresat_Adres, Numer_Sprawy, Waga, Rodzaj, Kwota FROM poczta_polska WHERE Lp = @lp";
                    {
                        adresat = reader.GetString(0);
                        adres = reader.GetString(1);
                        nazwa = reader.GetString(2);
                        waga = reader.GetString(3);
                        rodzaj = reader.GetString(4);
                        kwota = reader.GetFloat(5);
                        numer_kontrolny = reader.GetString(6);

                        cell = new PdfPCell(new Phrase(num.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
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

                        cell = new PdfPCell(new Phrase(rodzaj, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 8)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(kwota.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);
                        num++;
                        suma = suma + kwota;
                        msg_suma = String.Format("{0:0.00}", suma);


                    }
                    reader.Close();
                    cmd.Parameters.Clear();

                    uaktualnij_baze(y);
                }

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

                cell = new PdfPCell(new Phrase("SUMA", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(msg_suma, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                
                doc.Add(table);
                doc.Add(Chunk.NEWLINE);

                //Console.WriteLine("numer przy druku : " + numer_zestawienia);

                PdfPTable table2 = new PdfPTable(4);
                table2.HorizontalAlignment = 0;
                table2.TotalWidth = 550f;
                table2.LockedWidth = true;
                float[] widths2 = new float[] { 200f, 90f, 200f, 90f };
                table2.SetWidths(widths2);


                PdfPCell cell2 = new PdfPCell(new Phrase("Numer Zestawienia: " + numer_zestawienia.ToString(), normal));
                cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.BorderWidth = 0;
                table2.AddCell(cell2);

                PdfContentByte content = pdf.DirectContent;
                Barcode39 bar39 = new Barcode39();
                bar39.Code = numer_zestawienia.ToString();
                bar39.Font = null;
                Image img39 = bar39.CreateImageWithBarcode(content, null, null);

                var kod_kontrola = Image.GetInstance(img39);
                var imageCell = new PdfPCell(kod_kontrola);
                imageCell.BorderWidth = 0;
                table2.AddCell(imageCell);

                cell2 = new PdfPCell(new Phrase("Kontrola: " + numer_kontrolny.ToString(), normal));
                cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.BorderWidth = 0;
                table2.AddCell(cell2);

                PdfContentByte content2 = pdf.DirectContent;
                Barcode39 bar392 = new Barcode39();
                bar392.Code = numer_kontrolny;
                bar392.Font = null;
                Image img392 = bar392.CreateImageWithBarcode(content2, null, null);

                var kod_kontrola2 = Image.GetInstance(img392);
                var imageCell2 = new PdfPCell(kod_kontrola2);
                imageCell2.BorderWidth = 0;
                table2.AddCell(imageCell2);


                doc.Add(table2);

                doc.Add(Chunk.NEWLINE);

                p = new Paragraph("Ogółem przesyłek : " + (num-1).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                p = new Paragraph("Słownie : " + Formatowanie.LiczbaSlownie(num-1), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                doc.Add(Chunk.NEWLINE);

                p = new Paragraph("Sporządził : " + nazwa_uzytkownika, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_RIGHT;
                doc.Add(p);

                doc.Add(Chunk.NEWLINE);
                    
                p = new Paragraph("             Data przyjęcia : " , FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
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
        }//koniec public wykaz

        /// <summary>
        /// poprawienie rekordu Tworca ( bufor ) na numer zestawienia ktory jest pobieranny z tabeli zestawienie
        /// </summary>
        /// <param name="ind">numer zestaqewinia pobrany z metody pobierz_numer_zestawienia</param>

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
               // Console.WriteLine("numer przy pobraniu : " + numer_zestawienia);
            }
            reader.Close();
            cmd.Dispose();
            
            update_numer_zestawienia();
            if (kontrola_nr != nowy_numer_zestawienia)
            {
                pobierz_numer_zestawienia();
            }
        }

        private void update_numer_zestawienia()
        {
            string update = "UPDATE zestawienie SET Numer = @numer";
            MySqlCommand cmd = new MySqlCommand(update, conn.connection);
            nowy_numer_zestawienia = numer_zestawienia + 1;
            cmd.Parameters.AddWithValue("@numer", nowy_numer_zestawienia);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            pobierz_kontrole();
        }

        private void pobierz_kontrole()
        {
            
            string select = "SELECT Numer FROM zestawienie";
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                kontrola_nr = reader.GetInt32(0);
                // Console.WriteLine("numer przy pobraniu : " + numer_zestawienia);
            }
            reader.Close();
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
