using FluentValidation;
using VikashERP.Web.Models.Forms;

namespace VikashERP.Web.Validation;

public class UserFormModelValidator : AbstractValidator<UserFormModel>
{
    public UserFormModelValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("System role selection is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required for new accounts.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters.")
            .When(x => x.IsNewUser);

        RuleFor(x => x.Password)
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters.")
            .When(x => !x.IsNewUser && !string.IsNullOrWhiteSpace(x.Password));
    }
}
