using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TrainingCatalog.Application;

namespace TrainingCatalog.Api.Tests;

public sealed class AttendeeCreationTests
{
	[Fact]
	public async Task ReturnsCreatedAttendeeAndLocationWhenRequestIsValid()
	{
		using var factory = new TrainingCatalogApiFactory();
		using var client = factory.CreateClient();
		var training = await CreateTraining(client, "2026-09-15");
		var request = new CreateAttendeeRequest("Ana", "Silva", " ana@example.com ");

		var response = await client.PostAsJsonAsync($"/api/trainings/{training.Id}/attendees", request);

		Assert.Equal(HttpStatusCode.Created, response.StatusCode);
		Assert.Equal($"/api/trainings/{training.Id}/attendees/", response.Headers.Location?.ToString()[..$"/api/trainings/{training.Id}/attendees/".Length]);
		var attendee = await response.Content.ReadFromJsonAsync<Attendee>();
		Assert.NotNull(attendee);
		Assert.NotEqual(Guid.Empty, attendee.Id);
		Assert.Equal(training.Id, attendee.TrainingId);
		Assert.Equal(request.FirstName, attendee.FirstName);
		Assert.Equal(request.LastName, attendee.LastName);
		Assert.Equal("ANA@EXAMPLE.COM", attendee.Email);
	}

	[Theory]
	[InlineData(null, "Silva", "ana@example.com", "firstName")]
	[InlineData("Ana", null, "ana@example.com", "lastName")]
	[InlineData("Ana", "Silva", null, "email")]
	public async Task ReturnsBadRequestWhenAttendeeFieldIsInvalid(
		string? firstName,
		string? lastName,
		string? email,
		string fieldName)
	{
		using var factory = new TrainingCatalogApiFactory();
		using var client = factory.CreateClient();
		var training = await CreateTraining(client, "2026-09-15");
		var request = new CreateAttendeeRequest(firstName, lastName, email);

		var response = await client.PostAsJsonAsync($"/api/trainings/{training.Id}/attendees", request);

		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		using var error = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		Assert.True(error.RootElement.GetProperty("errors").TryGetProperty(fieldName, out _));
	}

	[Fact]
	public async Task ReturnsNotFoundWhenTrainingDoesNotExist()
	{
		using var factory = new TrainingCatalogApiFactory();
		using var client = factory.CreateClient();
		var request = new CreateAttendeeRequest("Ana", "Silva", "ana@example.com");

		var response = await client.PostAsJsonAsync($"/api/trainings/{Guid.NewGuid()}/attendees", request);

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}

	[Fact]
	public async Task ReturnsConflictAndDoesNotStoreDuplicateEmailAfterNormalization()
	{
		using var factory = new TrainingCatalogApiFactory();
		using var client = factory.CreateClient();
		var training = await CreateTraining(client, "2026-09-15");
		var firstResponse = await client.PostAsJsonAsync(
			$"/api/trainings/{training.Id}/attendees",
			new CreateAttendeeRequest("Ana", "Silva", " ana@example.com "));
		var secondResponse = await client.PostAsJsonAsync(
			$"/api/trainings/{training.Id}/attendees",
			new CreateAttendeeRequest("Outra", "Pessoa", "ANA@EXAMPLE.COM"));

		Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);
		Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
		using var error = JsonDocument.Parse(await secondResponse.Content.ReadAsStringAsync());
		Assert.True(error.RootElement.GetProperty("errors").TryGetProperty("email", out _));

		var listingResponse = await client.GetAsync($"/api/trainings/{training.Id}/attendees");
		var attendees = await listingResponse.Content.ReadFromJsonAsync<Attendee[]>();
		Assert.Equal(HttpStatusCode.OK, listingResponse.StatusCode);
		Assert.NotNull(attendees);
		Assert.Single(attendees);
	}

	[Fact]
	public async Task AllowsSameEmailInDifferentTrainings()
	{
		using var factory = new TrainingCatalogApiFactory();
		using var client = factory.CreateClient();
		var firstTraining = await CreateTraining(client, "2026-09-15");
		var secondTraining = await CreateTraining(client, "2026-09-16");
		var firstResponse = await client.PostAsJsonAsync(
			$"/api/trainings/{firstTraining.Id}/attendees",
			new CreateAttendeeRequest("Ana", "Silva", "ana@example.com"));
		var secondResponse = await client.PostAsJsonAsync(
			$"/api/trainings/{secondTraining.Id}/attendees",
			new CreateAttendeeRequest("Ana", "Silva", " ANA@EXAMPLE.COM "));

		Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);
		Assert.Equal(HttpStatusCode.Created, secondResponse.StatusCode);

		var firstAttendee = await firstResponse.Content.ReadFromJsonAsync<Attendee>();
		var secondAttendee = await secondResponse.Content.ReadFromJsonAsync<Attendee>();
		Assert.NotNull(firstAttendee);
		Assert.NotNull(secondAttendee);
		Assert.NotEqual(firstAttendee.Id, secondAttendee.Id);
		Assert.Equal(firstTraining.Id, firstAttendee.TrainingId);
		Assert.Equal(secondTraining.Id, secondAttendee.TrainingId);
	}

	private static async Task<Training> CreateTraining(HttpClient client, string startDate)
	{
		var request = new CreateTrainingRequest("Fundamentos de C#", "Introdução ao C#", startDate, 8);
		var response = await client.PostAsJsonAsync("/api/trainings", request);

		Assert.Equal(HttpStatusCode.Created, response.StatusCode);
		return (await response.Content.ReadFromJsonAsync<Training>())!;
	}
}