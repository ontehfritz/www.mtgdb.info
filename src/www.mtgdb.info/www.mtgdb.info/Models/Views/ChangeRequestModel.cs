using System;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class ChangeRequestModel : PageModel
    {
        public List<CardChange> Changes     { get; set; }
        public List<NewCard>    NewCards    { get; set; }
        public List<NewSet>     NewSets     { get; set; }
        public string Status                { get; set; }

        public ChangeRequestModel () : base()
        {
            Changes =  new List<CardChange>();
            NewCards = new List<NewCard>();
            NewSets =  new List<NewSet>();
        }
    }
}

