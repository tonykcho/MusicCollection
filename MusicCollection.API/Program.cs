using MusicCollection.API.Extensions;
using MusicCollection.DataAccess.DbContaxts;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging();

builder.Services.AddDbContext<MusicCollectionDbContext>();

builder.Services.AddControllers(options => {

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

app.MapGet("/", () => "Hello World!");

await app.MigrateAsync();
 
app.MapControllers();

app.UseHttpLogging();

app.Run();