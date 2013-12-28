using System;
using Nancy;
using SuperSimple.Auth;
using Nancy.Authentication.Forms;
using Nancy.ModelBinding;
using Nancy.Validation;


namespace MtgDb.Info
{
    public class LogonModule : NancyModule
    {
        //take advantage of nancy's IoC
        //see bootstrapper.cs this is where SSA gets intialized
        SuperSimpleAuth ssa; 

        public LogonModule (SuperSimpleAuth ssa)
        {
            this.ssa = ssa;

            Get["/logon"] = parameters => {
                LogonModel model = new LogonModel();

                return View["Logon/logon",model];
            };

            Post["/logon"] = parameters => {
                LogonModel model = this.Bind<LogonModel>();
                model.Errors.Add("Password or/and Username is incorrect.");

                User user = null;

                try
                {
                    user = ssa.Authenticate(model.UserName, model.Secret,
                        this.Context.Request.UserHostAddress);
                }
                catch(Exception e)
                {
                    model.Errors.Add(e.Message);

                    if(user == null)
                    {
                        return View["Logon/logon", model];
                    }
                }

                return this.LoginAndRedirect(user.AuthToken, fallbackRedirectUrl: "/");
            };

            Get["/logoff"] = parameters => {
                NancyUserIdentity nuser = (NancyUserIdentity)Context.CurrentUser;
                ssa.End(nuser.AuthToken);
             
                return this.LogoutAndRedirect("/");
            };

            Get ["/signup"] = parameters => {
                SignupModel model = new SignupModel();
                return View["signup", model];
            };

            Post ["/signup"] = parameters => {
                SignupModel model = this.Bind<SignupModel>();
                var result = this.Validate(model);

                if (!result.IsValid)
                {
                    model.Errors.AddRange(ErrorUtility.GetValidationErrors(result));
                    return View["Signup", model];
                }

                try
                {
                    ssa.CreateUser(model.UserName, model.Secret, model.UserName);
                }
                catch(Exception e)
                {
                    model.Errors.Add(e.Message);
                    return View["Signup", model];
                }

                LogonModel logon = new LogonModel();
                logon.Messages.Add("You have successfully created an account. Please Sign In.");
                return View["Logon", logon];

            };
        }
    }
}

