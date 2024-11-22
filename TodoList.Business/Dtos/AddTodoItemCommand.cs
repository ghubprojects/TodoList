using FluentValidation;

namespace TodoList.Business.Dtos;

public sealed record AddTodoItemCommand(
    string Title,
    string? Description,
    DateTime DueDate,
    bool IsCompleted);

public sealed class AddTodoItemCommandValidator : AbstractValidator<AddTodoItemCommand> {
    public AddTodoItemCommandValidator() {
        RuleFor(command => command.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(command => command.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(command => !string.IsNullOrEmpty(command.Description));

        RuleFor(command => command.DueDate)
            .GreaterThan(DateTime.Now).WithMessage("Due date must be a future date.");

        RuleFor(command => command.IsCompleted)
            .NotNull().WithMessage("Is completed must be specified.");
    }
}