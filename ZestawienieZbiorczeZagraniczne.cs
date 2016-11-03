using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace SIPS
{
    class ZestawienieZbiorczeZagraniczne
    {
        string jednostka_uzytkownika, wydzial, jednostka, nazwa_uzytkownika;
        Connect_Mysql conn;
        Paragraph p;
        MySqlDataReader reader = null;
        string nazwa_firmy, adres_firmy;
        int a, b, c, d, e, f;
        float kwota, aa, aaa, bb, bbb, cc, ccc, dd, ddd, ee, eee, ff, fff;
        int g, h, k, l, m, n;
        float gg, ggg, hh, hhh, kk, kkk, ll, lll, mm, mmm, nn, nnn;
        int o, u, q, r, s, t;
        float oo, ooo, uu, uuu, qq, qqq, rr, rrr, ss, sss, tt, ttt;
        int v, w, x, y, z, j;
        float vv, vvv, ww, www, xx, xxx, yy, yyy, zz, zzz, jj, jjj;
        int a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12;
        float aa1, aaa1, aa2, aaa2, aa3, aaa3, aa4, aaa4, aa5, aaa5, aa6, aaa6, aa7, aaa7, aa8, aaa8, aa9, aaa9, aa10, aaa10, aa11, aaa11, aa12, aaa12;
        int b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12;
        float bb1, bbb1, bb2, bbb2, bb3, bbb3, bb4, bbb4, bb5, bbb5, bb6, bbb6, bb7, bbb7, bb8, bbb8, bb9, bbb9, bb10, bbb10, bb11, bbb11, bb12, bbb12;
        int c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12;
        float cc1, ccc1, cc2, ccc2, cc3, ccc3, cc4, ccc4, cc5, ccc5, cc6, ccc6, cc7, ccc7, cc8, ccc8, cc9, ccc9, cc10, ccc10, cc11, ccc11, cc12, ccc12;
        int d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, d12;
        float dd1, ddd1, dd2, ddd2, dd3, ddd3, dd4, ddd4, dd5, ddd5, dd6, ddd6, dd7, ddd7, dd8, ddd8, dd9, ddd9, dd10, ddd10, dd11, ddd11, dd12, ddd12;

        public ZestawienieZbiorczeZagraniczne(string ju, Connect_Mysql con, string nu)
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

            a = b = c = d = e = f = 0;
            aa = bb = cc = dd = ee = ff = 0;
            g = h = k = l = m = n = 0;
            gg = hh = kk = ll = mm = nn = 0;
            o = u = q = r = s = t = 0;
            oo = uu = qq = rr = ss = tt = 0;
            v = w = x = y = z = j = 0;
            vv = ww = xx = yy = zz = jj = 0;
            a1 = a2 = a3 = a4 = a5 = a6 = a7 = a8 = a9 = a10 = a11 = a12 = 0;
            aa1 = aa2 = aa3 = aa4 = aa5 = aa6 = aa7 = aa8 = aa9 = aa10 = aa11 = aa12 = 0;
            b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = b9 = b10 = b11 = b12 = 0;
            bb1 = bb2 = bb3 = bb4 = bb5 = bb6 = bb7 = bb8 = bb9 = bb10 = bb11 = bb12 = 0;
            c1 = c2 = c3 = c4 = c5 = c6 = c7 = c8 = c9 = c10 = c11 = c12 = 0;
            cc1 = cc2 = cc3 = cc4 = cc5 = cc6 = cc7 = cc8 = cc9 = cc10 = cc11 = cc12 = 0;
            d1 = d2 = d3 = d4 = d5 = d6 = d7 = d8 = d9 = d10 = d11 = d12 = 0;
            dd1 = dd2 = dd3 = dd4 = dd5 = dd6 = dd7 = dd8 = dd9 = dd10 = dd11 = dd12 = 0;

            string select = "SELECT Rodzaj_numer, Jednostka FROM poczta_polska WHERE Data = @data AND Przyjety = 1";
            string sel = "SELECT Lp,Kwota FROM cennik";
            string jedn, nadawca;
            
            int numer, lp;
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
                    if (numer == 2100) a++;
                    if (numer == 2101) b++;
                    if (numer == 2102) c++;
                    if (numer == 2103) d++;
                    if (numer == 2104) e++;
                    if (numer == 2105) f++;

                    if (numer == 3100) g++;
                    if (numer == 3101) h++;
                    if (numer == 3102) k++;
                    if (numer == 3103) l++;
                    if (numer == 3104) m++;
                    if (numer == 3105) n++;

                    if (numer == 4100) o++;
                    if (numer == 4101) u++;
                    if (numer == 4102) q++;
                    if (numer == 4103) r++;
                    if (numer == 4104) s++;
                    if (numer == 4105) t++;

                    if (numer == 5100) v++;
                    if (numer == 5101) w++;
                    if (numer == 5102) x++;
                    if (numer == 5103) y++;
                    if (numer == 5104) z++;
                    if (numer == 5105) j++;

                    if (numer == 2110) a1++;
                    if (numer == 2111) a2++;
                    if (numer == 2112) a3++;
                    if (numer == 2113) a4++;
                    if (numer == 2114) a5++;
                    if (numer == 2115) a6++;

                    if (numer == 2120) a7++;
                    if (numer == 2121) a8++;
                    if (numer == 2122) a9++;
                    if (numer == 2123) a10++;
                    if (numer == 2124) a11++;
                    if (numer == 2125) a12++;

                    if (numer == 3110) b1++;
                    if (numer == 3111) b2++;
                    if (numer == 3112) b3++;
                    if (numer == 3113) b4++;
                    if (numer == 3114) b5++;
                    if (numer == 3115) b6++;

                    if (numer == 3120) b7++;
                    if (numer == 3121) b8++;
                    if (numer == 3122) b9++;
                    if (numer == 3123) b10++;
                    if (numer == 3124) b11++;
                    if (numer == 3125) b12++;

                    if (numer == 4110) c1++;
                    if (numer == 4111) c2++;
                    if (numer == 4112) c3++;
                    if (numer == 4113) c4++;
                    if (numer == 4114) c5++;
                    if (numer == 4115) c6++;

                    if (numer == 4120) c7++;
                    if (numer == 4121) c8++;
                    if (numer == 4122) c9++;
                    if (numer == 4123) c10++;
                    if (numer == 4124) c11++;
                    if (numer == 4125) c12++;

                    if (numer == 5110) d1++;
                    if (numer == 5111) d2++;
                    if (numer == 5112) d3++;
                    if (numer == 5113) d4++;
                    if (numer == 5114) d5++;
                    if (numer == 5115) d6++;

                    if (numer == 5120) d7++;
                    if (numer == 5121) d8++;
                    if (numer == 5122) d9++;
                    if (numer == 5123) d10++;
                    if (numer == 5124) d11++;
                    if (numer == 5125) d12++;

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

                if (lp == 2100) aa = kwota;
                if (lp == 2101) bb = kwota;
                if (lp == 2102) cc = kwota;
                if (lp == 2103) dd = kwota;
                if (lp == 2104) ee = kwota;
                if (lp == 2105) ff = kwota;

                if (lp == 3100) gg = kwota;
                if (lp == 3101) hh = kwota;
                if (lp == 3102) kk = kwota;
                if (lp == 3103) ll = kwota;
                if (lp == 3104) mm = kwota;
                if (lp == 3105) nn = kwota;

                if (lp == 4100) oo = kwota;
                if (lp == 4101) uu = kwota;
                if (lp == 4102) qq = kwota;
                if (lp == 4103) rr = kwota;
                if (lp == 4104) ss = kwota;
                if (lp == 4105) tt = kwota;

                if (lp == 5100) vv = kwota;
                if (lp == 5101) ww = kwota;
                if (lp == 5102) xx = kwota;
                if (lp == 5103) yy = kwota;
                if (lp == 5104) zz = kwota;
                if (lp == 5105) jj = kwota;

                if (lp == 2110) aa1 = kwota;
                if (lp == 2111) aa2 = kwota;
                if (lp == 2112) aa3 = kwota;
                if (lp == 2113) aa4 = kwota;
                if (lp == 2114) aa5 = kwota;
                if (lp == 2115) aa6 = kwota;

                if (lp == 2120) aa7 = kwota;
                if (lp == 2121) aa8 = kwota;
                if (lp == 2122) aa9 = kwota;
                if (lp == 2123) aa10 = kwota;
                if (lp == 2124) aa11 = kwota;
                if (lp == 2125) aa12 = kwota;

                if (lp == 3110) bb1 = kwota;
                if (lp == 3111) bb2 = kwota;
                if (lp == 3112) bb3 = kwota;
                if (lp == 3113) bb4 = kwota;
                if (lp == 3114) bb5 = kwota;
                if (lp == 3115) bb6 = kwota;

                if (lp == 3120) bb7 = kwota;
                if (lp == 3121) bb8 = kwota;
                if (lp == 3122) bb9 = kwota;
                if (lp == 3123) bb10 = kwota;
                if (lp == 3124) bb11 = kwota;
                if (lp == 3125) bb12 = kwota;

                if (lp == 4110) cc1 = kwota;
                if (lp == 4111) cc2 = kwota;
                if (lp == 4112) cc3 = kwota;
                if (lp == 4113) cc4 = kwota;
                if (lp == 4114) cc5 = kwota;
                if (lp == 4115) cc6 = kwota;

                if (lp == 4120) cc7 = kwota;
                if (lp == 4121) cc8 = kwota;
                if (lp == 4122) cc9 = kwota;
                if (lp == 4123) cc10 = kwota;
                if (lp == 4124) cc11 = kwota;
                if (lp == 4125) cc12 = kwota;

                if (lp == 5110) dd1 = kwota;
                if (lp == 5111) dd2 = kwota;
                if (lp == 5112) dd3 = kwota;
                if (lp == 5113) dd4 = kwota;
                if (lp == 5114) dd5 = kwota;
                if (lp == 5115) dd6 = kwota;

                if (lp == 5120) dd7 = kwota;
                if (lp == 5121) dd8 = kwota;
                if (lp == 5122) dd9 = kwota;
                if (lp == 5123) dd10 = kwota;
                if (lp == 5124) dd11 = kwota;
                if (lp == 5125) dd12 = kwota;
            }
            reader.Close();
            cmd.Dispose();

            aaa = a * aa;
            bbb = b * bb;
            ccc = c * cc;
            ddd = d * dd;
            eee = e * ee;
            fff = f * ff;
            ggg = g * gg;
            hhh = h * hh;
            kkk = k * kk;
            lll = l * ll;
            mmm = m * mm;
            nnn = n * nn;
            ooo = o * oo;
            uuu = u * uu;
            qqq = q * qq;
            rrr = r * rr;
            sss = s * ss;
            ttt = t * tt;
            vvv = v * vv;
            www = w * ww;
            xxx = x * xx;
            yyy = y * yy;
            zzz = z * zz;
            jjj = j * jj;

            aaa1 = a1 * aa1;
            aaa2 = a2 * aa2;
            aaa3 = a3 * aa3;
            aaa4 = a4 * aa4;
            aaa5 = a5 * aa5;
            aaa6 = a6 * aa6;
            aaa7 = a7 * aa7;
            aaa8 = a8 * aa8;
            aaa9 = a9 * aa9;
            aaa10 = a10 * aa10;
            aaa11 = a11 * aa11;
            aaa12 = a12 * aa12;

            bbb1 = b1 * bb1;
            bbb2 = b2 * bb2;
            bbb3 = b3 * bb3;
            bbb4 = b4 * bb4;
            bbb5 = b5 * bb5;
            bbb6 = b6 * bb6;
            bbb7 = b7 * bb7;
            bbb8 = b8 * bb8;
            bbb9 = b9 * bb9;
            bbb10 = b10 * bb10;
            bbb11 = b11 * bb11;
            bbb12 = b12 * bb12;

            ccc1 = c1 * cc1;
            ccc2 = c2 * cc2;
            ccc3 = c3 * cc3;
            ccc4 = c4 * cc4;
            ccc5 = c5 * cc5;
            ccc6 = c6 * cc6;
            ccc7 = c7 * cc7;
            ccc8 = c8 * cc8;
            ccc9 = c9 * cc9;
            ccc10 = c10 * cc10;
            ccc11 = c11 * cc11;
            ccc12 = c12 * cc12;

            ddd1 = d1 * dd1;
            ddd2 = d2 * dd2;
            ddd3 = d3 * dd3;
            ddd4 = d4 * dd4;
            ddd5 = d5 * dd5;
            ddd6 = d6 * dd6;
            ddd7 = d7 * dd7;
            ddd8 = d8 * dd8;
            ddd9 = d9 * dd9;
            ddd10 = d10 * dd10;
            ddd11 = d11 * dd11;
            ddd12 = d12 * dd12;

        }//koniec private void pobierz dane

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
        }//koniec konwertuj

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

                p = new Paragraph("w obrocie zagranicznym nadanych w dniu   " + DateTime.Now.ToShortDateString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12));
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

                PdfPTable table = new PdfPTable(13);
                table.HorizontalAlignment = 0;
                table.TotalWidth = 550f;
                table.LockedWidth = true;
                float[] widths = new float[] { 130f, 35f, 35f, 35f, 35f, 35f, 35f, 35f, 35f, 35f, 35f, 35f, 35f };
                table.SetWidths(widths);

                PdfPCell cell = new PdfPCell(new Phrase("PRZEDZIAŁ WAGOWY", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.Rowspan = 4;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("PRZESYŁKI LISTOWE NIEREJESTROWANE", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.Colspan = 12;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Ekonomiczne", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 8)));
                cell.Colspan = 4;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Priorytetowe", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 8)));
                cell.Colspan = 8;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Kraje europejskie", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Kraje pozaeuropejskie", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("STREFA 'A'", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("STREFA 'B'", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("STREFA 'C'", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("STREFA 'D'", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                for ( int i = 0; i < 6; i++)
                {
                    cell = new PdfPCell(new Phrase("liczba", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("wartość", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("do 50g", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(a.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(aaa.ToString() , FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((g + o + v).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((ggg + ooo + vvv).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((a1+a7).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((aaa1+aaa7).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((b1 + b7).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((bbb1 + bbb7).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((c1 + c7).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((ccc1 + ccc7).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((d1 + d7).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((ddd1 + ddd7).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ponad 50g do 100g", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(b.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(bbb.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((h+u+w).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((hhh + uuu + www).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((a2 + a8).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((aaa2 + aaa8).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase((b2 + b8).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((bbb2 + bbb8).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((c2 + c8).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((ccc2 + ccc8).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((d2 + d8).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((ddd2 + ddd8).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ponad 100g do 350g", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(c.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(ccc.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((k+x+q).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((kkk+xxx+qqq).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((a3 + a9).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((aaa3 + aaa9).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((b3 + b9).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((bbb3 + bbb9).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((c3 + c9).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((ccc3 + ccc9).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((d3 + d9).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((ddd3 + ddd9).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ponad 350g do 500g", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(d.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(ddd.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((l+r+y).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((lll+rrr+yyy).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((a4 + a10).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((aaa4 + aaa10).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((b4 + b10).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((bbb4 + bbb10).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((c4 + c10).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((ccc4 + ccc10).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((d4 + d10).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((ddd4 + ddd10).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ponad 500g do 1000g", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(e.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(eee.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((m+z+s).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((mmm + zzz + sss).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((a5 + a11).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((aaa5 + aaa11).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((b5 + b11).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((bbb5 + bbb11).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((c5 + c11).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((ccc5 + ccc11).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((d5 + d11).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((ddd5 + ddd11).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ponad 1000g do 2000g", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(f.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(fff.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((n+t+j).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((nnn + ttt + jjj).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((a6 + a12).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((aaa6 + aaa12).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((b6 + b12).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((bbb6 + bbb12).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((c6 + c12).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((ccc6 + ccc12).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((d6 + d12).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((ddd6 + ddd12).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("RAZEM", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 9)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((a+b+c+d+e+f).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((aaa+bbb+ccc+ddd+eee+fff).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((g+h+k+l+m+n+o+u+q+r+s+t+v+w+x+y+z+j).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((ggg + hhh + kkk+lll+mmm+nnn+ooo+uuu+qqq+rrr+sss+ttt+vvv+www+xxx+yyy+zzz+jjj).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((a1+a2+a3+a4+a5+a6+a7+a8+a9+a10+a11+a12).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((aaa1 + aaa2+aaa3+aaa4+aaa5+aaa6+aaa7+aaa8+aaa9+aaa10+aaa11+aaa12).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((b1 + b2 + b3 + b4 + b5 + b6 + b7 + b8 + b9 + b10 + b11 + b12).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((bbb1 + bbb2 + bbb3 + bbb4 + bbb5 + bbb6 + bbb7 + bbb8 + bbb9 + bbb10 + bbb11 + bbb12).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((c1 + c2 + c3 + c4 + c5 + c6 + c7 + c8 + c9 + c10 + c11 + c12).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((ccc1 + ccc2 + ccc3 + ccc4 + ccc5 + ccc6 + ccc7 + ccc8 + ccc9 + ccc10 + ccc11 + ccc12).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((d1 + d2 + d3 + d4 + d5 + d6 + d7 + d8 + d9 + d10 + d11 + d12).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase((ddd1 + ddd2 + ddd3 + ddd4 + ddd5 + ddd6 + ddd7 + ddd8 + ddd9 + ddd10 + ddd11 + ddd12).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 7)));
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
            }//koniec readyforreading
            else
            {
                MessageBox.Show("Otwarty PDF");

                FileInfo file = new FileInfo(@"c:\Program\ZestawienieZbiorcze.pdf");
                file.Create().Close();
            }
        }//koniec private void utworz_pdf

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
