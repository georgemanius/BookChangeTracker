namespace BookChangeTracker.Application.Models;

public static class ErrorCodes
{
    public const string BookNotFound = "BOOK_NOT_FOUND";
    public const string AuthorNotFound = "AUTHOR_NOT_FOUND";
    public const string AuthorAlreadyAssigned = "AUTHOR_ALREADY_ASSIGNED";
    public const string AuthorNotAssigned = "AUTHOR_NOT_ASSIGNED";
    public const string InternalServerError = "INTERNAL_SERVER_ERROR";
}
