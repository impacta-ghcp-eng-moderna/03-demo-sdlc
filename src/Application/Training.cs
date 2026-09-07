namespace TrainingCatalog.Application;

public sealed record CreateTrainingRequest(
    string? Title,
    string? Description,
    string? StartDate,
    int DurationHours);

public sealed record Training(
    Guid Id,
    string Title,
    string Description,
    DateOnly StartDate,
    int DurationHours);

public sealed record CreateAttendeeRequest(
    string? FirstName,
    string? LastName,
    string? Email);

public sealed record Attendee(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    Guid TrainingId);

public static class EmailNormalizer
{
    public static string Normalize(string email) => email.Trim().ToUpperInvariant();
}