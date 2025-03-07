using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;
using PeikkoPrecastWallDesigner.Application.Common.DTOs;
using PeikkoPrecastWallDesigner.Application.Computations.DTOs;

namespace PeikkoPrecastWallDesigner.Api.Tests;

public class ComputationControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private static Task<ComputingResultDto>? InitializeId;
    private ComputingResultDto Returnvalue = new ComputingResultDto();

    public ComputationControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        InitializeId ??= ComputeLayerLoads_ReturnOkResult(); // Ensure only one initialization
    }

    [Fact]
    public async Task TestEndpoint_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("/api/computations/test");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Success", content);
    }

    private async Task<ComputingResultDto> ComputeLayerLoads_ReturnOkResult()
    {
        var layersDto = new LayersDto
        {
            InternalLayer = new LayerDto { Name = "InternalTest", X = 1, Y = 1, Width = 1, Height = 1, Thickness = 1 },
            ExternalLayer = new LayerDto { Name = "ExternalTest", X = 1, Y = 1, Width = 1, Height = 1, Thickness = 1 },
            InsulatedLayerThickness = 90,
            Hole = new HoleDto { Name = "bothTest", X = 1, Y = 1, Width = 1, Height = 1, Position = "External" }
        };

        var response = await _client.PostAsJsonAsync("/api/computations/loads", layersDto);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<ComputingResultDto>();
        Assert.NotNull(result);
        Assert.Equal("Processing", result.Status);

        Console.WriteLine("ComputeResultId: " + result.Id);

        return result;
    }

    //[Fact]
    [Fact(Skip = "Skipping this test temporarily due to an issue.")]
    public async Task UpdateValue()
    {
        Returnvalue = await InitializeId; // Ensure ID is initialized
        var updateDto = new ComputingResultDto
        {
            Id = Returnvalue.Id,
            Value = "[{\"Name\":\"InternalTest2\",\"SurfaceArea\":0,\"Volume\":0,\"WeightKg\":0,\"WeightKn\":0},{\"Name\":\"ExternalTest2\",\"SurfaceArea\":0,\"Volume\":0,\"WeightKg\":0,\"WeightKn\":0}]",
            Status = "Completed",
        };

        var patchResponse = await _client.PatchAsJsonAsync("/api/computations/loads", updateDto);
        Assert.Equal(HttpStatusCode.OK, patchResponse.StatusCode);

        Console.WriteLine("UpdateResult: " + Returnvalue.Id);
    }

    [Fact]
    public async Task GetLoadResult_ReturnOkResult()
    {
        Returnvalue = await InitializeId; // Ensure ID is initialized
        var getResponse = await _client.GetAsync($"/api/computations/loads/{Returnvalue.Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var getResult = await getResponse.Content.ReadFromJsonAsync<ComputingResultDto>();
        Assert.NotNull(getResult);
        Assert.Equal(Returnvalue.Id, getResult.Id);
        Assert.Equal("Completed", getResult.Status);

        Console.WriteLine("GetResultId: " + getResult.Id);
        Console.WriteLine("GetResultValue: " + getResult.Value);
    }
}

