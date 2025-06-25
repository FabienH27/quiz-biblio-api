using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.Infrastructure.Configuration;
using QuizBiblio.Infrastructure.Storage;
using QuizBiblio.IntegrationTests.Auth;
using QuizBiblio.IntegrationTests.FakeServices;
using System.Net;
using System.Net.Http.Headers;

namespace QuizBiblio.IntegrationTests;

internal class QuizBiblioApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString;
    private readonly string _databaseName;

    public QuizBiblioApplicationFactory(string connectionString, string databaseName)
    {
        _connectionString = connectionString;
        _databaseName = databaseName;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {

            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["QuizStoreDatabaseSettings:ConnectionString"] = _connectionString,
                ["QuizStoreDatabaseSettings:DatabaseName"] = _databaseName,
                ["Cookie:Secure"] = "false", //required for tokens to passed through requests
                ["Cookie:SameSite"] = "Lax"
            });
        });

        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthenticationSchemeProvider.Name;
                options.DefaultChallengeScheme = TestAuthenticationSchemeProvider.Name;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                TestAuthenticationSchemeProvider.Name,
                _ => { });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IMongoClient>();
            services.RemoveAll<IMongoDbContext>();
            services.RemoveAll<IStorageClientWrapper>();
            services.RemoveAll<IDatabaseConfigurationProvider>();

            services.AddSingleton<IDatabaseConfigurationProvider>(
                new FakeDatabaseConfigurationProvider(_connectionString, _databaseName));

            var mongoClientSettings = MongoClientSettings.FromConnectionString(_connectionString);
            mongoClientSettings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };

            var client = new MongoClient(mongoClientSettings);

            services.AddSingleton<IStorageClientWrapper, FakeStorageClientWrapper>();

            services.AddSingleton<IMongoClient>(client);
            
            //We add the actual implementation but with newly injected configuration
            services.AddSingleton<IMongoDbContext, MongoDbContext>();
        });
    }

    public HttpClient GetUnauthorizedClient()
    {
        var cookieContainerHandler  = new CookieContainerHandler();

        var client = CreateDefaultClient(cookieContainerHandler);

        client.DefaultRequestHeaders.Add("SkipAuth", "true");
        return client;
    }

    public HttpClient GetAuthorizedClient()
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthenticationSchemeProvider.Name);
        return client;
    }

    public HttpClient GetAdminClient()
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthenticationSchemeProvider.Name);
        client.DefaultRequestHeaders.Add("role", "ADMIN");
        return client;
    }
}
