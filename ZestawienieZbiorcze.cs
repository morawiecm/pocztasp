using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace SIPS
{
    class ZestawienieZbiorcze
    {
        string jednostka_uzytkownika, wydzial, jednostka, nazwa_uzytkownika;
        Connect_Mysql conn;
        Paragraph p;
        MySqlDataReader reader = null;
        int a, c, e, g, k, m;//liczba ekonomicznych zwykłych
        int aa, cc, ee, gg, kk, mm;//liczba priorytetowych
        float b, d, f, h, l, n;//cena jednostkowa za ekonomiczne zwykle wg gramatury
        float bb, dd, ff, hh, ll, nn;//cena jednostkowa za priorytetowe zwykle wg gramatury
        string nazwa_firmy, adres_firmy;

        public ZestawienieZbiorcze(string ju, Connect_Mysql con, string nu)
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

        private void Pobierz_dane()
        {
            string select = "SELECT Rodzaj_numer, Jednostka FROM poczta_polska WHERE Data = @data AND Przyjety = 1";
            string sel = "SELECT Lp,Kwota FROM cennik";
            string jedn, nadawca;
            float kwota;
            int numer,lp;

            a = c = e = g = k = m = 0;
            aa = cc = ee = gg = kk = mm = 0;
            MySqlCommand cmd = new MySqlCommand(select, conn.connection);
            cmd.Parameters.AddWithValue("@data", DateTime.Now.ToShortDateString());
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                numer = reader.GetInt32(0);
                nadawca = reader.GetString(1);
                string[] dziel = nadawca.Split(new string[] { " - " }, StringSplitOptions.None);
                jedn = dziel[0];

               // Console.WriteLine(jednostka_uzytkownika);
              //  Console.WriteLine(jedn);

                if (jedn.Equals(jednostka))
                {
                    
                    if (numer == 1100) a++;
                    if (numer == 1101) c++;
                    if (numer == 1102) e++;
                    if (numer == 1200) g++;
                    if (numer == 1201) k++;
                    if (numer == 1202) m++;

                    if (numer == 1110) aa++;
                    if (numer == 1111) cc++;
                    if (numer == 1112) ee++;
                    if (numer == 1210) gg++;
                    if (numer == 1211) kk++;
                    if (numer == 1212) mm++;
                }
            }
            reader.Close();
            cmd.Dispose();
            cmd = new MySqlCommand(sel, conn.connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lp = reader.GetInt32(0);
                kwota = reader.GetFloat(1);
                if (lp == 1100) b = kwota;
                if (lp == 1101) d = kwota;
                if (lp == 1102) f = kwota;
                if (lp == 1200) h = kwota;
                if (lp == 1201) l = kwota;
                if (lp == 1202) n = kwota;

                if (lp == 1110) bb = kwota;
                if (lp == 1111) dd = kwota;
                if (lp == 1112) ff = kwota;
                if (lp == 1210) hh = kwota;
                if (lp == 1211) ll = kwota;
                if (lp == 1212) nn = kwota;
            }
            reader.Close();
            cmd.Dispose();


        }//koniec pobierz dane

        private void Utworz_Pdf()
        {
            Document doc = new Document(iTextSharp.text.PageSize.A4, 10f, 10f, 8f, 8f);
            FileInfo info = new FileInfo(@"c:\Program\ZestawienieZbiorcze.pdf");
            if (ReadyForReading(info) == true)
            {
                PdfWriter pdf = PdfWriter.GetInstance(doc, new FileStream(@"c:\Program\ZestawienieZbiorcze.pdf", FileMode.Create));
                doc.Open();
               
                doc.Add(Chunk.NEWLINE);


                p = new Paragraph("Zestawienie ilościowo-wartościowe dla przesyłek listowych nierejestrowanych", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12));
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                p = new Paragraph("w obrocie krajowym nadanych w dniu   " + DateTime.Now.ToShortDateString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12));
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                doc.Add(Chunk.NEWLINE);

                p = new Paragraph("Nazwa firmy : " + nazwa_firmy + "        " + "    Adres : " + adres_firmy, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);
                p = new Paragraph("Forma opłaty za przesyłki: Umowa ID  311201/W z dnia 29.09.2015r.", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);
                doc.Add(Chunk.NEWLINE);

                PdfPTable table = new PdfPTable(7);
                table.HorizontalAlignment = 0;
                table.TotalWidth = 550f;
                table.LockedWidth = true;
                float[] widths = new float[] { 130f, 70f, 70f, 70f, 70f, 70f, 70f };
                table.SetWidths(widths);

                PdfPCell cell = new PdfPCell(new Phrase("PRZEDZIAŁ WAGOWY", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.Rowspan = 3;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("PRZESYŁKI LISTOWE NIEREJESTROWANE", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.Colspan = 6;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Ekonomiczne", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 8)));
                cell.Colspan = 3;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Priorytetowe", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 8)));
                cell.Colspan = 3;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("liczba", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("cena jednostkowa", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("łączna wartość", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("liczba", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("cena jednostkowa", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("łączna wartość", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("GABARYT A", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 8)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                for ( int i = 0; i < 6; i++)
                {
                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("do 350g", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(a.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(b.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((a * b).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(aa.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(bb.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((aa * bb).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ponad 350g do 1000g", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(c.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(d.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((c * d).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(cc.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(dd.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((cc * dd).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ponad 1000g do 2000g", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(e.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(f.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((e * f).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(ee.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(ff.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((ee * ff).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("GABARYT B", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 8)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                for (int i = 0; i < 6; i++)
                {
                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("do 350g", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(g.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(h.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((g * h).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(gg.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(hh.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((gg * hh).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ponad 350g do 1000g", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(k.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(l.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((k * l).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(kk.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(ll.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((kk * ll).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ponad 1000g do 2000g", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(m.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(n.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((m * n).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(mm.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(nn.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((mm * nn).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("RAZEM", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 8)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((a+c+e+g+k+m).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((a*b+c*d+e*f+g*h+k*l+m*n).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((aa + cc + ee + gg + kk + mm).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((aa * bb + cc * dd + ee * ff + gg * hh + kk * ll + mm * nn).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);


                doc.Add(table);
                doc.Add(Chunk.NEWLINE);


                p = new Paragraph("Przekazał : " + nazwa_uzytkownika, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 8));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                p = new Paragraph("", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 8));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                p = new Paragraph("Odebrał : ", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 8));
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                p = new Paragraph("Data i godzina odbioru : ", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 8));
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                doc.Close();



                System.Diagnostics.Process.Start(@"c:\Program\ZestawienieZbiorcze.pdf");
            }//koniec if ready to use
            else
            {
                MessageBox.Show("Otwarty PDF");

                FileInfo file = new FileInfo(@"c:\Program\ZestawienieZbiorcze.pdf");
                file.Create().Close();
            }
        }//koniec utworz pdf-a

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
