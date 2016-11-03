using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace SIPS
{
    class Wykaz_Poprawiony_PP_Zwykle
    {
        string   nr_zestawienia;
        string numer_wykazu, nazwa_uzytkownika;
        Connect_Mysql conn;
        int[] tabelka = new int[300];
        Paragraph p;
        MySqlDataReader reader = null;
        string wydzial, jednostka;
        string nazwa, rodzaj, adres, adresat, waga;
        string pelna_nazwa_jednostki;
        float kwota, suma;
        string msg_suma;
        string select = "SELECT Adresat, Adresat_Adres, Numer_Sprawy, Waga, Rodzaj, Kwota FROM poczta_polska WHERE Tworca = @lp";
        string select1 = "SELECT Jednostka FROM poczta_polska WHERE Tworca = @tworca";
        //string update = "UPDATE poczta_polska SET Tworca = @bufor, Data = @data WHERE Lp = @buf ";
        Font normal = new Font(Font.FontFamily.HELVETICA, 10f, Font.NORMAL);
        Font bold = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD);
        MySqlCommand cmd;

        public Wykaz_Poprawiony_PP_Zwykle(string t9, string t10, string t11, Connect_Mysql con)
        {
            this.nr_zestawienia = "Zestawienie" + t9;
            this.numer_wykazu = t10;
            this.nazwa_uzytkownika = t11;
            this.conn = con;
            suma = 0;
            Document doc = new Document(iTextSharp.text.PageSize.A4, 14f, 14f, 12f, 12f);
            FileInfo info = new FileInfo(@"c:\Program\Popraw_PP.pdf");
            if (ReadyForReading(info) == true)
            { //uruchomienie procesu tworzenia pdfa
                cmd = new MySqlCommand(select1, conn.connection);
                cmd.Parameters.AddWithValue("@tworca", nr_zestawienia);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pelna_nazwa_jednostki = reader.GetString(0);
                    
                }
                reader.Close();

                
                PdfWriter pdf = PdfWriter.GetInstance(doc, new FileStream(@"c:\Program\Popraw_PP.pdf", FileMode.Create));
                string[] dziel = pelna_nazwa_jednostki.Split(new string[] { " - " }, StringSplitOptions.None);
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
                float[] widths = new float[] { 20f, 120f, 120f, 90f, 40f, 120f, 40f };
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

                cmd = new MySqlCommand(select, conn.connection);
                cmd.Parameters.AddWithValue("@lp", nr_zestawienia);
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

                p = new Paragraph(nr_zestawienia, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
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

                p = new Paragraph("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_RIGHT;
                doc.Add(p);

                p = new Paragraph("Poprawił :                            ", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
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



                System.Diagnostics.Process.Start(@"c:\Program\Popraw_PP.pdf");


            }//koniec tworzenia pdf-a
            else { MessageBox.Show("Otwarty PDF");

                FileInfo file = new FileInfo(@"c:\Program\Popraw_PP.pdf");
                file.Create().Close();
            }
            

        }//koniec public

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
