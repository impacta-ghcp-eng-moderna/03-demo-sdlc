using System.Net;
using System.Net.Http.Json;
using TrainingCatalog.Application;

namespace TrainingCatalog.Api.Tests;

public sealed class TrainingLifecycleTests
{
    [Fact]
    public async Task CreatesUpdatesAndDeletesTrainingThroughCatalogApi()
    {
        using var factory = new TrainingCatalogApiFactory();
        using var client = factory.CreateClient();
        var creationRequest = new CreateTrainingRequest(
            "Fundamentos de C#",
            "Introdução ao C#",
            "2026-09-15",
            8);

        var creationResponse = await client.PostAsJsonAsync("/api/trainings", creationRequest);

        Assert.Equal(HttpStatusCode.Created, creationResponse.StatusCode);
        var createdTraining = await creationResponse.Content.ReadFromJsonAsync<Training>();
        Assert.NotNull(createdTraining);

        var retrievalResponse = await client.GetAsync($"/api/trainings/{createdTraining.Id}");

        Assert.Equal(HttpStatusCode.OK, retrievalResponse.StatusCode);
        var retrievedTraining = await retrievalResponse.Content.ReadFromJsonAsync<Training>();
        Assert.NotNull(retrievedTraining);
        Assert.Equal(createdTraining, retrievedTraining);

        var updateRequest = new CreateTrainingRequest(
            "C# Avançado",
            "Tópicos avançados de C#",
            "2026-09-16",
            16);
        var updateResponse = await client.PutAsJsonAsync($"/api/trainings/{createdTraining.Id}", updateRequest);

        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        var updatedTraining = await updateResponse.Content.ReadFromJsonAsync<Training>();
        Assert.NotNull(updatedTraining);
        Assert.Equal(createdTraining.Id, updatedTraining.Id);
        Assert.Equal(updateRequest.Title, updatedTraining.Title);
        Assert.Equal(updateRequest.Description, updatedTraining.Description);
        Assert.Equal(DateOnly.Parse(updateRequest.StartDate!), updatedTraining.StartDate);
        Assert.Equal(updateRequest.DurationHours, updatedTraining.DurationHours);

        var updatedRetrievalResponse = await client.GetAsync($"/api/trainings/{createdTraining.Id}");

        Assert.Equal(HttpStatusCode.OK, updatedRetrievalResponse.StatusCode);
        var retrievedUpdatedTraining = await updatedRetrievalResponse.Content.ReadFromJsonAsync<Training>();
        Assert.Equal(updatedTraining, retrievedUpdatedTraining);

        var deletionResponse = await client.DeleteAsync($"/api/trainings/{createdTraining.Id}");
        var deletedRetrievalResponse = await client.GetAsync($"/api/trainings/{createdTraining.Id}");
        var missingRetrievalResponse = await client.GetAsync($"/api/trainings/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NoContent, deletionResponse.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, deletedRetrievalResponse.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, missingRetrievalResponse.StatusCode);
    }
}