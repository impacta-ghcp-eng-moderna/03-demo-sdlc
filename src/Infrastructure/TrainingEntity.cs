using TrainingCatalog.Application;

namespace TrainingCatalog.Infrastructure;

public sealed class TrainingEntity
{
    public Guid Id { get; set; }

    public ICollection<AttendeeEntity> Attendees { get; set; } = [];

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }

    public int DurationHours { get; set; }

    public Training ToTraining() => new(Id, Title, Description, StartDate, DurationHours);
}