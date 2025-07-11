using BlogAs.ViewModels;
using BlogAs.ViewModels.Accounts;
using FluentValidation;

namespace BlogAs.Validators;

public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
{
    public LoginViewModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}