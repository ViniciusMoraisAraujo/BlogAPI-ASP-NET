using BlogAs.Data;
using BlogAs.ViewModels;
using BlogAs.ViewModels.Accounts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAs.Validators;

public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
{

    public RegisterViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(2).WithMessage("Name length must be greater than 2 characters")
            .MaximumLength(80).WithMessage("Name maximum length than 80 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");
    }
}