using System;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class PageModel
    {
        public List<string> Messages { get; set; }
        public List<string> Errors { get; set; }
        public List<string> Information { get; set; }
        public List<string> Warnings { get; set; }
        public string Title { get; set; }
        public Planeswalker Planeswalker { get; set; }
        public string ActiveMenu { get; set; }

        public PageModel ()
        {
            Messages = new List<string> ();
            Errors = new List<string> ();
            Information = new List<string> ();
        }
    }
}

