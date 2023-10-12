using Game;

using Mongo2Go;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
MongoDbRunner runner = MongoDbRunner.Start(Path.Combine(desktopPath, "data", "wortkette", "bff"));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGameServices(
    config =>
        {
            config.GameCollectionName = "games";
            config.ConnectionString = runner.ConnectionString;
            config.MongoDbName = "games";
        });


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_clientAllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("https://wortkette.pcfres-intra.dev.datev.de",
                "https://wortkette.pcfopen-intra.dev.datev.de");
        });
});

WebApplication app = builder.Build();


app.UseCors("_clientAllowSpecificOrigins");

//app.UseCors(x => x
//    .AllowAnyMethod()
//    .AllowAnyHeader()
//    .SetIsOriginAllowed(origin => true) // allow any origin
//    //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins separated with comma
//    .WithOrigins("https://wortkette.pcfres-intra.dev.datev.de", "https://wortkette.pcfopen-intra.dev.datev.de")
//    .AllowCredentials()); // allow credentials

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGameHubs();
app.Run();
