using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using Image = iTextSharp.text.Image;

namespace WPF_Bestelbons.Models
{
    public class PDFCreator
    {
        public string ProjectDirectory { get; set; }
        public float PageFillLevel;

        public static float CalculatePdfPTableHeight(PdfPTable table)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.A4))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        w.CloseStream = false;
                        doc.Open();
                        doc.Add(new Chunk(""));
                        table.TotalWidth = 257;
                        table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);

                        doc.Close();
                        return table.TotalHeight;
                    }
                }
            }
        }

        public void Create(Bestelbon bestelbon, User CurrentnUser, FileStream fs)
        {
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);   // Margin Left, Margin Right, Margin Top , Margin Bottom
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            writer.PageEvent = new Footer();
            document.Open();
            PageFillLevel = 0.0f;

            FontFactory.Register("c:/windows/fonts/SegoeUI.ttf", "SegoeUI");

            Font Segoe08 = FontFactory.GetFont("SegoeUI", 8F);
            Font Segoe10 = FontFactory.GetFont("SegoeUI", 10F);
            Font Segoe12 = FontFactory.GetFont("SegoeUI", 12F);
            Font Segoe14 = FontFactory.GetFont("SegoeUI", 14F);
            Font Segoe16 = FontFactory.GetFont("SegoeUI", 16F);
            Font Segoe18 = FontFactory.GetFont("SegoeUI", 18F);
            Font Segoe20 = FontFactory.GetFont("SegoeUI", 20F);
            Font Segoe24 = FontFactory.GetFont("SegoeUI", 24F);
            BaseColor LightGray = new BaseColor(192, 192, 192, 64);

            #region FIRST LAYER
            Image img = Image.GetInstance("D:/Desktop/Bestelbons/LOGOORANGE.png");
            img.ScaleToFit(150, 150);
            img.SetAbsolutePosition(70.0f, 680.0f);
            document.Add(img);

            #endregion
            #region SECOND LAYER

            // ADRESS REGION ASTRATEC

            PdfPTable Adresstable = new PdfPTable(5);
            Adresstable.DefaultCell.Border = PdfPCell.RIGHT_BORDER;
            Adresstable.SetWidths(new float[] { 1.5f, 1f, 1f, 1f, 1.7f });
            Adresstable.WidthPercentage = 100;
            Adresstable.HorizontalAlignment = Element.ALIGN_LEFT;

            Paragraph pEmpty = new Paragraph("", Segoe14);
            PdfPCell cellEmpty = new PdfPCell(pEmpty);
            cellEmpty.Border = PdfPCell.NO_BORDER;


            Paragraph pFirma = new Paragraph("  ASTRA-TEC  BVBA", Segoe24);
            PdfPCell cellFirma = new PdfPCell(pFirma);
            cellFirma.Colspan = 3;
            cellFirma.Border = PdfPCell.NO_BORDER;
            Adresstable.AddCell(cellFirma);

            Adresstable.AddCell(cellEmpty);
            Paragraph pDatum = new Paragraph(DateTime.Now.ToString(), Segoe10);
            PdfPCell datumCell = new PdfPCell(pDatum);
            datumCell.Border = PdfPCell.NO_BORDER;
            datumCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            datumCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            Adresstable.AddCell(datumCell);

            Paragraph pStraat = new Paragraph("Industrielaan", Segoe12);
            PdfPCell cellStraat = new PdfPCell(pStraat);
            cellStraat.Border = PdfPCell.NO_BORDER;
            cellStraat.HorizontalAlignment = Element.ALIGN_LEFT;
            Adresstable.AddCell(cellStraat);

            Paragraph pNummer = new Paragraph("19", Segoe12);
            PdfPCell cellNummer = new PdfPCell(pNummer);
            cellNummer.Border = PdfPCell.NO_BORDER;
            cellNummer.HorizontalAlignment = Element.ALIGN_RIGHT;
            Adresstable.AddCell(cellNummer);

            Adresstable.AddCell(cellEmpty);
            Adresstable.AddCell(cellEmpty);
            Adresstable.AddCell(cellEmpty);

            Paragraph pPostCode = new Paragraph("8810", Segoe12);
            PdfPCell cellPostCode = new PdfPCell(pPostCode);
            cellPostCode.Border = PdfPCell.NO_BORDER;
            cellPostCode.HorizontalAlignment = Element.ALIGN_LEFT;
            Adresstable.AddCell(cellPostCode);

            Paragraph pStad = new Paragraph("Lichtervelde", Segoe12);
            PdfPCell cellStad = new PdfPCell(pStad);
            cellStad.Border = PdfPCell.NO_BORDER;
            cellStad.HorizontalAlignment = Element.ALIGN_RIGHT;
            Adresstable.AddCell(cellStad);

            Adresstable.AddCell(cellEmpty);
            Adresstable.AddCell(cellEmpty);
            Adresstable.AddCell(cellEmpty);

            Paragraph pBTWnr = new Paragraph("BE 0455.138.549", Segoe12);
            PdfPCell cellBTWnr = new PdfPCell(pBTWnr);
            cellBTWnr.Border = PdfPCell.NO_BORDER;
            cellBTWnr.HorizontalAlignment = Element.ALIGN_LEFT;
            Adresstable.AddCell(cellBTWnr);

            Adresstable.AddCell(cellEmpty);
            Adresstable.AddCell(cellEmpty);
            Adresstable.AddCell(cellEmpty);
            Paragraph pContact = new Paragraph("Contact :    ", Segoe12);
            PdfPCell contactCell = new PdfPCell(pContact);
            contactCell.Border = PdfPCell.NO_BORDER;
            contactCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Adresstable.AddCell(contactCell);

            Paragraph pTEL = new Paragraph("+32 (0) 51 72 24 46", Segoe12);
            PdfPCell cellTEL = new PdfPCell(pTEL);
            cellTEL.Border = PdfPCell.NO_BORDER;
            cellTEL.HorizontalAlignment = Element.ALIGN_LEFT;
            Adresstable.AddCell(cellTEL);

            Adresstable.AddCell(cellEmpty);
            Adresstable.AddCell(cellEmpty);
            Adresstable.AddCell(cellEmpty);
            Paragraph pUser = new Paragraph(CurrentnUser.Email, Segoe12);
            PdfPCell userCell = new PdfPCell(pUser);
            userCell.Border = PdfPCell.NO_BORDER;
            userCell.Colspan = 2;
            userCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Adresstable.AddCell(userCell);

            Chunk chunkWWW = new Chunk("www.astratec.be");
            chunkWWW.SetAnchor("http://www.astratec.be");
            Paragraph pWWW = new Paragraph();
            pWWW.Add(chunkWWW);
            PdfPCell cellWWW = new PdfPCell(pWWW);
            cellWWW.Colspan = 2;
            cellWWW.Border = PdfPCell.NO_BORDER;
            cellWWW.HorizontalAlignment = Element.ALIGN_LEFT;
            Adresstable.AddCell(cellWWW);

            Adresstable.AddCell(cellEmpty);
            Adresstable.AddCell(cellEmpty);
            Paragraph pUsertel = new Paragraph(CurrentnUser.Tel, Segoe12);
            PdfPCell usertelCell = new PdfPCell(pUsertel);
            usertelCell.Border = PdfPCell.NO_BORDER;
            usertelCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Adresstable.AddCell(usertelCell);
            PageFillLevel += CalculatePdfPTableHeight(Adresstable);


            Adresstable.SpacingBefore = 15f;
            Adresstable.SpacingAfter = 30f;
            PageFillLevel += 45;            // 15 + 30 as the previous set SpacingBefore and SpacingAfter


            document.Add(Adresstable);

            Paragraph pBESTELBON = new Paragraph("                                                           BESTELBON", Segoe24);
            pBESTELBON.SpacingAfter = 10f;
            PageFillLevel += 10;            // 15 + 30 as the previous set SpacingAfter
            document.Add(pBESTELBON);


            document.Add(new Paragraph("Leverancier : " + bestelbon.Leverancier.Name.ToString(), Segoe12));

            float[] widthstable = { 1.5f, 1f, 15f, 2f, 2f };
            PdfPTable table = new PdfPTable(widthstable);
            table.DefaultCell.Border = PdfPCell.NO_BORDER;
            table.WidthPercentage = 100;
            int j = 0;
            bool grayfill = false;

            foreach (Bestelbonregel regel in bestelbon.Bestelbonregels)
            {
                if (j % 2 == 0) grayfill = true;
                else grayfill = false;

                Paragraph pAantal = new Paragraph(regel.Aantal.ToString(), Segoe12);
                PdfPCell cellAantal = new PdfPCell(pAantal);
                cellAantal.Border = PdfPCell.NO_BORDER;
                cellAantal.FixedHeight = 16f;
                if (grayfill) cellAantal.BackgroundColor = LightGray;
                table.AddCell(cellAantal);

                Paragraph pEenheid = new Paragraph(regel.Eenheid.ToString(), Segoe08);
                PdfPCell cellEenheid = new PdfPCell(pEenheid);
                cellEenheid.Border = PdfPCell.NO_BORDER;
                cellEenheid.PaddingTop = 7f;
                cellEenheid.VerticalAlignment = Element.ALIGN_TOP;
                if (grayfill) cellEenheid.BackgroundColor = LightGray;
                table.AddCell(cellEenheid);

                Paragraph pBestelregel = new Paragraph(regel.Bestelregel.ToString(), Segoe12);
                PdfPCell cellBestelregel = new PdfPCell(pBestelregel);
                cellBestelregel.Border = PdfPCell.NO_BORDER;
                if (grayfill) cellBestelregel.BackgroundColor = LightGray;
                table.AddCell(cellBestelregel);

                Paragraph pPrice = new Paragraph(regel.Prijs.ToString("0000.00").TrimStart('0'), Segoe12);
                PdfPCell cellPrice = new PdfPCell(pPrice);
                cellPrice.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellPrice.Border = PdfPCell.NO_BORDER;
                if (grayfill) cellPrice.BackgroundColor = LightGray;
                table.AddCell(cellPrice);

                Paragraph pTotalPrice = new Paragraph(regel.TotalePrijs.ToString("0000.00").TrimStart('0'), Segoe12);
                PdfPCell cellTotalPrice = new PdfPCell(pTotalPrice);
                cellTotalPrice.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellTotalPrice.Border = PdfPCell.NO_BORDER;
                if (grayfill) cellTotalPrice.BackgroundColor = LightGray;
                table.AddCell(cellTotalPrice);
                
                j++;
                PageFillLevel += 16;  //   cellAantal.FixedHeight = 16f;
                if (PageFillLevel >= 872 - 225 - 60 - 60)         // 2e 60 is Top and bottom margin of Page !!
                {
                    table.SpacingBefore = 30f;
                    document.Add(table);
                    document.NewPage();                          // Opmerkingen en Handtekening tabellen zijn 225 hoog + A4 size is 872 !! + 60 spacing after en before !
                    PageFillLevel = 0;
                    table.DeleteBodyRows();
                }


            }


            table.SpacingBefore = 30f;
            PageFillLevel += 30;
            table.SpacingAfter = 30f;
            PageFillLevel += 30;           

            document.Add(table);
            Paragraph pTotaal = new Paragraph("                                                                                                           TOTAAL :    " + bestelbon.TotalPrice.ToString("0000.00").TrimStart('0'), Segoe14);
            pTotaal.SpacingAfter = 20f;
            PageFillLevel += 34;            // 20 as the previous set SpacingAfter + 14 for the TOTAAL string
            document.Add(pTotaal);

            // TABLE MET OPMERKINGEN

            PdfPTable Opmerkingentable = new PdfPTable(2);
            Opmerkingentable.DefaultCell.Border = PdfPCell.NO_BORDER;
            Opmerkingentable.SetWidths(new float[] { 30.0f, 0.5f });
            Opmerkingentable.WidthPercentage = 100;
            Opmerkingentable.HorizontalAlignment = Element.ALIGN_CENTER;

            Paragraph pOpmerkingText = new Paragraph("Opmerking", Segoe12);
            PdfPCell celllev1 = new PdfPCell(pOpmerkingText);
            celllev1.FixedHeight = 20f;
            celllev1.HorizontalAlignment = Element.ALIGN_LEFT;
            celllev1.Border = PdfPCell.NO_BORDER;
            Opmerkingentable.AddCell(celllev1);

            Opmerkingentable.AddCell(cellEmpty);

            Paragraph pOpmerking = new Paragraph(bestelbon.Opmerking, Segoe08);
            PdfPCell celllev2 = new PdfPCell(pOpmerking);
            celllev2.FixedHeight = 20f;
            celllev2.HorizontalAlignment = Element.ALIGN_LEFT;
            celllev2.Border = PdfPCell.NO_BORDER;
            Opmerkingentable.AddCell(celllev2);

            Opmerkingentable.AddCell(cellEmpty);

            Opmerkingentable.SpacingAfter = 10f;
            PageFillLevel += 10;            // 10 as the previous set SpacingAfter

            document.Add(Opmerkingentable);

            PageFillLevel += CalculatePdfPTableHeight(Opmerkingentable);

            if (PageFillLevel >= 872 - 115 - 60)  // 60 is Top and bottom margin of Page !!
            {
                document.NewPage();        // 115 = height of handtekeningenTABLE !!
                PageFillLevel = 0;
            }




            // TABLE MET LEVERINGSVOORWAARDEN EN HANDTEKENING

            PdfPTable Leveringsvwdntable = new PdfPTable(3);
            Leveringsvwdntable.DefaultCell.Border = PdfPCell.NO_BORDER;
            Leveringsvwdntable.SetWidths(new float[] { 3.0f, 0.5f, 0.5f });
            Leveringsvwdntable.WidthPercentage = 100;
            Leveringsvwdntable.HorizontalAlignment = Element.ALIGN_CENTER;

            Paragraph pLeveringsvoorwaardenText = new Paragraph("Leveringsvoorwaarden", Segoe12);
            PdfPCell cell1 = new PdfPCell(pLeveringsvoorwaardenText);
            cell1.Border = PdfPCell.NO_BORDER;
            cell1.FixedHeight = 20f;
            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
            Leveringsvwdntable.AddCell(cell1);

            Paragraph pVoorAkkoordText = new Paragraph("Voor Akkoord", Segoe12);
            PdfPCell cell2 = new PdfPCell(pVoorAkkoordText);
            cell2.Colspan = 2;
            cell2.Border = PdfPCell.NO_BORDER;
            cell2.FixedHeight = 20f;
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            Leveringsvwdntable.AddCell(cell2);

            Paragraph pLeveeringsvoorwaarden = new Paragraph(bestelbon.Leveringsvoorwaarden, Segoe08);
            PdfPCell cell3 = new PdfPCell(pLeveeringsvoorwaarden);
            cell3.Rowspan = 2;
            cell3.Border = PdfPCell.NO_BORDER;
            cell3.HorizontalAlignment = Element.ALIGN_LEFT;
            Leveringsvwdntable.AddCell(cell3);

            Paragraph pLastName = new Paragraph(CurrentnUser.LastName, Segoe10);
            PdfPCell cell4 = new PdfPCell(pLastName);
            cell4.Border = PdfPCell.NO_BORDER;
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.FixedHeight = 20f;
            Leveringsvwdntable.AddCell(cell4);

            Paragraph pFirstName = new Paragraph(CurrentnUser.FirstName, Segoe10);
            PdfPCell cell5 = new PdfPCell(pFirstName);
            cell5.Border = PdfPCell.NO_BORDER;
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.FixedHeight = 20f;
            Leveringsvwdntable.AddCell(cell5);

            Image sigimage = Image.GetInstance(CurrentnUser.Handtekening);
            sigimage.ScaleToFit(75.0f, 75.0f);
            PdfPCell sig = new PdfPCell(sigimage);
            sig.Border = PdfPCell.NO_BORDER;
            sig.Colspan = 2;
            sig.HorizontalAlignment = Element.ALIGN_CENTER;
            Leveringsvwdntable.AddCell(sig);


            document.Add(Leveringsvwdntable);
            PageFillLevel += CalculatePdfPTableHeight(Leveringsvwdntable);

            int NumberOfPages = writer.PageNumber;

            #endregion
            document.Close();
            writer.Close();

        }




    }

    public class Footer : PdfPageEventHelper
    {
        public PdfTemplate total { get; set; }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            total = writer.DirectContent.CreateTemplate(30, 16); ; // Width and Height    Creating a template as a placeholder in each Page so we can fill it in at the end !!
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {

            try
            {
                base.OnEndPage(writer, document);
                PdfPTable pagetable = new PdfPTable(3);
                pagetable.SetWidths(new float[] { 24f, 24f, 2f });
                pagetable.TotalWidth = 527;
                pagetable.HorizontalAlignment = Element.ALIGN_CENTER;
                pagetable.DefaultCell.FixedHeight = 20f;
                pagetable.DefaultCell.Border = PdfPCell.TOP_BORDER;

                pagetable.AddCell("");
                pagetable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                pagetable.AddCell($"Page {writer.PageNumber} of ");
                PdfPCell cell = new PdfPCell(Image.GetInstance(total));
                cell.Border = PdfPCell.TOP_BORDER;
                pagetable.AddCell(cell);
                pagetable.WriteSelectedRows(0, -1, 34, 34, writer.DirectContent);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            ColumnText.ShowTextAligned(total, Element.ALIGN_LEFT, new Phrase((writer.PageNumber).ToString()), 2, 2, 0);
        }

    }


}
