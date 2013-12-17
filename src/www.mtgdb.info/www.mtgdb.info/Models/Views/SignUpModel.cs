using System;

namespace mtgdb.info
{
    public class SignupModel : PageModel
    {
        public string UserName { get; set; }
        public string Secret { get; set; }
        public string ComfirmSecret { get; set; }

        public SignupModel () : base()
        {

        }
    }
}

