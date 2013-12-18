using System;
using System.Collections.Generic;

namespace mtgdb.info
{
    public class UserCard
    {
        public string UserName { get; set; }
        public int MultiverseId { get; set; }
        public int Amount { get; set; }
        public List<Guid> DeckId { get; set; }

        public UserCard ()
        {
            DeckId = new List<Guid> ();
        }
    }
}

