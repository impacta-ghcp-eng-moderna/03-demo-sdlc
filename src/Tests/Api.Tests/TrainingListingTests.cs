using System.Net;
using System.Net.Http.Json;
using TrainingCatalog.Application;

namespace TrainingCatalog.Api.Tests;

public sealed class TrainingListingTests
{
    [Fact]
    public async Task ReturnsEmptyCollectionWhenCatalogHasNoTrainings()
    {
        using var factory = new TrainingCatalogApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/trainings");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var trainings = await response.Content.ReadFromJsonAsync<Training[]>();
        Assert.NotNull(trainings);
        Assert.Empty(trainings);
    }

    [Fact]
    public async Task ReturnsTrainingAfterItIsCreated()
    {
        using var factory = new TrainingCatalogApiFactory();
        using var client = factory.CreateClient();
        var request = new CreateTrainingRequest(
            "Fundamentos de C#",
            "Introdução ao C#",
            "2026-09-15",
            8);

        var creationResponse = await client.PostAsJsonAsync("/api/trainings", request);
        var listingResponse = await client.GetAsync("/api/trainings");

        Assert.Equal(HttpStatusCode.Created, creationResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, listingResponse.StatusCode);

        var trainings = await listingResponse.Content.ReadFromJsonAsync<Training[]>();
        var training = Assert.Single(trainings!);
        Assert.Equal(request.Title, training.Title);
        Assert.Equal(request.Description, training.Description);
        Assert.Equal(DateOnly.Parse(request.StartDate!), training.StartDate);
        Assert.Equal(request.DurationHours, training.DurationHours);
    }
}