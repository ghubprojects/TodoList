using FluentValidation;

namespace TodoList.Business.Dtos;

public sealed record UpdateTodoItemCommand(
    int Id,
    string Title,
    string? Description,
    DateTime DueDate,
    bool IsCompleted);

public sealed class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand> {
    public UpdateTodoItemCommandValidator() {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(command => command.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(command => command.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(command => !string.IsNullOrEmpty(command.Description));

        RuleFor(command => command.DueDate)
            .GreaterThan(DateTime.Now).WithMessage("Due date must be a future date.");
    }
}