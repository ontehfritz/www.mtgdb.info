using System;

namespace MtgDb.Info
{
    public class SettingsModel : PageModel
    {
        public string Name              { get; set; }
        public string Email             { get; set; }
        public string Password          { get; set; }
        public string ConfirmPassword   { get; set; }
        public bool Yes                 { get; set; }
       
        public SettingsModel () : base()
        {

        }
    }
}

