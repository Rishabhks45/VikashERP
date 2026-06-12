using FluentValidation;
using VikashERP.Application.Features.Users.DTOs;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Application.Features.Users.Validators;

public class CreateUserAccountDtoValidator : AbstractValidator<CreateUserAccountDto>
{
    public CreateUserAccountDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(role => UserRoleExtensions.FromString(role) != null)
            .WithMessage("Invalid role value. Must be a valid system role (e.g., Super Admin, Back Office User, Customer, Employee, Manager).");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");
    }
}

public class UpdateUserAccountDtoValidator : AbstractValidator<UpdateUserAccountDto>
{
    public UpdateUserAccountDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(role => UserRoleExtensions.FromString(role) != null)
            .WithMessage("Invalid role value. Must be a valid system role (e.g., Super Admin, Back Office User, Customer, Employee, Manager).");

        RuleFor(x => x.Password)
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Password));
    }
}
