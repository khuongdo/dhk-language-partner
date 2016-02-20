using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Excel;
using System.Data;
using MSWord = Microsoft.Office.Interop.Word;

namespace DHK_Easy_Flash_Card
{
    public class MainViewModel:ViewModelBase
    {
        private string _ExcelPath;
        private string _Front;
        private string _Back;
        private string _OutputPath;
        private string _SpecialField;
        private float _FrontFontSize;

        public float FrontFontSize
        {
            get { return _FrontFontSize; }
            set
            {
                if (value != _FrontFontSize)
                {
                    _FrontFontSize = value;
                    OnPropertyChanged("FrontFontSize");
                }
            }
        }
        public string SpecialField
        {
            get { return _SpecialField; }
            set
            {
                if (value != _SpecialField)
                {
                    _SpecialField = value;
                    OnPropertyChanged("SpecialField");
                }
            }
        }
        public string ExcelPath
        {
            get { return _ExcelPath; }
            set
            {
                if (value != _ExcelPath)
                {
                    _ExcelPath = value;
                    OnPropertyChanged("ExcelPath");
                }
            }
        }
        public string OutputPath
        {
            get { return _OutputPath; }
            set
            {
                if (value != _OutputPath)
                {
                    _OutputPath = value;
                    OnPropertyChanged("OutputPath");
                }
            }
        }
        public string Front
        {
            get { return _Front; }
            set
            {
                if (value != _Front)
                {
                    _Front = value;
                    OnPropertyChanged("Front");
                }
            }
        }
        public string Back
        {
            get { return _Back; }
            set
            {
                if (value != _Back)
                {
                    _Back = value;
                    OnPropertyChanged("Back");
                }
            }
        }
        public List<CardSetObject> CardSetCollection { get; set; }
        

        public MainViewModel()
        {
            CardSetCollection = new List<CardSetObject>();
            FrontFontSize = 14f;
        }

        public void LoadExcelFile(bool IsFirstLineHeader)
        {
            FileStream stream = File.Open(ExcelPath, FileMode.Open, FileAccess.Read);

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
                newCardset.CardDatatable = table;
                CardSetCollection.Add(newCardset);
            }
        }
        public void CreateCard()
        {
            foreach (var item in CardSetCollection)
            {
                item.CreateCard(Front, Back, SpecialField);
            }
        }

        public void CreatePDF()
        {
            if (string.IsNullOrEmpty(OutputPath) || string.IsNullOrEmpty(ExcelPath) || string.IsNullOrEmpty(Front) || string.IsNullOrEmpty(Back))
                return;
            MSWord.Application WordApp = new MSWord.Application();
            WordApp.Visible = true;

            LoadExcelFile(true);
            CreateCard();

            //Write cards
            foreach (CardSetObject cardset in CardSetCollection)
            {
                MSWord.Document WordDoc = WordApp.Documents.Add();
                //Pagesetup
                WordDoc.PageSetup.PaperSize = MSWord.WdPaperSize.wdPaperA4;
                WordDoc.PageSetup.LeftMargin = 28.3464567f;
                WordDoc.PageSetup.RightMargin = 28.3464567f;
                WordDoc.PageSetup.TopMargin = 28.3464567f;
                WordDoc.PageSetup.BottomMargin = 28.3464567f;
                //Calculate and insert table
                int NumberOfRow = ((cardset.Cards.Count / 16 + 1) * 2) * 8;
                MSWord.Table CurrTable;
                CurrTable = WordDoc.Tables.Add(WordDoc.Range(0, 0), NumberOfRow, 2);
                CurrTable.Borders.OutsideLineStyle = MSWord.WdLineStyle.wdLineStyleSingle;
                CurrTable.Borders.InsideLineStyle = MSWord.WdLineStyle.wdLineStyleSingle;
                CurrTable.Rows.Height = 96.378f;
                CurrTable.Rows.HeightRule = MSWord.WdRowHeightRule.wdRowHeightExactly;
                CurrTable.Range.ParagraphFormat.Alignment = MSWord.WdParagraphAlignment.wdAlignParagraphCenter;
                int CardIndex = 0;
                for (int row = 1; row <= NumberOfRow; row++)
                {
                    if (CardIndex >= cardset.Cards.Count)
                    {
                        break;
                    }
                    for (int col = 1; col <= 2; col++)
                    {
                        if (CardIndex >= cardset.Cards.Count)
                        {
                            break;
                        }
                        var CurrFrontCell = CurrTable.Cell(row, col);
                        if (CurrFrontCell.Range.Text != "\r\a")
                        {
                            break;
                        }
                        CurrFrontCell.VerticalAlignment = MSWord.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                        CurrFrontCell.Range.InsertAfter(cardset.Cards[CardIndex].FrontSide);
                        CurrFrontCell.Range.Font.Bold = 1;
                        CurrFrontCell.Range.Font.Size = FrontFontSize;
                        int BackSideCol = col == 1 ? 2 : 1;
                        CurrTable.Cell(row + 8, BackSideCol).Range.InsertAfter(cardset.Cards[CardIndex].BackSide);
                        CurrTable.Cell(row + 8, BackSideCol).VerticalAlignment = MSWord.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                        CardIndex++;
                    }
                }
                WordDoc.ExportAsFixedFormat(Path.Combine(OutputPath, cardset.Name + ".pdf"), MSWord.WdExportFormat.wdExportFormatPDF, true);
            }
            WordApp.Quit(MSWord.WdSaveOptions.wdDoNotSaveChanges);
        }
    }
}
