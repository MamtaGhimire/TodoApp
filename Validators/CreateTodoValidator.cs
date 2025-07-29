using FluentValidation;
using TodoApp.DTOs;

namespace TodoApp.Validators;

public class CreateTodoValidator : AbstractValidator<CreateTodoDto>
{
    public CreateTodoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100);

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.Now).WithMessage("Due date must be in the future");
    }
}
