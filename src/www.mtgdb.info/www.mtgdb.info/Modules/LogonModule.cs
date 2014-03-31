using System;
using Nancy;
using SuperSimple.Auth;
using Nancy.Authentication.Forms;
using Nancy.ModelBinding;
using Nancy.Validation;
using System.Configuration;


namespace MtgDb.Info
{
    public class LogonModule : NancyModule
    {
        //take advantage of nancy's IoC
        //see bootstrapper.cs this is where SSA gets intialized
        SuperSimpleAuth ssa; 
        IRepository repository = 
            new MongoRepository (ConfigurationManager.AppSettings.Get("db"));

        public LogonModule (SuperSimpleAuth ssa)
        {
            this.ssa = ssa;

            Get["/settings"] = parameters => {
                SettingsModel model = new SettingsModel();

                if(this.Context.CurrentUser == null)
                {
                    return this.LogoutAndRedirect("/");
                }

                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;
               
                return View["Logon/Settings",model];
            };

            Post["/settings"] = parameters => {
                SettingsModel model = this.Bind<SettingsModel>();

                if(this.Context.CurrentUser == null)
                {
                    return this.LogoutAndRedirect("/");
                }

                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;
               

                if(Request.Form.Save != null)
                {
                    model.Planeswalker.Profile.Email = model.Email;
                    model.Planeswalker.Profile.Name = model.Name;

                    try
                    {
                        model.Planeswalker = repository.UpdatePlaneswalker(model.Planeswalker);
                    }
                    catch(Exception e)
                    {
                        model.Errors.Add("Could not update account");
                        model.Errors.Add(e.Message);
                    }
                }

                if(Request.Form.Delete != null)
                {
                    try
                    {
                        if(model.Yes)
                        {
                            ssa.Disable(model.Planeswalker.AuthToken);
                            repository.RemovePlaneswalker(model.Planeswalker.Id);
                            return this.LogoutAndRedirect("/");
                        }
                        else
                        {
                            model.Errors.Add("You must check, 'Yes, I know'. To delete.");
                        }
                    }
                    catch(Exception e)
                    {
                        model.Errors.Add("Account could not be deleted");
                        model.Errors.Add(e.Message);
                    }
                }

                if(Request.Form.ChangePassword != null)
                {
                    if(model.Password != null && model.ConfirmPassword != null)
                    {
                        if(model.Password == model.ConfirmPassword)
                        {
                            try
                            {
                                ssa.ChangePassword(model.Planeswalker.AuthToken, model.Password);
                                model.Messages.Add("Password successfully changed.");
                            }
                            catch(Exception e)
                            {
                                model.Errors.Add("Password cannot be changed.");
                                model.Errors.Add(e.Message);
                            }  
                        }
                        else
                        {
                            model.Errors.Add("Password and Confirmation Password do not match.");
                        }
                    }
                    else
                    {
                        model.Errors.Add("Password and Confirmation Password must not be blank.");
                    }
                }

                return View["Logon/Settings",model];
            };
           
            Get["/logon"] = parameters => {
                LogonModel model = new LogonModel();
                model.ActiveMenu = "signin";
                model.UrlRedirect = (string)Request.Query.Url;

                if(Request.Query.returnUrl != null)
                {
                    model.UrlRedirect = (string)Request.Query.returnUrl;
                }

                return View["Logon/logon",model];
            };

            Post["/logon"] = parameters => {
                LogonModel model = this.Bind<LogonModel>();
                model.ActiveMenu = "signin";
                var results = this.Validate(model);

                if(!results.IsValid)
                {
                    model.Errors = ErrorUtility.GetValidationErrors(results);
                    return View["Logon/Logon", model];
                }

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

                return this.LoginAndRedirect(user.AuthToken, 
                    fallbackRedirectUrl: model.UrlRedirect);
            };

			Get ["/register"] = parameters => {
                SignupModel model = new SignupModel();
				model.ActiveMenu = "register";
				return View["register", model];
            };

			Post ["/register"] = parameters => {
                SignupModel model = this.Bind<SignupModel>();
                var result = this.Validate(model);
				model.ActiveMenu = "register";

                if (!result.IsValid)
                {
                    model.Errors.AddRange(ErrorUtility.GetValidationErrors(result));
					return View["Register", model];
                }

                try
                {
                    repository.AddPlaneswalker(model.UserName, model.Secret, model.Email);
                }
                catch(Exception e)
                {
                    model.Errors.Add(e.Message);
					return View["Register", model];
                }

                LogonModel logon = new LogonModel();
                logon.Messages.Add("You have successfully created an account. Please Sign In.");

                try
                {
                    Email.send("planeswalker@mtgdb.info", 
                        "New Planeswalker alert", model.UserName);
                }
                catch(Exception e)
                {
                    //swallow this for now
                }

                return View["Logon", logon];

            };

            Get["/logout"] = parameters => {
                Planeswalker nuser = (Planeswalker)Context.CurrentUser;
                ssa.End(nuser.AuthToken);

                return this.LogoutAndRedirect((string)Request.Query.Url);
            };

            Get ["/forgot"] = parameters => {
                ForgotModel model = new ForgotModel();
                model.ActiveMenu = "signin";
                return View["Forgot", model];
            };

            Post ["/forgot"] = parameters => {
                ForgotModel model = this.Bind<ForgotModel>();
                model.ActiveMenu = "signin";

                string subject = "MtgDb.info: Password reset request.";
                string body = "You have requested a password reset. You new password is: {0}";

                try
                {
                    string newPass = ssa.Forgot(model.Email);
                    Email.send(model.Email, subject,string.Format(body,newPass));
                    model.Messages.Add("Your new password has been successfully sent to your email.");
                }
                catch(Exception e)
                {
                    model.Errors.Add(e.Message);
                }
                    
                return View["Forgot", model];
            };
        }
    }
}

