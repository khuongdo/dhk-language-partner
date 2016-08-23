using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DHK_Easy_Flash_Card
{
    public class CardSetObject
    {
        public List<FlashCardObject> Cards { get; set; }
        public DataTable CardDatatable { get; set; }
        public string Name { get; set; }
        public string PdfPath { get; set; }

        public CardSetObject()
        {
            Cards = new List<FlashCardObject>();
            CardDatatable = new DataTable();
        }

        public void CreateCard(string ColumnsFront, string ColumnsBack, string SpecialField)
        {
            List<int> ColumnsFrontInt = new List<int>();
            ColumnsFront.Split(',').ToList().ForEach(x => ColumnsFrontInt.Add(Convert.ToInt32(x)));
            List<int> ColumnsBackInt = new List<int>();
            ColumnsBack.Split(',').ToList().ForEach(x => ColumnsBackInt.Add(Convert.ToInt32(x)));
            List<int> SpecialFieldInt = new List<int>();
            if (!string.IsNullOrEmpty(SpecialField))
                SpecialField.Split(',').ToList().ForEach(x => SpecialFieldInt.Add(Convert.ToInt32(x)));
            foreach (DataRow row in CardDatatable.Rows)
            {
                FlashCardObject NewCard = new FlashCardObject();
                //Add front side
                foreach (int i in ColumnsFrontInt)
                {
                    if (string.IsNullOrEmpty(row[i].ToString()) || row[i].ToString() == "\r\n")
                        continue;
                    if (SpecialFieldInt.Contains(i))
                    {
                        NewCard.FrontSide += row[i].ToString().ToUpper() + Environment.NewLine;
                    }
                    else
                    {
                        NewCard.FrontSide += row[i].ToString() + Environment.NewLine;
                    }
                }
                if (NewCard.FrontSide == null)
                    continue;
                if (NewCard.FrontSide.Length >= 2)
                {
                    NewCard.FrontSide = NewCard.FrontSide.Substring(0, NewCard.FrontSide.Count() - 2);
                }
                //Add back side
                foreach (int j in ColumnsBackInt)
                {
                    if (string.IsNullOrEmpty(row[j].ToString()))
                        continue;
                    if (SpecialFieldInt.Contains(j))
                    {
                        NewCard.BackSide += row[j].ToString().ToUpper() + Environment.NewLine;
                    }
                    else
                    {
                        NewCard.BackSide += row[j].ToString() + Environment.NewLine;
                    }
                }
                if (NewCard.BackSide == null)
                    continue;
                if (NewCard.BackSide.Length >= 2)
                {
                    NewCard.BackSide = NewCard.BackSide.Substring(0, NewCard.BackSide.Count() - 2);
                }
                Cards.Add(NewCard);
            }
        }
        
    }
}
