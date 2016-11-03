using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Windows.Forms;

namespace SIPS
{
    class StatystykiPDF
    {
        Connect_Mysql conn;
        Paragraph p;
        MySqlDataReader reader = null;
        string OrganW, Jednostka, poczatek, koniec, select = null;
        int Organ;
        Font normal = new Font(Font.FontFamily.HELVETICA, 6f, Font.NORMAL);
        Font bold = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD);
        // int a1, a2, a3, a4, a5, a6, a7, a8, a9, a10;
        float suma, sumasumy;


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

        /// <summary>
        /// wydruk statystyk w zakladce serwis
        /// </summary>
        /// <param name="con"></param>
        /// <param name="ow"></param>
        /// <param name="jedn"></param>
        /// <param name="pocz"></param>
        /// <param name="konie"></param>

        public StatystykiPDF(Connect_Mysql con, int ow, string jedn, string pocz, string konie)
        {
            this.conn = con;
            // this.RodzajListu = rl;
            this.Organ = ow;
            this.Jednostka = jedn;
            this.poczatek = pocz;
            this.koniec = konie;
            sumasumy = 0;
            if (Organ == -1) OrganW = "Wszystko";
            if (Organ == 0) OrganW = "Policja";
            if (Organ == 1) OrganW = "Policja KPK";
            if (Organ == 2) OrganW = "Prokuratura";
            if (Organ == 3) OrganW = "Sąd";
            if (Organ == 4) OrganW = "Egzekutor";
            if (Organ == 5) OrganW = "Inne organy";

            Document doc = new Document(iTextSharp.text.PageSize.A4, 14f, 14f, 12f, 12f);
            FileInfo info = new FileInfo(@"c:\Program\Statystyki.pdf");

            if (ReadyForReading(info) == true)
            {
                PdfWriter pdf = PdfWriter.GetInstance(doc, new FileStream(@"c:\Program\Statystyki.pdf", FileMode.Create));
                doc.Open();

                p = new Paragraph("Data : " + DateTime.Now.ToShortDateString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_RIGHT;
                doc.Add(p);

                doc.Add(Chunk.NEWLINE);

                p = new Paragraph("STATYSTYKI ZBIORCZE", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 14));
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);
                p = new Paragraph("Jednostka : " + Jednostka, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);
                p = new Paragraph("Organ wszczęcia : " + OrganW, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);
                p = new Paragraph("Data : " + "Od " + poczatek + " do " + koniec, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10));
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                doc.Add(Chunk.NEWLINE);

                PdfPTable table = new PdfPTable(14);
                table.HorizontalAlignment = 0;
                table.TotalWidth = 570f;
                table.LockedWidth = true;
                float[] widths = new float[] { 80f, 44f, 39f, 32f, 32f, 40f, 40f, 44f, 44f, 40f, 40f, 40f, 40f, 54f };
                table.SetWidths(widths);

                PdfPCell cell = new PdfPCell(new Phrase("Jednostka", normal));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("List Zwykły Ekonomiczny", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 6)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("List Zwykły Priorytet", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 6)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("List Polecony", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 6)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("List ZPO", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 6)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("List Priorytet Polecony", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 6)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("List Priorytet ZPO", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 6)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Paczka Ekonomiczna", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 6)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Paczka Ekonomiczna ZPO", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 6)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Paczka Priorytetowa", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 6)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Paczka Priorytetowa ZPO", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 6)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("List Zwykły Zagraniczny ", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 6)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("List Pol. ZPO Zagraniczny", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 6)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("SUMA", FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 6)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);


                if (OrganW.Equals("Wszystko"))
                {
                    if (Jednostka.Equals("Wszystko"))
                    { select = "SELECT Rodzaj, Jednostka, Kwota FROM poczta_polska WHERE Przyjety = 1 AND Data >= @data1 AND Data <= @data2"; }
                    else { select = "SELECT Rodzaj, Jednostka, Kwota FROM poczta_polska WHERE Przyjety = 1 AND Data >= @data1 AND Data <= @data2 "; }
                }
                else
                {
                    if (Jednostka.Equals("Wszystko"))
                        select = "SELECT Rodzaj, Jednostka, Kwota FROM poczta_polska WHERE Przyjety = 1 AND Data >= @data1 AND Data <= @data2 AND OW = @ow";
                    else select = "SELECT Rodzaj, Jednostka, Kwota FROM poczta_polska WHERE Przyjety = 1 AND Data >= @data1 AND Data <= @data2 AND OW = @ow";
                }

                MySqlCommand cmd = new MySqlCommand(select, conn.connection);
                cmd.Parameters.AddWithValue("@data1", poczatek);
                cmd.Parameters.AddWithValue("@data2", koniec);
                cmd.Parameters.AddWithValue("@ow", ow);
                cmd.Parameters.AddWithValue("@jednostka", Jednostka);

                //generowanie zestawienia - tworzenie tablicy z wartosciami
                string rodzaj, jed;
                float kw;
                int[] ilosc = new int[19];
                string jednn;
                string[] jednostki = { "KWP Gorzów Wlkp.", "KMP Gorzów Wlkp.", "KMP Zielona Góra", "KPP Krosno Odrzańskie", "KPP Międzyrzecz", "KPP Nowa Sól", "KPP Słubice", "KPP Strzelce Krajeńskie", "KPP Sulęcin", "KPP Świebodzin", "KPP Wschowa", "KPP Żagań", "KPP Żary", "KP Drezdenko", "KP Gubin", "KP Iłowa", "KP Kostrzyn", "KP Kożuchów", "KP Lubsko", "KP Rzepin", "KP Skwierzyna", "KP Sława", "KP Sulechów", "KP Szprotawa", "KP Witnica", "PP Łęknica", "PP Trzebiel" };
                string[] rodz_listu = { "List Zwykły Ekonomiczny", "List Zwykły Priorytet", "List Polecony", "List ZPO", "List Priorytet Polecony", "List Priorytet ZPO", "Paczka Ekonomiczna", "Paczka Ekonomiczna ZPO", "Paczka Priorytetowa", "Paczka Priorytetowa ZPO","List Zwykły Zagraniczny", "List Polecony ZPO Zagraniczny" };
                if (Jednostka.Equals("Wszystko"))
                    for (int gy = 0; gy < jednostki.Length; gy++)
                    {
                        cell = new PdfPCell(new Phrase(jednostki[gy], FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);



                        Array.Clear(ilosc, 0, 19);
                        suma = 0;
                        for (int jj = 0; jj < rodz_listu.Length; jj++)
                        {
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {

                                rodzaj = reader.GetString(0);
                                jed = reader.GetString(1);
                                kw = reader.GetFloat(2);

                                string[] dziel = jed.Split(new string[] { " - " }, StringSplitOptions.None);
                                jednn = dziel[0];


                                if (jednostki[gy].Equals(jednn))

                                    if (Organ == 1)
                                    {
                                        if ((rodz_listu[jj] + " - KPK").Equals(rodzaj))
                                        {
                                            // Console.WriteLine(rodz_listu[jj]);
                                            ilosc[jj] = ilosc[jj] + 1;
                                            suma = suma + kw;
                                            //     sumasumy = sumasumy +  suma;
                                        }
                                    }
                                    else
                                           // if (rodz_listu[jj].Equals(rodzaj))
                                           if ((rodz_listu[jj]).Equals(rodzaj) || (rodz_listu[jj] + " - KPK").Equals(rodzaj) || (rodz_listu[jj] + " Gabaryt A").Equals(rodzaj) || (rodz_listu[jj] + " Gabaryt B").Equals(rodzaj) || (rodz_listu[jj] + " Zagraniczny").Equals(rodzaj) )
                                    {

                                        ilosc[jj] = ilosc[jj] + 1;
                                        suma = suma + kw;
                                        //  sumasumy = sumasumy + suma;
                                    }



                            }
                            reader.Close();
                        }

                        for (int jj = 0; jj < rodz_listu.Length; jj++)
                        {
                            cell = new PdfPCell(new Phrase(ilosc[jj].ToString(), normal));
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                        }

                        cell = new PdfPCell(new Phrase(Math.Round(suma, 2, MidpointRounding.AwayFromZero).ToString(), normal));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        sumasumy = sumasumy + suma;
                    }
                else
                {

                    cell = new PdfPCell(new Phrase(Jednostka, FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10)));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    Array.Clear(ilosc, 0, 18);
                    suma = 0;
                    for (int jj = 0; jj < rodz_listu.Length; jj++)
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                            rodzaj = reader.GetString(0);
                            jed = reader.GetString(1);
                            kw = reader.GetFloat(2);

                            string[] dziel = jed.Split(new string[] { " - " }, StringSplitOptions.None);
                            jednn = dziel[0];


                            if (Jednostka.Equals(jednn))
                                if (Organ == 1)
                                {
                                    if ((rodz_listu[jj] + " - KPK").Equals(rodzaj))
                                    {
                                        // Console.WriteLine(rodz_listu[jj]);
                                        ilosc[jj] = ilosc[jj] + 1;
                                        suma = suma + kw;
                                        // sumasumy = sumasumy + suma;
                                    }
                                }
                                else
                               //  if (rodz_listu[jj].Equals(rodzaj))
                               if ((rodz_listu[jj]).Equals(rodzaj) || (rodz_listu[jj] + " - KPK").Equals(rodzaj) || (rodz_listu[jj] + " Gabaryt A").Equals(rodzaj) || (rodz_listu[jj] + " Gabaryt B").Equals(rodzaj) || (rodz_listu[jj] + " Zagraniczny").Equals(rodzaj))
                                {

                                    ilosc[jj] = ilosc[jj] + 1;
                                    suma = suma + kw;
                                    //  sumasumy = sumasumy + suma;
                                }



                        }
                        reader.Close();
                    }

                    for (int jj = 0; jj < rodz_listu.Length; jj++)
                    {
                        cell = new PdfPCell(new Phrase(ilosc[jj].ToString(), normal));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);
                    }

                    cell = new PdfPCell(new Phrase(Math.Round(suma, 2, MidpointRounding.AwayFromZero).ToString(), normal));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    sumasumy = sumasumy + suma;
                }



                for (int jj = 0; jj < 12; jj++)
                {
                    cell = new PdfPCell(new Phrase("", normal));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("SUMA", normal));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Math.Round(sumasumy, 2, MidpointRounding.AwayFromZero).ToString(), normal));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                doc.Add(table);
                doc.Close();
                System.Diagnostics.Process.Start(@"c:\Program\Statystyki.pdf");
            }
            else
            {
                MessageBox.Show("Otwarty PDF");

                FileInfo file = new FileInfo(@"c:\Program\Statystyki.pdf");
                file.Create().Close();
            }
        }

    }
}
