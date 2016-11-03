using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System.Windows.Forms;



namespace SIPS
{
    class WykazPDF
    {
        Connect_Mysql conn;
        Connect_Mysql pol;
        Paragraph p;
        string login_uzytkownika, nazwa_uzytkownika,jednostka_uzytkownika, numer_kontrolny;
        string wydzial, jednostka, wykaz;
        string nazwa, rodzaj, uwagi, adresat;
        int cel_numer , ro;
        int numer_zestawienia, nowy_numer_zestawienia, kontrola_nr;
        int wybor,lp;
        MySqlDataReader reader = null;
        string select = null, nazwa_wykazu = null, update = null; //select do zapytania sql oraz nazwa_wykazu czyli czy jawne czy niejawne
        Font normal = new Font(Font.FontFamily.HELVETICA, 10f, Font.NORMAL);
        Font bold = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD);

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

        public WykazPDF(Connect_Mysql con , string lu , string nu , int w , string ju , string wy)
        {
            this.conn = con;
            this.login_uzytkownika = lu;
            this.nazwa_uzytkownika = nu;
            this.wybor = w;
            this.jednostka_uzytkownika = ju;
            this.wykaz = wy;
            
            //lp = 1;
            pobierz_numer_zestawienia();
            if ( wybor == 1)
            {
                nazwa_wykazu = "PRZESYŁEK JAWNYCH NADANYCH";
                select = "SELECT Rodzaj , Nazwa , Cel_numer , Uwagi, Lp FROM resort WHERE Bufor = @bufor AND Typ = 1";
                update = "UPDATE resort SET Bufor = @bufor WHERE Bufor = @buf AND Typ = 1";
            }//koniec if ==1
            else
            {
                nazwa_wykazu = "PRZESYŁEK NIEJAWNYCH NADANYCH";
                select = "SELECT Rodzaj , Nazwa , Cel_numer , Uwagi, Lp FROM resort WHERE Bufor = @bufor AND Typ = 2";
                update = "UPDATE resort SET Bufor = @bufor WHERE Bufor = @buf AND Typ = 2";
            }//koniec else

            Document doc = new Document(iTextSharp.text.PageSize.A4 , 14f , 14f , 12f , 12f);
            FileInfo info = new FileInfo(@"c:\Program\" + login_uzytkownika + ".pdf");
            //ReadyForReading(info);

            //  if (ReadyForReading) { }
            if (lp != 1)
                if (ReadyForReading(info) == true)

                { //uruchomienie procesu tworzenia pdfa
                    PdfWriter pdf = PdfWriter.GetInstance(doc, new FileStream(@"c:\Program\" + login_uzytkownika + ".pdf", FileMode.Create));
                    lp = 1;
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

                    p = new Paragraph("WYKAZ Nr : " + wykaz, bold);
                    p.Alignment = Element.ALIGN_CENTER;
                    doc.Add(p);

                    p = new Paragraph(nazwa_wykazu, bold);
                    p.Alignment = Element.ALIGN_CENTER;
                    doc.Add(p);

                    doc.Add(Chunk.NEWLINE);

                    PdfPTable table = new PdfPTable(5);
                    table.HorizontalAlignment = 0;
                    table.TotalWidth = 550f;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 20f, 160f, 50f, 160f, 160f };
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

                    cell = new PdfPCell(new Phrase("Adresat", normal));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Uwagi", normal));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    MySqlCommand cmd = new MySqlCommand(select, conn.connection);
                    cmd.Parameters.AddWithValue("@bufor", login_uzytkownika);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ro = reader.GetInt16(0);
                        nazwa = reader.GetString(1);
                        cel_numer = reader.GetInt16(2);
                        uwagi = reader.GetString(3);
                        numer_kontrolny = reader.GetString(4);


                        if (ro == 1) rodzaj = "List"; else rodzaj = "Paczka";
                        zamien_numer(cel_numer);

                        cell = new PdfPCell(new Phrase(lp.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        
                        table.AddCell(cell);

                        lp++;

                        cell = new PdfPCell(new Phrase(nazwa, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(rodzaj, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 8)));
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(adresat, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(uwagi, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);
                        

                    }//koniec while dla reader
                    reader.Close();
                    lp = lp - 1;

                    doc.Add(table);

                    doc.Add(Chunk.NEWLINE);
                    /*
                    p = new Paragraph("Numer Zestawienia : " + numer_zestawienia.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                    p.Alignment = Element.ALIGN_LEFT;
                    doc.Add(p);
                    doc.Add(Chunk.NEWLINE);
                    */
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



                    //doc.Add(img39);

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
                    //MessageBox.Show(Formatowanie.LiczbaSlownie(lp));

                    uaktualnij_baze();
                    System.Diagnostics.Process.Start(@"c:\Program\" + login_uzytkownika + ".pdf");
                }//koniec tworzenia pdf-a
                else
                {
                    MessageBox.Show("Otwarty PDF");

                    FileInfo file = new FileInfo(@"c:\Program\" + login_uzytkownika + ".pdf");
                    file.Create().Close();
                }
            else MessageBox.Show("Brak listów do wydrukowania" , "Brak wpisu");
        }//koniec public wykazPDF

        public void uaktualnij_baze()
        {
            MySqlCommand cmd = new MySqlCommand(update, conn.connection);
            cmd.Parameters.AddWithValue("@bufor", "Zestawienie" + numer_zestawienia);
            cmd.Parameters.AddWithValue("@buf", login_uzytkownika);
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
            
            update_numer_zestawienia();
            
            if(kontrola_nr != nowy_numer_zestawienia)
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



    }
}
