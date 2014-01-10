using System;
using SuperSimple.Auth;
using Nancy;
using Nancy.Security;

namespace MtgDb.Info
{
    public class CardModule : NancyModule
    {

        public CardModule ()
        {
            this.RequiresAuthentication ();

            Post ["/cards/{id}/add"] = parameters => {

            };

            Post ["/cards/{id}/remove"] = parameters => {

            };
        }
    }
}

