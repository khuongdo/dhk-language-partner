using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFlashCard_Winform.Objects;
using System.IO;
using Excel;
using System.Data;
using PdfiumViewer;
using MSWord = Microsoft.Office.Interop.Word;

namespace EasyFlashCard_Winform.ViewModel
{
    public class MainViewModel
    {
        public List<CardSetObject> CardSetCollection { get; set; }


        public MainViewModel()
        {
            CardSetCollection = new List<CardSetObject>();
        }

        public void LoadExcelFile(string Path, bool IsFirstLineHeader, string Front, string Back)
        {
            FileStream stream = File.Open(Path, FileMode.Open, FileAccess.Read);

            //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            //4. DataSet - Create column names from first row
            excelReader.IsFirstRowAsColumnNames = IsFirstLineHeader;
            DataSet result = excelReader.AsDataSet();
            //6. Free resources (IExcelDataReader is IDisposable)
            excelReader.Close();

            foreach (DataTable table in result.Tables)
            {
                CardSetObject newCardset = new CardSetObject();
                newCardset.Name = table.TableName;
                newCardset.CreateCard(table, Front, Back);
                CardSetCollection.Add(newCardset);
            }
        }

        public void CreatePDF(string Type)
        {
            MSWord.Application WordApp = new MSWord.Application();
            WordApp.Visible = true;
            MSWord.Document WordDoc = WordApp.Documents.Add();
            //Pagesetup
            WordDoc.PageSetup.PaperSize = MSWord.WdPaperSize.wdPaperA4;
            WordDoc.PageSetup.LeftMargin = 28.3464567f;
            WordDoc.PageSetup.RightMargin = 28.3464567f;
            WordDoc.PageSetup.TopMargin = 28.3464567f;
            WordDoc.PageSetup.BottomMargin = 28.3464567f;

            foreach (CardSetObject cardset in CardSetCollection)
            {
                var Cards = cardset.Cards;
                switch (Type)
                {
                    case "A4 8x2":
                        {
                            int NumberOfPage = Cards.Count / 16;
                            MSWord.Table CurrTable;
                            MSWord.Range EndRangeOfTable = WordDoc.Range(0, 0);
                            //Add pages
                            for (int i = 1; i <= 16; i++)
                            {
                                CurrTable = WordDoc.Tables.Add(EndRangeOfTable, 8, 2);
                                EndRangeOfTable = WordDoc.Range(CurrTable.Range.End, CurrTable.Range.End);
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

            }

        }
    }
}
