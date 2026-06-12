using FluentValidation;
using VikashERP.Web.Models.Forms;

namespace VikashERP.Web.Validation;

public class ForgotPasswordFormModelValidator : AbstractValidator<ForgotPasswordFormModel>
{
    public ForgotPasswordFormModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.")
            .MaximumLength(256);
    }
}
