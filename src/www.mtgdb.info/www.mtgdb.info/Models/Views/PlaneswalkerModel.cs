using System;
using System.Collections.Generic;
using System.Linq;

namespace MtgDb.Info
{
    public class PlaneswalkerModel : PageModel
    {
        public CardInfo[] Cards                 { get; set; }
        public bool Show                        { get; set; }
        public UserCard[] UserCards             { get; set; }
        public int TotalAmount                  { get; set; }
        public int TotalCards                   { get; set; }
        public CardSet[]  Sets                  { get; set; }
        public CardSet[]  AllSets               { get; set; }
        public Dictionary<string, int> Counts   { get; set; }
        public Profile Profile                  { get; set; }
        public string SetId                     { get; set; }
        public Dictionary<string,int> Blocks    { get; set; }
		public string Block                     { get; set; }

        public PlaneswalkerModel () : base ()
        {
            Counts = new Dictionary<string, int>();
            Blocks = new Dictionary<string, int>();
        }

        //return default setid for block or type
        public string DefaultSet(string blockOrType)
        {
            string setId = AllSets
                .Where(x => x.Block == blockOrType)
                .OrderByDescending(x => x.ReleasedAt)
                .Select(x => x.Id).FirstOrDefault();

            if(setId == null)
            {
                setId = AllSets
                    .Where(x => x.Type == blockOrType)
                    .OrderByDescending(x => x.ReleasedAt)
                    .Select(x => x.Id).FirstOrDefault();
            }

            return setId;
        }
    }
}

