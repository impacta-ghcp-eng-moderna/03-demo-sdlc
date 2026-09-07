using System.Net;
using System.Net.Http.Json;
using TrainingCatalog.Application;

namespace TrainingCatalog.Api.Tests;

public sealed class TrainingDeletionTests
{
    [Fact]
    public async Task ReturnsNoContentWhenTrainingIsDeleted()
    {
        using var factory = new TrainingCatalogApiFactory();
        using var client = factory.CreateClient();
        var createdTraining = await CreateTraining(client);

        var deletionResponse = await client.DeleteAsync($"/api/trainings/{createdTraining.Id}");
        var retrievalResponse = await client.GetAsync($"/api/trainings/{createdTraining.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deletionResponse.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, retrievalResponse.StatusCode);
    }

    [Fact]
    public async Task ReturnsNotFoundWhenIdentifierDoesNotExist()
    {
        using var factory = new TrainingCatalogApiFactory();
        using var client = factory.CreateClient();

        var response = await client.DeleteAsync($"/api/trainings/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private static async Task<Training> CreateTraining(HttpClient client)
    {
        var request = new CreateTrainingRequest(
            "Fundamentos de C#",
            "Introdução ao C#",
            "2026-09-15",
            8);
        var response = await client.PostAsJsonAsync("/api/trainings", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<Training>())!;
    }
}