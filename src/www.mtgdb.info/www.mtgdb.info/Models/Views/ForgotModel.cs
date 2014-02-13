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

    public class ForgotValidator : AbstractValidator<ForgotModel>
    {
        public ForgotValidator()
        {
            RuleFor(f => f.Email).NotEmpty();
            RuleFor(f => f.Email).EmailAddress();
        }
    }
}

