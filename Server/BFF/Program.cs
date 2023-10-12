
using BFF;

using Game;
using System.Collections;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
//builder.Logging.AddAzureWebAppDiagnostics();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();
builder.Services.AddGameServices(o => GameServiceConfiguratorAzure.ConfigGameServiceAzure(o, builder));
builder.Services.AddLogging();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
WebApplication app = builder.Build();

Console.WriteLine("================================");

foreach (DictionaryEntry kvp in System.Environment.GetEnvironmentVariables())
{
    Console.WriteLine($"{kvp.Key} = {kvp.Value}");
}



// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}




app.UseHttpsRedirection();
app.MapControllers();
app.MapGameHubs();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins separated with comma
    .WithOrigins("https://wortkette.pcfres-intra.dev.datev.de", "https://wortkette.pcfopen-intra.dev.datev.de")
    .AllowCredentials()); // allow credentials
app.Run();
