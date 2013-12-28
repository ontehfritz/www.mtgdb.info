using System;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class UserCard
    {
        public int MultiverseId { get; set; }
        public int Amount { get; set; }
        public List<Guid> DeckId { get; set; }

        public UserCard ()
        {
            DeckId = new List<Guid> ();
        }
    }
}

