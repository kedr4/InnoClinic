namespace IntegrationTests.Tests;

[Collection("OfficesChangeStatus")]
public class ChangeOfficeStatusIntegrationTests : IClassFixture<OfficesServiceAppFactoryFixture>
{
    private readonly HttpClient _client;

    public ChangeOfficeStatusIntegrationTests(OfficesServiceAppFactoryFixture appFixture)
    {
        _client = appFixture.CreateClient();
    }

    [Fact]
    public async Task ChangeOfficeStatus_ReturnsOkAndUpdatesStatus()
    {
        // Arrange
        var createOfficeCommand = new CreateOfficeCommand(
            City: "York",
            Street: "5th Avenue",
            HouseNumber: "10",
            OfficeNumber: "101",
            PhotoUrl: null,
            RegistryPhoneNumber: "+123456789",
            IsActive: true
        );

        var response = await _client.PostAsJsonAsync("/api/offices", createOfficeCommand);
        response.IsSuccessStatusCode.Should().BeTrue();
        var officeId = await response.Content.ReadFromJsonAsync<Guid>();

        // Act
        var changeStatusResponse = await _client.PutAsync($"/api/offices/{officeId}/{false}", null);
        changeStatusResponse.IsSuccessStatusCode.Should().BeTrue();

        // Assert
        var finalOfficeResponse = await _client.GetAsync($"/api/offices/{officeId}");
        finalOfficeResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var finalOffice = await finalOfficeResponse.Content.ReadFromJsonAsync<OfficeDTO>();
        finalOffice.Should().NotBeNull();
        finalOffice.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task ChangeOfficeStatus_WhenOfficeNotFound_ReturnsNotFound()
    {
        // Act
        var nonExistentOfficeId = Guid.NewGuid();
        var changeStatusResponse = await _client.PutAsync($"/api/offices/{nonExistentOfficeId}/{true}", null);

        // Assert
        changeStatusResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
