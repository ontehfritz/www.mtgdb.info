using System;
using SuperSimple.Auth;
using Nancy;
using Nancy.Security;

namespace MtgDb.Info
{
    public class CardModule : NancyModule
    {
        public IRepository repository = new MongoRepository ("mongodb://localhost");

        public CardModule ()
        {
            this.RequiresAuthentication ();

            Post ["/cards/{id}/amount/{count}"] = parameters => {
                int multiverseId = (int)parameters.id;
                int count = (int)parameters.count;
                Guid walkerId = ((Planeswalker)this.Context.CurrentUser).Id;

                repository.AddUserCard(walkerId,multiverseId,count);


                return count.ToString();
            };
        }
    }
}

