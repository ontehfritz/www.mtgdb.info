using System;

namespace MtgDb.Info
{
    public class LogonModel : PageModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Secret { get; set; }
        public string UrlRedirect { get; set; }

        public LogonModel () : base()
        {

        }
    }
}

