using MarketPulse.Server.Services;
using MarketPulse.Server.Queries;
using MarketPulse.Server.Models;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Register HttpClient for FinnhubService
builder.Services.AddHttpClient<FinnhubService>();

// Register GraphQL services
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddSubscriptionType<Subscription>() // Register the Subscription type
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddInMemorySubscriptions();  // Necessary for enabling subscriptions

// Register background service
builder.Services.AddHostedService<StockPriceBackgroundService>();

var app = builder.Build();

// Enable CORS
app.UseCors();

// Enable WebSocket support
app.UseWebSockets();

// Configure the GraphQL endpoints
app.MapGraphQL(path: "/graphql/ui");

app.Run();
