using System;
using Nancy.Authentication.Forms;
using Nancy.Security;
using Nancy;
using System.Collections.Generic;
using SuperSimple.Auth;

namespace MtgDb.Info
{
    public class NancyUserMapper: IUserMapper
    {
        SuperSimpleAuth ssa; 
        IRepository repository = new MongoRepository ("mongodb://localhost");

        public NancyUserMapper(SuperSimpleAuth ssa)
        {
            this.ssa = ssa;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            try
            {
                User ssaUser = ssa.Validate (identifier,
                    context.Request.UserHostAddress);

                Planeswalker user = new Planeswalker 
                {
                    UserName = ssaUser.UserName,
                    AuthToken = ssaUser.AuthToken,
                    Email = ssaUser.Email,
                    Id = ssaUser.Id,
                    Claims = ssaUser.Claims,
                    Roles = ssaUser.Roles,
                    Profile = repository.GetProfile(ssaUser.Id)
                };
                return user;
            }
            catch(Exception e)
            {
                throw new Exception ("Guid: " + identifier.ToString() + " " + e.Message, e);
            }
                
            return null;
        }
    }

    public class Planeswalker : IUserIdentity
    {
        public Guid Id { get; set; }
        public Guid AuthToken { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Claims { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public Profile Profile { get; set; }
    }
}