using System;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class ChangeRequestModel : PageModel
    {
        public List<CardChange> Changes     { get; set; }
        public List<NewCard>    NewCards    { get; set; }
        public string Status                { get; set; }

        public ChangeRequestModel () : base()
        {
            Changes = new List<CardChange>();
        }

    }
}

