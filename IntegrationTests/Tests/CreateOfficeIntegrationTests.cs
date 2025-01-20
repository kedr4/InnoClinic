namespace IntegrationTests.Tests;

[Collection("OfficesCreate")]
public class CreateOfficeIntegrationTests : IClassFixture<OfficesServiceAppFactoryFixture>
{
    private readonly HttpClient _client;

    public CreateOfficeIntegrationTests(OfficesServiceAppFactoryFixture appFixture)
    {
        _client = appFixture.CreateClient();
    }

    [Fact]
    public async Task CreateOffice_ReturnsValidGuid()
    {
        // Arrange
        var createOfficeCommand = new CreateOfficeCommand(
            City: "New York",
            Street: "5th Avenue",
            HouseNumber: "10",
            OfficeNumber: "101",
            PhotoUrl: null,
            RegistryPhoneNumber: "+123456789",
            IsActive: true
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/offices", createOfficeCommand);

        response.EnsureSuccessStatusCode();
        var officeId = await response.Content.ReadFromJsonAsync<Guid>();
        officeId.Should().NotBe(Guid.Empty);

        // Assert
        var createdOffice = await _client.GetAsync($"/api/offices/{officeId}");
        createdOffice.StatusCode.Should().Be(HttpStatusCode.OK);
        var officeDtoResponseContent = await createdOffice.Content.ReadAsStringAsync();
        var officeDto = JsonSerializer.Deserialize<OfficeDTO>(officeDtoResponseContent);

        officeDto.Should().NotBeNull();
        officeDto.Street.Should().Be(createOfficeCommand.Street);
        officeDto.City.Should().Be(createOfficeCommand.City);
        officeDto.HouseNumber.Should().Be(createOfficeCommand.HouseNumber);
        officeDto.OfficeNumber.Should().Be(createOfficeCommand.OfficeNumber);
        officeDto.RegistryPhoneNumber.Should().Be(createOfficeCommand.RegistryPhoneNumber);
        officeDto.Address.Should().NotBeNull();
    }
}
