using FluentValidation;
using VikashERP.Web.Models.Forms;

namespace VikashERP.Web.Validation;

public class ResetPasswordFormModelValidator : AbstractValidator<ResetPasswordFormModel>
{
    public ResetPasswordFormModelValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Please confirm your password.")
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match.");
    }
}
