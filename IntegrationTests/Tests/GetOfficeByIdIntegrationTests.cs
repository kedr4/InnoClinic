namespace IntegrationTests.Tests;

[Collection("OfficesGetById")]

public class GetOfficeByIdIntegrationTests : IClassFixture<OfficesServiceAppFactoryFixture>
{
    private readonly HttpClient _client;

    public GetOfficeByIdIntegrationTests(OfficesServiceAppFactoryFixture appFixture)
    {
        _client = appFixture.CreateClient();
    }

    [Fact]
    public async Task GetOfficeById_ReturnsOfficeById()
    {
        // Arrange
        var testOffice = new CreateOfficeCommand(
            City: "New York",
            Street: "5th Avenue",
            HouseNumber: "500",
            OfficeNumber: "101",
            PhotoUrl: null,
            RegistryPhoneNumber: "+123456789"
        );

        var postOfficeResponse = await _client.PostAsJsonAsync("/api/offices", testOffice);
        postOfficeResponse.EnsureSuccessStatusCode();

        var responseContent = await postOfficeResponse.Content.ReadAsStringAsync();
        var officeId = JsonSerializer.Deserialize<Guid>(responseContent);

        var insertedOfficeResponse = await _client.GetAsync($"/api/offices/{officeId}");
        insertedOfficeResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var finalOffice = await insertedOfficeResponse.Content.ReadFromJsonAsync<OfficeDTO>();
        finalOffice.Should().NotBeNull();

        // Act
        var getOfficeResponse = await _client.GetAsync($"/api/offices/{officeId}");
        getOfficeResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var officeDtoResponseContent = await getOfficeResponse.Content.ReadAsStringAsync();
        var officeDto = JsonSerializer.Deserialize<OfficeDTO>(officeDtoResponseContent);

        // Assert
        officeDto.Should().NotBeNull();
        officeDto.City.Should().Be(testOffice.City);
        officeDto.Street.Should().Be(testOffice.Street);
        officeDto.HouseNumber.Should().Be(testOffice.HouseNumber);
        officeDto.OfficeNumber.Should().Be(testOffice.OfficeNumber);
        officeDto.RegistryPhoneNumber.Should().Be(testOffice.RegistryPhoneNumber);
        officeDto.IsActive.Should().Be(testOffice.IsActive);
    }
}
