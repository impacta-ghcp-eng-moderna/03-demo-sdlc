using System.Net;
using System.Net.Http.Json;
using TrainingCatalog.Application;

namespace TrainingCatalog.Api.Tests;

public sealed class TrainingRetrievalTests
{
    [Fact]
    public async Task ReturnsTrainingWhenIdentifierExists()
    {
        using var factory = new TrainingCatalogApiFactory();
        using var client = factory.CreateClient();
        var request = new CreateTrainingRequest(
            "Fundamentos de C#",
            "Introdução ao C#",
            "2026-09-15",
            8);

        var creationResponse = await client.PostAsJsonAsync("/api/trainings", request);
        var createdTraining = await creationResponse.Content.ReadFromJsonAsync<Training>();
        var response = await client.GetAsync($"/api/trainings/{createdTraining!.Id}");

        Assert.Equal(HttpStatusCode.Created, creationResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var training = await response.Content.ReadFromJsonAsync<Training>();
        Assert.NotNull(training);
        Assert.Equal(createdTraining.Id, training.Id);
        Assert.Equal(request.Title, training.Title);
    }

    [Fact]
    public async Task ReturnsNotFoundWhenIdentifierDoesNotExist()
    {
        using var factory = new TrainingCatalogApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync($"/api/trainings/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}