using System;

namespace MtgDb.Info
{
    public class DeckModel : PageModel
    {
        public Deck Deck            { get; set; }
        public string DeckFile      { get; set; }
        public string Description   { get; set; }
        public string Name          { get; set; }
        public string Email         { get; set; }
      
        public DeckModel () : base(){}
    }
}

