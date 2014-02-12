using System;
using FluentValidation;

namespace MtgDb.Info
{
    public class ForgotModel : PageModel
    {
        public string Email { get; set; }

        public ForgotModel (): base()
        {
        }
    }

    public class ForgotValidator : AbstractValidator<SignupModel>
    {
        public ForgotValidator()
        {
            RuleFor(signup => signup.Email).NotEmpty();
            RuleFor(signup => signup.Email).EmailAddress();
        }
    }
}

