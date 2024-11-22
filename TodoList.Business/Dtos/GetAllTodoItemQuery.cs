using FluentValidation;

namespace TodoList.Business.Dtos;

public sealed record GetAllTodoItemQuery(
    string? SearchText,
    bool? IsCompleted,
    string? SortColumn,
    string? SortOrder,
    int Page,
    int PageSize);

public class GetAllTodoQueryValidator : AbstractValidator<GetAllTodoItemQuery> {
    public GetAllTodoQueryValidator() {
        RuleFor(x => x.SearchText)
            .MaximumLength(100).WithMessage("Search text must not exceed 100 characters.");

        RuleFor(x => x.SortColumn)
            .Must(field => string.IsNullOrEmpty(field) || new[] { "title", "duedate", "id" }.Contains(field.ToLower()))
            .WithMessage("Sort column must be 'title', 'duedate', or 'id'.");

        RuleFor(x => x.SortOrder)
            .Must(order => string.IsNullOrEmpty(order) || new[] { "asc", "desc" }.Contains(order.ToLower()))
            .WithMessage("Sort order must be 'asc' or 'desc'.");

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
    }
}