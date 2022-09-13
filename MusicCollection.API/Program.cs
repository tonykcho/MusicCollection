using MusicCollection.API.Extensions;
using MusicCollection.API.Caching;
using MusicCollection.DataAccess.DbContaxts;
using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging();

builder.Services.AddDbContext<MusicCollectionDbContext>();

builder.Services.AddControllers(options =>
{
    // options.Filters.Add<AuthorizeFilter>();
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));

builder.Services.AddRedisOutputCache(options =>
{
    options.AddPolicy("ById", CacheByIdPolicy.Instance);
    // options.AddBasePolicy(policy => policy.NoCache());
});

builder.Services.AddDataProtection();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen();

builder.ConfigureAuth();

builder.ConfigureDependencyInjections();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Middlewares
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares
app.UseOutputCache();

app.MapGet("/", () => "Hello World!");

await app.MigrateAsync();

app.MapControllers();

app.UseHttpLogging();

app.Run();