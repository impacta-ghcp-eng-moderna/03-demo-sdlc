using System.Globalization;
using Microsoft.EntityFrameworkCore;
using TrainingCatalog.Application;
using TrainingCatalog.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TrainingCatalogDbContext>(options =>
	options.UseSqlite(builder.Configuration.GetConnectionString("TrainingCatalog")));
builder.Services.AddCors(options =>
	options.AddPolicy("ClientDevelopment", policy => policy
		.WithOrigins(
			"http://localhost:5152",
			"http://127.0.0.1:5152",
			"https://localhost:7240",
			"https://127.0.0.1:7240")
		.AllowAnyHeader()
		.AllowAnyMethod()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseHttpsRedirection();
}

if (app.Environment.IsDevelopment())
{
	app.UseCors("ClientDevelopment");
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapPost("/api/trainings", async (CreateTrainingRequest request, TrainingCatalogDbContext dbContext) =>
{
	var errors = new Dictionary<string, string[]>();

	if (string.IsNullOrWhiteSpace(request.Title))
	{
		errors["title"] = ["O título é obrigatório."];
	}

	if (string.IsNullOrWhiteSpace(request.Description))
	{
		errors["description"] = ["A descrição é obrigatória."];
	}

	var startDate = default(DateOnly);

	if (string.IsNullOrWhiteSpace(request.StartDate) ||
		!DateOnly.TryParseExact(request.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
	{
		errors["startDate"] = ["A data de início deve ser informada no formato YYYY-MM-DD."];
	}

	if (request.DurationHours <= 0)
	{
		errors["durationHours"] = ["A carga horária deve ser maior que zero."];
	}

	if (errors.Count > 0)
	{
		return Results.BadRequest(new { errors });
	}

	var training = new TrainingEntity
	{
		Id = Guid.NewGuid(),
		Title = request.Title!,
		Description = request.Description!,
		StartDate = startDate,
		DurationHours = request.DurationHours
	};

	dbContext.Trainings.Add(training);

	try
	{
		await dbContext.SaveChangesAsync();
	}
	catch (DbUpdateException)
	{
		return Results.Conflict(new
		{
			errors = new Dictionary<string, string[]>
			{
				["startDate"] = ["Já existe um treinamento com esta data de início."]
			}
		});
	}

	var response = training.ToTraining();
	return Results.Created($"/api/trainings/{response.Id}", response);
})
	.Produces<Training>(StatusCodes.Status201Created)
	.Produces(StatusCodes.Status400BadRequest)
	.Produces(StatusCodes.Status409Conflict);

app.MapPost("/api/trainings/{trainingId:guid}/attendees", async (Guid trainingId, CreateAttendeeRequest request, TrainingCatalogDbContext dbContext) =>
{
	var errors = new Dictionary<string, string[]>();

	if (string.IsNullOrWhiteSpace(request.FirstName))
	{
		errors["firstName"] = ["O nome é obrigatório."];
	}

	if (string.IsNullOrWhiteSpace(request.LastName))
	{
		errors["lastName"] = ["O sobrenome é obrigatório."];
	}

	if (string.IsNullOrWhiteSpace(request.Email))
	{
		errors["email"] = ["O e-mail é obrigatório."];
	}

	if (errors.Count > 0)
	{
		return Results.BadRequest(new { errors });
	}

	var trainingExists = await dbContext.Trainings.AnyAsync(training => training.Id == trainingId);

	if (!trainingExists)
	{
		return Results.NotFound();
	}

	var normalizedEmail = EmailNormalizer.Normalize(request.Email!);
	var emailAlreadyRegistered = await dbContext.Attendees.AnyAsync(attendee =>
		attendee.TrainingId == trainingId && attendee.Email == normalizedEmail);

	if (emailAlreadyRegistered)
	{
		return Results.Conflict(new
		{
			errors = new Dictionary<string, string[]>
			{
				["email"] = ["Este e-mail já está inscrito neste treinamento."]
			}
		});
	}

	var attendee = new AttendeeEntity
	{
		Id = Guid.NewGuid(),
		FirstName = request.FirstName!,
		LastName = request.LastName!,
		Email = normalizedEmail,
		TrainingId = trainingId
	};

	dbContext.Attendees.Add(attendee);

	try
	{
		await dbContext.SaveChangesAsync();
	}
	catch (DbUpdateException)
	{
		return Results.Conflict(new
		{
			errors = new Dictionary<string, string[]>
			{
				["email"] = ["Este e-mail já está inscrito neste treinamento."]
			}
		});
	}

	var response = attendee.ToAttendee();
	return Results.Created($"/api/trainings/{trainingId}/attendees/{response.Id}", response);
})
	.Produces<Attendee>(StatusCodes.Status201Created)
	.Produces(StatusCodes.Status400BadRequest)
	.Produces(StatusCodes.Status404NotFound)
	.Produces(StatusCodes.Status409Conflict);

app.MapGet("/api/trainings/{trainingId:guid}/attendees", async (Guid trainingId, TrainingCatalogDbContext dbContext) =>
{
	var trainingExists = await dbContext.Trainings.AnyAsync(training => training.Id == trainingId);

	if (!trainingExists)
	{
		return Results.NotFound();
	}

	var attendees = await dbContext.Attendees
		.AsNoTracking()
		.Where(attendee => attendee.TrainingId == trainingId)
		.Select(attendee => attendee.ToAttendee())
		.ToArrayAsync();

	return Results.Ok(attendees);
})
	.Produces<IReadOnlyCollection<Attendee>>(StatusCodes.Status200OK)
	.Produces(StatusCodes.Status404NotFound);

app.MapGet("/api/trainings", async (TrainingCatalogDbContext dbContext) =>
{
	var trainings = await dbContext.Trainings
		.AsNoTracking()
		.Select(training => training.ToTraining())
		.ToArrayAsync();

	return Results.Ok(trainings);
})
	.Produces<IReadOnlyCollection<Training>>(StatusCodes.Status200OK);

app.MapGet("/api/trainings/{id:guid}", async (Guid id, TrainingCatalogDbContext dbContext) =>
{
	var training = await dbContext.Trainings.AsNoTracking().SingleOrDefaultAsync(training => training.Id == id);
	return training is null ? Results.NotFound() : Results.Ok(training.ToTraining());
})
	.Produces<Training>(StatusCodes.Status200OK)
	.Produces(StatusCodes.Status404NotFound);

app.MapPut("/api/trainings/{id:guid}", async (Guid id, CreateTrainingRequest request, TrainingCatalogDbContext dbContext) =>
{
	var errors = new Dictionary<string, string[]>();

	if (string.IsNullOrWhiteSpace(request.Title))
	{
		errors["title"] = ["O título é obrigatório."];
	}

	if (string.IsNullOrWhiteSpace(request.Description))
	{
		errors["description"] = ["A descrição é obrigatória."];
	}

	var startDate = default(DateOnly);

	if (string.IsNullOrWhiteSpace(request.StartDate) ||
		!DateOnly.TryParseExact(request.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
	{
		errors["startDate"] = ["A data de início deve ser informada no formato YYYY-MM-DD."];
	}

	if (request.DurationHours <= 0)
	{
		errors["durationHours"] = ["A carga horária deve ser maior que zero."];
	}

	if (errors.Count > 0)
	{
		return Results.BadRequest(new { errors });
	}

	var training = await dbContext.Trainings.SingleOrDefaultAsync(training => training.Id == id);

	if (training is null)
	{
		return Results.NotFound();
	}

	training.Title = request.Title!;
	training.Description = request.Description!;
	training.StartDate = startDate;
	training.DurationHours = request.DurationHours;

	try
	{
		await dbContext.SaveChangesAsync();
	}
	catch (DbUpdateException)
	{
		return Results.Conflict(new
		{
			errors = new Dictionary<string, string[]>
			{
				["startDate"] = ["Já existe um treinamento com esta data de início."]
			}
		});
	}

	return Results.Ok(training.ToTraining());
})
	.Produces<Training>(StatusCodes.Status200OK)
	.Produces(StatusCodes.Status400BadRequest)
	.Produces(StatusCodes.Status404NotFound)
	.Produces(StatusCodes.Status409Conflict);

app.MapDelete("/api/trainings/{id:guid}", async (Guid id, TrainingCatalogDbContext dbContext) =>
{
	var training = await dbContext.Trainings.SingleOrDefaultAsync(training => training.Id == id);

	if (training is null)
	{
		return Results.NotFound();
	}

	dbContext.Trainings.Remove(training);
	await dbContext.SaveChangesAsync();
	return Results.NoContent();
})
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

app.Run();

public partial class Program;
