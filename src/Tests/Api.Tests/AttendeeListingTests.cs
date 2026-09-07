using System.Net;
using System.Net.Http.Json;
using TrainingCatalog.Application;

namespace TrainingCatalog.Api.Tests;

public sealed class AttendeeListingTests
{
	[Fact]
	public async Task ReturnsEmptyCollectionWhenTrainingHasNoAttendees()
	{
		using var factory = new TrainingCatalogApiFactory();
		using var client = factory.CreateClient();
		var training = await CreateTraining(client, "2026-09-14");

		var response = await client.GetAsync($"/api/trainings/{training.Id}/attendees");

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		var attendees = await response.Content.ReadFromJsonAsync<Attendee[]>();
		Assert.Empty(attendees!);
	}

	[Fact]
	public async Task ReturnsAttendeeAfterItIsCreated()
	{
		using var factory = new TrainingCatalogApiFactory();
		using var client = factory.CreateClient();
		var training = await CreateTraining(client, "2026-09-15");
		var request = new CreateAttendeeRequest("Ana", "Silva", "ana@example.com");

		var creationResponse = await client.PostAsJsonAsync($"/api/trainings/{training.Id}/attendees", request);
		var listingResponse = await client.GetAsync($"/api/trainings/{training.Id}/attendees");

		Assert.Equal(HttpStatusCode.Created, creationResponse.StatusCode);
		Assert.Equal(HttpStatusCode.OK, listingResponse.StatusCode);
		var attendees = await listingResponse.Content.ReadFromJsonAsync<Attendee[]>();
		var attendee = Assert.Single(attendees!);
		Assert.Equal(training.Id, attendee.TrainingId);
		Assert.Equal(request.FirstName, attendee.FirstName);
		Assert.Equal(request.LastName, attendee.LastName);
		Assert.Equal(request.Email!.ToUpperInvariant(), attendee.Email);
	}

	[Fact]
	public async Task ReturnsNotFoundWhenListingAttendeesForMissingTraining()
	{
		using var factory = new TrainingCatalogApiFactory();
		using var client = factory.CreateClient();

		var response = await client.GetAsync($"/api/trainings/{Guid.NewGuid()}/attendees");

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}

	private static async Task<Training> CreateTraining(HttpClient client, string startDate)
	{
		var request = new CreateTrainingRequest("Fundamentos de C#", "Introdução ao C#", startDate, 8);
		var response = await client.PostAsJsonAsync("/api/trainings", request);

		Assert.Equal(HttpStatusCode.Created, response.StatusCode);
		return (await response.Content.ReadFromJsonAsync<Training>())!;
	}
}