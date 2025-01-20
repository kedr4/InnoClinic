using Business.Offices.Commands.UpdateOffice;

namespace IntegrationTests.Tests
{
    [Collection("OfficesUpdate")]
    public class UpdateOfficeIntegrationTests : IClassFixture<OfficesServiceAppFactoryFixture>
    {
        private readonly HttpClient _client;

        public UpdateOfficeIntegrationTests(OfficesServiceAppFactoryFixture appFixture)
        {
            _client = appFixture.CreateClient();
        }

        [Fact]
        public async Task UpdateOffice_ReturnsOkAndUpdatesOfficeDetails()
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
            var createResponse = await _client.PostAsJsonAsync("/api/offices", createOfficeCommand);
            createResponse.IsSuccessStatusCode.Should().BeTrue();
            var officeId = await createResponse.Content.ReadFromJsonAsync<Guid>();

            var updateOfficeCommand = new UpdateOfficeCommand(
                Id: officeId,
                City: "York Updated",
                Street: "5th Avenue Updated",
                HouseNumber: "20",
                OfficeNumber: "202",
                PhotoUrl: null,
                RegistryPhoneNumber: "+987654321",
                IsActive: false
            );

            // Act
            var updateResponse = await _client.PutAsJsonAsync("/api/offices", updateOfficeCommand);
            updateResponse.IsSuccessStatusCode.Should().BeTrue();

            // Assert
            var updatedOfficeResponse = await _client.GetAsync($"/api/offices/{officeId}");
            updatedOfficeResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updatedOffice = await updatedOfficeResponse.Content.ReadFromJsonAsync<OfficeDTO>();

            updatedOffice.Should().NotBeNull();
            updatedOffice.City.Should().Be(updateOfficeCommand.City);
            updatedOffice.Street.Should().Be(updateOfficeCommand.Street);
            updatedOffice.HouseNumber.Should().Be(updateOfficeCommand.HouseNumber);
            updatedOffice.OfficeNumber.Should().Be(updateOfficeCommand.OfficeNumber);
            updatedOffice.PhotoUrl.Should().Be(updateOfficeCommand.PhotoUrl);
            updatedOffice.RegistryPhoneNumber.Should().Be(updateOfficeCommand.RegistryPhoneNumber);
            updatedOffice.IsActive.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateOffice_WhenOfficeNotFound_ReturnsNotFound()
        {
            // Arrange
            var updateOfficeCommand = new UpdateOfficeCommand(
                Id: Guid.NewGuid(),
                City: "Nonexistent City",
                Street: "Nonexistent Street",
                HouseNumber: "99",
                OfficeNumber: "999",
                PhotoUrl: null,
                RegistryPhoneNumber: "+000000000",
                IsActive: true
            );

            // Act
            var updateResponse = await _client.PutAsJsonAsync("/api/offices", updateOfficeCommand);

            // Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
