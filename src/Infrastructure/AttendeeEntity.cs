using TrainingCatalog.Application;

namespace TrainingCatalog.Infrastructure;

public sealed class AttendeeEntity
{
	public Guid Id { get; set; }

	public string FirstName { get; set; } = string.Empty;

	public string LastName { get; set; } = string.Empty;

	public string Email { get; set; } = string.Empty;

	public Guid TrainingId { get; set; }

	public TrainingEntity Training { get; set; } = null!;

	public Attendee ToAttendee() => new(Id, FirstName, LastName, Email, TrainingId);
}