using Business.Exceptions;
using DataAccess.Models;

namespace IntegrationTests.Tests;

[Collection("OfficesGet")]
public class GetOfficesIntegrationTests : IClassFixture<OfficesServiceAppFactoryFixture>
{
    private readonly HttpClient _client;

    public GetOfficesIntegrationTests(OfficesServiceAppFactoryFixture appFixture)
    {
        _client = appFixture.CreateClient();
    }


    [Fact]
    public async Task GetAllOffices_ReturnsAllOffices()
    {
        // Arrange
        var testOffices = GenerateOffices(10);

        foreach (var office in testOffices)
        {
            var createOfficeCommand = new CreateOfficeCommand(
                City: office.City,
                Street: office.Street,
                HouseNumber: office.HouseNumber,
                OfficeNumber: office.OfficeNumber,
                PhotoUrl: office.PhotoUrl,
                RegistryPhoneNumber: office.RegistryPhoneNumber,
                IsActive: office.IsActive
            );

            var getResponse = await _client.PostAsJsonAsync("/api/offices", createOfficeCommand);
            getResponse.EnsureSuccessStatusCode();
        }

        // Act
        var response = await _client.GetAsync("/api/offices/1/10");
        response.EnsureSuccessStatusCode();

        var contentStream = await response.Content.ReadAsStreamAsync();
        var offices = await JsonSerializer.DeserializeAsync<List<OfficeDTO>>(contentStream);

        // Assert
        offices.Should().NotBeNull();
        offices.Count.Should().Be(10);

        foreach (var office in offices)
        {
            var result = offices.FirstOrDefault(o => o.Id == office.Id);
            result.Should().NotBeNull();
            result.City.Should().Be(office.City);
            result.Street.Should().Be(office.Street);
        }
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(-4, -2)]
    [InlineData(-123456, -4)]
    [InlineData(-3, 2)]
    [InlineData(3, -2)]
    public async Task GetAllOffices_ReturnsValidationError_WhenGivenInvalidPage(int page, int pageSize)
    {
        // Act
        var response = await _client.GetAsync($"/api/offices/{page}/{pageSize}");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var contentStream = await response.Content.ReadAsStringAsync();
        var errorResponse = JsonSerializer.Deserialize<ValidationAppException>(contentStream);

        // Asert
        errorResponse.Should().NotBeNull();
        errorResponse.Message.Should().Be("Validation failed");
    }

    private List<Office> GenerateOffices(int count, bool includeInactive = false)
    {
        var faker = new Faker<Office>()
            .RuleFor(o => o.Id, f => Guid.CreateVersion7())
            .RuleFor(o => o.City, f => f.Address.City())
            .RuleFor(o => o.Street, f => f.Address.StreetName())
            .RuleFor(o => o.HouseNumber, f => f.Random.Number(1, 100).ToString())
            .RuleFor(o => o.OfficeNumber, f => f.Random.Number(1, 100).ToString())
            .RuleFor(o => o.Address, (f, o) => $"{o.City}, {o.Street} {o.HouseNumber}, office No.{o.OfficeNumber}")
            .RuleFor(o => o.PhotoUrl, f => null)
            .RuleFor(o => o.RegistryPhoneNumber, f => "+123456789")
            .RuleFor(o => o.IsActive, f => includeInactive ? f.Random.Bool() : true);

        return faker.Generate(count);
    }
}
