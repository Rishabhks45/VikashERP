using FluentValidation;
using VikashERP.Web.Models.Forms;

namespace VikashERP.Web.Validation;

public class LoginFormModelValidator : AbstractValidator<LoginFormModel>
{
    public LoginFormModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .MaximumLength(256);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}
