using BfsApi;
using Bricknode.Soap.Sdk.Extensions;
using Bricknode_Broker_Utilities.Contracts.Settings;
using Bricknode_Broker_Utilities.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Program
{
    static async Task Main(string[] args)
    {
        var host = BuildHost();
        var menuService = host.Services.GetService<MenuService>();
        await menuService.ShowMenu();
        Console.WriteLine("Press any key to close");
        Console.ReadKey();
    }

    private static IHost BuildHost()
    {
        var builder = new HostBuilder()
            .UseEnvironment("Local")
            .ConfigureHostConfiguration(configurationBuilder => { configurationBuilder.AddEnvironmentVariables(); })
            .ConfigureAppConfiguration((hostContext, configurationBuilder) =>
            {
                configurationBuilder.AddUserSecrets<UserSecrets>();
            })
            .ConfigureServices((hostContext, services) =>
            {
                var userSecrets = hostContext.Configuration.GetSection("UserSecrets").Get<UserSecrets>();

                var clientBuilder = services.AddMultiBfsApiClient();

                foreach (var bricknodeBrokerInstance in userSecrets.BfsCredentials)
                {
                    clientBuilder.AddNamedBfsApiClient(bricknodeBrokerInstance.BfsInstanceKey, bfsApiConfiguration =>
                    {
                        bfsApiConfiguration.Credentials = new Credentials
                        {
                            UserName = bricknodeBrokerInstance.Username,
                            Password = bricknodeBrokerInstance.Password
                        };

                        bfsApiConfiguration.EndpointAddress = bricknodeBrokerInstance.ApiEndpoint;
                        bfsApiConfiguration.Identifier = bricknodeBrokerInstance.Identifier;
                    });
                }
                clientBuilder.BuildClients();
                AddServices(services, hostContext);

            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.AddConsole();
            });

        return builder.Build();
    }

    private static void AddServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddTransient<MenuService>();
        services.AddSingleton(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger("DefaultLogger"));
        services.Configure<UserSecrets>(context.Configuration.GetSection("UserSecrets"));
        services.AddOptions();

    }
}