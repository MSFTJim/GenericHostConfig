using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Main program logic at the top for clarity
IConfiguration configuration = SetupConfiguration(args);

// Write directly to console
Console.WriteLine("Console app is running...");

// Access and display secret values
var apiKey = configuration?["SampleSecret:ApiKey"] ?? "Not found";
Console.WriteLine($"The API key from secrets: {apiKey}");

var dog = configuration?["Dog"] ?? "Not found";
Console.WriteLine($"Dog name: {dog}");

// Access other configuration values from appsettings.json
var appName = configuration?["AppName"] ?? "Generic Host App";
Console.WriteLine($"Application name: {appName}");

// Keep the app running until manually closed
// Console.WriteLine("\nPress any key to exit...");
// Console.ReadKey();

// Method containing the configuration setup logic
static IConfiguration SetupConfiguration(string[] args)
{
    // Create a host builder and explicitly set the environment to Development
    var builder = Host.CreateDefaultBuilder(args)
        .UseEnvironment("Development");

    // Configure configuration to include user secrets
    builder.ConfigureAppConfiguration((hostContext, config) =>
    {
        // Add user secrets when in development environment
        if (hostContext.HostingEnvironment.IsDevelopment())
        {
            config.AddUserSecrets<Program>();
        }
    });

    // Build the host
    var host = builder.Build();

    // Access configuration directly without DI and ensure it's not null
    return host.Services.GetRequiredService<IConfiguration>();
}
