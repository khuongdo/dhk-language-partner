using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHK_Easy_Flash_Card
{
    public class FlashCardObject
    {
        public string FrontSide { get; set; }
        public string BackSide { get; set; }
        public int FrontX { get; set; }
        public int FrontY { get; set; }
        public int BackX { get; set; }
        public int BackY { get; set; }

        public FlashCardObject() { }
    }
}
