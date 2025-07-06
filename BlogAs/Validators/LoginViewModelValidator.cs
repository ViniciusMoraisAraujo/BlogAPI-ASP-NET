using BlogAs.ViewModels;
using FluentValidation;

namespace BlogAs.Validators;

public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
{
    public LoginViewModelValidator()
    {
        RuleFor().
    }
}