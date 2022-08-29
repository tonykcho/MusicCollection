using MusicCollection.API.Extensions;
using MusicCollection.API.Caching;
using MusicCollection.DataAccess.DbContaxts;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging();

builder.Services.AddDbContext<MusicCollectionDbContext>();

builder.Services.AddControllers(options =>
{

});

builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));

builder.Services.AddRedisOutputCache(options =>
{
    options.AddPolicy("ById", CacheByIdPolicy.Instance);
    // options.AddBasePolicy(policy => policy.NoCache());
});

builder.Services.AddSwaggerGen();

builder.ConfigureAuth();

builder.ConfigureDependencyInjections();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseOutputCache();

app.MapGet("/", () => "Hello World!");

await app.MigrateAsync();

app.MapControllers();

app.UseHttpLogging();

app.Run();