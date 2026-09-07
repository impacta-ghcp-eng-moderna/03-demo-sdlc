using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TrainingCatalog.Application;

namespace TrainingCatalog.Api.Tests;

public sealed class TrainingUpdateTests
{
    [Fact]
    public async Task ReturnsUpdatedTrainingWhenRequestIsValid()
    {
        using var factory = new TrainingCatalogApiFactory();
        using var client = factory.CreateClient();
        var createdTraining = await CreateTraining(client, "2026-09-15");
        var request = new CreateTrainingRequest(
            "C# Avançado",
            "Tópicos avançados de C#",
            "2026-09-16",
            16);

        var response = await client.PutAsJsonAsync($"/api/trainings/{createdTraining.Id}", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var training = await response.Content.ReadFromJsonAsync<Training>();
        Assert.NotNull(training);
        Assert.Equal(createdTraining.Id, training.Id);
        Assert.Equal(request.Title, training.Title);
        Assert.Equal(request.Description, training.Description);
        Assert.Equal(DateOnly.Parse(request.StartDate!), training.StartDate);
        Assert.Equal(request.DurationHours, training.DurationHours);
    }

    [Fact]
    public async Task ReturnsBadRequestWhenRequestIsInvalid()
    {
        using var factory = new TrainingCatalogApiFactory();
        using var client = factory.CreateClient();
        var request = new CreateTrainingRequest("", "Descrição", "2026-09-15", 8);

        var response = await client.PutAsJsonAsync($"/api/trainings/{Guid.NewGuid()}", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var error = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.True(error.RootElement.GetProperty("errors").TryGetProperty("title", out _));
    }

    [Fact]
    public async Task ReturnsNotFoundWhenIdentifierDoesNotExist()
    {
        using var factory = new TrainingCatalogApiFactory();
        using var client = factory.CreateClient();
        var request = new CreateTrainingRequest("Fundamentos de C#", "Introdução ao C#", "2026-09-15", 8);

        var response = await client.PutAsJsonAsync($"/api/trainings/{Guid.NewGuid()}", request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ReturnsConflictWhenStartDateBelongsToAnotherTraining()
    {
        using var factory = new TrainingCatalogApiFactory();
        using var client = factory.CreateClient();
        var firstTraining = await CreateTraining(client, "2026-09-15");
        await CreateTraining(client, "2026-09-16");
        var request = new CreateTrainingRequest("C# Avançado", "Tópicos avançados de C#", "2026-09-16", 16);

        var response = await client.PutAsJsonAsync($"/api/trainings/{firstTraining.Id}", request);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        using var error = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(
            "Já existe um treinamento com esta data de início.",
            error.RootElement.GetProperty("errors").GetProperty("startDate")[0].GetString());
    }

    private static async Task<Training> CreateTraining(HttpClient client, string startDate)
    {
        var request = new CreateTrainingRequest("Fundamentos de C#", "Introdução ao C#", startDate, 8);
        var response = await client.PostAsJsonAsync("/api/trainings", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<Training>())!;
    }
}