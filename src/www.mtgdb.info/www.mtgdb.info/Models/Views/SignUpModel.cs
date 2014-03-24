using System;
using FluentValidation;

namespace MtgDb.Info
{
    public class SignupModel : PageModel
    {
        public string UserName      { get; set; }
        public string Email         { get; set; }
        public string Secret        { get; set; }
        public string ConfirmSecret { get; set; }

        public SignupModel () : base(){}
    }

    public class SignupValidator : AbstractValidator<SignupModel>
    {
        public SignupValidator()
        {
            RuleFor(signup => signup.UserName).NotEmpty();
            RuleFor(signup => signup.Email).NotEmpty();
            RuleFor(signup => signup.Email).EmailAddress();
            RuleFor(signup => signup.Secret).NotEmpty();
            RuleFor(signup => signup.ConfirmSecret).Must((signup, confirmSecret) => 
                confirmSecret == signup.Secret).WithMessage("Password and Confirmation password do not match.");
        }
    }
}

