using BlogAs.ViewModels;
using BlogAs.ViewModels.Categories;
using FluentValidation;

namespace BlogAs.Validators;

public class EditorCategoryViewModelValidator : AbstractValidator<EditorCategoryViewModel> 
{
    public EditorCategoryViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(80).WithMessage("Name must have less than 80 characters")
            .MinimumLength(3).WithMessage("Name must have more than 3 characters");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required")
            .MaximumLength(80).WithMessage("Slug must have less than 80 characters");
    }
}