using System;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class SetLogsModel : PageModel
    {
        public List<SetChange> Changes { get; set; }
        public int NewVersion          { get; set; }
        public string SetId            { get; set; }

        public SetLogsModel () : base()
        {
            Changes = new List<SetChange>();
        }
    }
}


