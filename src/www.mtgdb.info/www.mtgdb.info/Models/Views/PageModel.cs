using System;
using System.Collections.Generic;

namespace mtgdb.info
{
    public class PageModel
    {
        public List<string> Messages { get; set; }
        public List<string> Errors { get; set; }
        public string Title { get; set; }

        public PageModel ()
        {
            Messages = new List<string> ();
            Errors = new List<string> ();
        }
    }
}

