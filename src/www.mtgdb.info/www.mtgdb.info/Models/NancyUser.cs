using System;
using Nancy.Authentication.Forms;
using Nancy.Security;
using Nancy;
using System.Collections.Generic;
using SuperSimple.Auth;

namespace mtgdb.info
{
    public class NancyUserMapper: IUserMapper
    {
        SuperSimpleAuth ssa; 

        public NancyUserMapper(SuperSimpleAuth ssa){
            this.ssa = ssa;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            User ssaUser = ssa.Validate (identifier,
                context.Request.UserHostAddress);

            NancyUserIdentity user = new NancyUserIdentity {
                UserName = ssaUser.UserName,
                AuthToken = ssaUser.AuthToken,
                Email = ssaUser.Email,
                Id = ssaUser.Id,
                Claims = ssaUser.Claims,
                Roles = ssaUser.Roles
            };

            return user;
        }
    }

    public class NancyUserIdentity : IUserIdentity
    {
        public Guid Id { get; set; }
        public Guid AuthToken { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Claims { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}