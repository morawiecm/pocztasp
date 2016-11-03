using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace SIPS
{
    class ZestawienieZbiorczePocztaFirmowa
    {
        string jednostka_uzytkownika, wydzial, jednostka, nazwa_uzytkownika;
        Connect_Mysql conn;
        Paragraph p;
        MySqlDataReader reader = null;
        string nazwa_firmy, adres_firmy;
        int paczki, zwykle, polecone,zagraniczne_zwykle,zagraniczne_polecone;

        public ZestawienieZbiorczePocztaFirmowa(string ju, Connect_Mysql con, string nu)
        {
            this.jednostka_uzytkownika = ju;
            this.conn = con;
            this.nazwa_uzytkownika = nu;
            string[] dziel = jednostka_uzytkownika.Split(new string[] { " - " }, StringSplitOptions.None);
            wydzial = dziel[1];
            jednostka = dziel[0];
            konwertuj();
            Pobierz_dane();
            Utworz_Pdf();
        }//koniec public zestawienie zbioorcze

        private void Pobierz_dane()
            {
            string jedn, nadawca;
            int numer;
            zwykle = paczki = polecone = zagraniczne_zwykle = zagraniczne_polecone = 0;
            string select = "SELECT Rodzaj_numer, Jednostka FROM poczta_polska WHERE Data = @data AND Przyjety = 1";
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            cmd.Parameters.AddWithValue("@data", DateTime.Now.ToShortDateString());
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                numer = reader.GetInt32(0);
                nadawca = reader.GetString(1);
                string[] dziel = nadawca.Split(new string[] { " - " }, StringSplitOptions.None);
                jedn = dziel[0];

                if (jedn.Equals(jednostka))
                {
                    if (numer == 1100) zwykle++;
                    if (numer == 1101) zwykle++;
                    if (numer == 1102) zwykle++;
                    if (numer == 1110) zwykle++;
                    if (numer == 1111) zwykle++;
                    if (numer == 1112) zwykle++;
                    if (numer == 1200) zwykle++;
                    if (numer == 1201) zwykle++;
                    if (numer == 1202) zwykle++;
                    if (numer == 1210) zwykle++;
                    if (numer == 1211) zwykle++;
                    if (numer == 1212) zwykle++;

                    if (numer == 1120) polecone++;
                    if (numer == 1121) polecone++;
                    if (numer == 1122) polecone++;
                    if (numer == 1130) polecone++;
                    if (numer == 1131) polecone++;
                    if (numer == 1132) polecone++;
                    if (numer == 1140) polecone++;
                    if (numer == 1141) polecone++;
                    if (numer == 1142) polecone++;
                    if (numer == 1150) polecone++;
                    if (numer == 1151) polecone++;
                    if (numer == 1152) polecone++;

                    if (numer == 1220) polecone++;
                    if (numer == 1221) polecone++;
                    if (numer == 1222) polecone++;
                    if (numer == 1230) polecone++;
                    if (numer == 1231) polecone++;
                    if (numer == 1232) polecone++;
                    if (numer == 1240) polecone++;
                    if (numer == 1241) polecone++;
                    if (numer == 1242) polecone++;
                    if (numer == 1250) polecone++;
                    if (numer == 1251) polecone++;
                    if (numer == 1252) polecone++;

                    for (int iii = 1160; iii < 1199; iii++)
                        if (numer == iii) paczki++;

                    for (int iii = 1260; iii < 1299; iii++)
                        if (numer == iii) paczki++;

                    for (int iii = 2100; iii < 2109; iii++)
                        if (numer == iii) zagraniczne_zwykle++;

                    for (int iii = 3100; iii < 3109; iii++)
                        if (numer == iii) zagraniczne_zwykle++;

                    for (int iii = 4100; iii < 4109; iii++)
                        if (numer == iii) zagraniczne_zwykle++;

                    for (int iii = 5100; iii < 5109; iii++)
                        if (numer == iii) zagraniczne_zwykle++;

                    for (int iii = 2110; iii < 2129; iii++)
                        if (numer == iii) zagraniczne_polecone++;

                    for (int iii = 3110; iii < 3129; iii++)
                        if (numer == iii) zagraniczne_polecone++;

                    for (int iii = 4110; iii < 4129; iii++)
                        if (numer == iii) zagraniczne_polecone++;

                    for (int iii = 5110; iii < 5129; iii++)
                        if (numer == iii) zagraniczne_polecone++;


                }//koniec if
            }//koniec while reader.read
            reader.Close();
            cmd.Dispose();

            }//koniec pobierz dane

        private void konwertuj()
        {
            string select = "SELECT Nazwa, Adres FROM komendy WHERE Jednostka = @jednostka";
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            cmd.Parameters.AddWithValue("@jednostka", jednostka);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                nazwa_firmy = reader.GetString(0);
                adres_firmy = reader.GetString(1);
            }
            reader.Close();
            cmd.Dispose();
        }

        private void Utworz_Pdf()
        {
            Document doc = new Document(iTextSharp.text.PageSize.A4, 10f, 10f, 8f, 8f);
            FileInfo info = new FileInfo(@"c:\Program\ZestawienieZbiorcze.pdf");
            if (ReadyForReading(info) == true)
            {
                PdfWriter pdf = PdfWriter.GetInstance(doc, new FileStream(@"c:\Program\ZestawienieZbiorcze.pdf", FileMode.Create));
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

                p = new Paragraph("Zestawienie przesyłek przekazanych do przewozu", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12));
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                p = new Paragraph("w ramach usługi POCZTA FIRMOWA", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12));
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                doc.Add(Chunk.NEWLINE);

                PdfPTable table = new PdfPTable(2);
                table.HorizontalAlignment = 0;
                table.TotalWidth = 550f;
                table.LockedWidth = true;
                float[] widths = new float[] { 350f, 200f};
                table.SetWidths(widths);

                PdfPCell cell = new PdfPCell(new Phrase("Klient otrzymał zestawienie i dokumenty nadawcze (kopie)*              tak              nie", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Przesyłki i dokumenty nadawcze przekazane do przewozu w ramach usługi POCZTA FIRMOWA w dniu " + DateTime.Now.ToShortDateString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Odebrane przesyłki lub zbiory :", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Liczba przesyłek/zbiorów (w sztukach)", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Przesyłki przekazane luzem :" , FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Obrót krajowy :" , FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Przesyłki polecone", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Przesyłki listowe z zadeklarowaną wartością", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Paczki pocztowe", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(paczki.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Paczki pocztowe z zadeklarowaną wartością", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Przesyłki firmowe polecone", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(polecone.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Przesyłki firmowe nierejestrowane", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(zwykle.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Przesyłki listowe nierejestrowane", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Przesyłki aglomeracja", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Inne usługi", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Obrót zagraniczny :", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Przesyłki listowe polecone", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(zagraniczne_polecone.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Przesyłki listowe z zadeklarowaną wartością", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Paczki pocztowe", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Paczki pocztowe z zadeklarowaną wartością", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Worek M", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Worek M polecony", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Przesyłki listowe nierejestrowane", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(zagraniczne_zwykle.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Lub przesyłki przekazane w zbiorach :" , FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Kaseta", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Paleta", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Worek", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Kontener", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("....................", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                string line1 = "KLIENT" + "\n" + "\n" + "\n";
                string line2 = "................................................................................" + "\n";
                string line3 = "data, godzina ( lub odcisk datownika ) i podpis";

                p = new Paragraph();
                Phrase ph1 = new Phrase(line1, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7));
                Phrase ph2 = new Phrase(line2, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7));
                Phrase ph3 = new Phrase(line3, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7));
                p.Add(ph1);
                p.Add(ph2);
                p.Add(ph3);
                cell = new PdfPCell(p);
                cell.MinimumHeight = 30f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                line1 = "PRZEDSTAWICIEL (POCZTA POLSKA S.A.)" + "\n" + "\n" + "\n";
                line2 = "................................................................................" + "\n";
                line3 = "data, godzina ( lub odcisk datownika ) i podpis";

                p = new Paragraph();
                ph1 = new Phrase(line1, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7));
                ph2 = new Phrase(line2, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7));
                ph3 = new Phrase(line3, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7));
                p.Add(ph1);
                p.Add(ph2);
                p.Add(ph3);
                cell = new PdfPCell(p);
                cell.MinimumHeight = 30f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                line1 = "REALIZUJĄCY (POCZTA POLSKA S.A.)**" + "\n" + "\n" + "\n";
                line2 = "................................................................................" + "\n";
                line3 = "data, godzina ( lub odcisk datownika ) i podpis";

                p = new Paragraph();
                ph1 = new Phrase(line1, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7));
                ph2 = new Phrase(line2, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7));
                ph3 = new Phrase(line3, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7));
                p.Add(ph1);
                p.Add(ph2);
                p.Add(ph3);
                cell = new PdfPCell(p);
                cell.Colspan = 2;
                cell.MinimumHeight = 30f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                doc.Add(table);

                p = new Paragraph("* - właściwe zaznaczyć znakiem 'X' ", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                p = new Paragraph("** - potwierdzenie odbioru przesyłek wraz z dokumentami nadawczymi", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                doc.Close();
                System.Diagnostics.Process.Start(@"c:\Program\ZestawienieZbiorcze.pdf");
            }//koniec readyfirreading
            else
            {
                MessageBox.Show("Otwarty PDF");

                FileInfo file = new FileInfo(@"c:\Program\ZestawienieZbiorcze.pdf");
                file.Create().Close();
            }

        }//koniec utworz pdf

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
