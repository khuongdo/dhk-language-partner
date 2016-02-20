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
                string temp = "";
                foreach (int i in ColumnsFrontInt)
                {
                    if (SpecialFieldInt.Contains(i))
                    {
                        temp += row[i].ToString().ToUpper() + Environment.NewLine;
                    }
                    else
                    {
                        temp += row[i].ToString() + Environment.NewLine;
                    }
                }
                NewCard.FrontSide = temp.Substring(0, temp.Count() - 2);
                //Add back side
                foreach (int j in ColumnsBackInt)
                {
                    if (SpecialFieldInt.Contains(j))
                    {
                        NewCard.BackSide += row[j].ToString().ToUpper() + Environment.NewLine;
                    }
                    else
                    {
                        NewCard.BackSide += row[j].ToString() + Environment.NewLine;
                    }
                }
                NewCard.BackSide = NewCard.BackSide.Substring(0, NewCard.BackSide.Count() - 2);
                Cards.Add(NewCard);
            }
        }
        
    }
}
