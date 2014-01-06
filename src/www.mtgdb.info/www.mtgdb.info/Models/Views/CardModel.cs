using System;

namespace MtgDb.Info
{
    public class CardModel : PageModel
    {
        public Card Card { get; set; }
        public Card[] Prints { get; set; }

        public CardModel (){
        }
    }
}

