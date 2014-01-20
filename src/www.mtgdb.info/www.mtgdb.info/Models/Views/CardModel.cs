using System;

namespace MtgDb.Info
{
    public class CardModel : PageModel
    {
        public Card Card { get; set; }
        public int Amount { get; set; } 
        public Card[] Prints { get; set; }

        public CardModel (){
        }
    }
}