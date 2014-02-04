using System;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class PlaneswalkerModel : PageModel
    {
        public CardInfo[] Cards { get; set; }
        public bool Show { get; set; }
        public UserCard[] UserCards { get; set; }
        public int TotalAmount { get; set; }
        public int TotalCards { get; set; }
        public CardSet[]  Sets { get; set; }
        public Dictionary<string, int> Counts { get; set; }
        public Profile Profile { get; set; }
        public string SetId { get; set; }
        public Dictionary<string,int> Blocks { get; set; }
        public string Block { get; set; }

        public PlaneswalkerModel () : base ()
        {
            Counts = new Dictionary<string, int>();
            Blocks = new Dictionary<string, int>();
            //this.Planeswalker
        }
    }
}

