using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EasyFlashCard_Winform.Objects
{
    public class CardSetObject
    {
        public List<FlashCardObject> Cards { get; set; }
        public string Name { get; set; }
        public string PdfPath { get; set; }

        public CardSetObject()
        {
            Cards = new List<FlashCardObject>();
        }

        public void CreateCard(DataTable Table, string ColumnsFront, string ColumnsBack)
        {
            List<int> ColumnsFrontInt = new List<int>();
            ColumnsFront.Split(',').ToList().ForEach(x => ColumnsFrontInt.Add(Convert.ToInt32(x)));
            List<int> ColumnsBackInt = new List<int>();
            ColumnsBack.Split(',').ToList().ForEach(x => ColumnsBackInt.Add(Convert.ToInt32(x)));

            foreach (DataRow row in Table.Rows)
            {
                FlashCardObject NewCard = new FlashCardObject();
                //Add front side
                foreach (int i in ColumnsFrontInt)
                {
                    NewCard.FrontSide = row[i].ToString() + Environment.NewLine;
                }
                //Add back side
                foreach (int j in ColumnsBackInt)
                {
                    NewCard.BackSide = row[j].ToString() + Environment.NewLine;
                }
                Cards.Add(NewCard);
            }
        }
        
    }
}
