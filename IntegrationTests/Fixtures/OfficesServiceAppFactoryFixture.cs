using DataAccess.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MongoDb;
using Program = Presentation.Program;

namespace IntegrationTests.Fixtures;

public class OfficesServiceAppFactoryFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MongoDbContainer mongoDbContainer = new MongoDbBuilder()
        .WithImage("mongo:latest")
        .Build();


    public async Task InitializeAsync()
    {
        await mongoDbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await mongoDbContainer.DisposeAsync();
        await base.DisposeAsync();

    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        //builder.ConfigureAppConfiguration((context, config) =>
        //{
        //    Environment.SetEnvironmentVariable("Test_MongoDbConnectionString", mongoDbContainer.GetConnectionString());
        //});

        var mongoOptions = new MongoDBSettings
        {
            CollectionName = "Offices",
            DatabaseName = "OfficesServiceDB",
            ConnectionString = mongoDbContainer.GetConnectionString()
        };

        builder.ConfigureServices(services =>
        {
            services.AddSingleton(mongoOptions);
        });
    }
}
