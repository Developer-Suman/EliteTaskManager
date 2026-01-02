
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Project.BLL;
using Project.BLL.Abstraction;
using Project.BLL.Services.Interface;
using Project.DLL;
using Project.DLL.DbContext;
using Project.DLL.Seed;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using Web.Configs;
using Web.CustomHealthChecks;
using Web.CustomHealthChecks.HealthChecksEndPoints;

try
{
    var builder = WebApplication.CreateBuilder(args);

    ConfigurationManager configuration = builder.Configuration;
    builder.Services
        .AddDAL(configuration)
        .AddBLL();






    builder.Services.AddHttpClient();

    builder.Services.AddHealthChecks()
        .AddCheck<ApiHealthchecks>(nameof(ApiHealthchecks))
        .AddDbContextCheck<ApplicationDbContext>();


    // Register ApiEndpointsConfig from appsettings.json
    builder.Services.Configure<HealthChecksEndPointConfig>(builder.Configuration.GetSection("Endpoints"));

    // Register a singleton instance of HttpClient
    builder.Services.AddHttpClient<ApiHealthchecks>();


    builder.Services.AddDistributedMemoryCache(); // Required for session state
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
        options.Cookie.HttpOnly = true; // Make session cookie accessible only via HTTP requests
        options.Cookie.IsEssential = true; // Marks session cookie as essential
    });


    //Configure RateLimiter
    builder.Services.AddRateLimiter(config =>
    {
        config.AddFixedWindowLimiter("FixedWindowPolicy", options =>
        {
            options.Window = TimeSpan.FromSeconds(2);
            options.PermitLimit = 3;
            options.QueueLimit = 1;
            options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;

        }).RejectionStatusCode = 429;
    });








    builder.Services.AddRateLimiter(config =>
    {
        config.AddSlidingWindowLimiter("SlidingWindowPolicy", options =>
        {
            options.Window = TimeSpan.FromSeconds(15);
            options.PermitLimit = 3;
            options.QueueLimit = 2;
            options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
            options.SegmentsPerWindow = 3;
        }).RejectionStatusCode = 429;

    });

    builder.Services.AddRateLimiter(config =>
    {
        config.AddTokenBucketLimiter("TokenBucketPolicy", options =>
        {
            options.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
            options.TokenLimit = 3;
            options.QueueLimit = 2;
            options.TokensPerPeriod = 2;
            options.AutoReplenishment = true;
            options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        }).RejectionStatusCode = 429;
    });


    builder.Services.AddRateLimiter(config =>
    {
        config.AddConcurrencyLimiter("ConcurrencyPolicy", options =>
        {
            options.PermitLimit = 3;
            options.QueueLimit = 0;
            options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;

        }).RejectionStatusCode = 429;
    });

    // Add services to the container.
    Dependencies.Inject(builder);

    Log.Information("Application StartUp");

    var app = builder.Build();
    //app.UseRouting();
    app.UseRateLimiter();

    app.MapHealthChecks("/healthcheck", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
  
    });
    app.MapHealthChecksUI(options => options.UIPath = "/dashboard");


    // Apply migrations during application startup
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();  // Apply any pending migrations
    }


    using (var scope = app.Services.CreateScope())
    {
        var dataSeeder = scope.ServiceProvider.GetRequiredService<IinitializeRepository>();
        await dataSeeder.ControllerActionSeeder();
    }


    ApplicationConfiguration.Configure(app); //Configurations

    app.Run();

}
catch (Exception ex)
{
    Log.Error("The following {Exception} was thrown during Application Startup", ex);
}
finally
{
    Log.CloseAndFlush();
}
