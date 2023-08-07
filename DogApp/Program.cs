using DogApp;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IDogService, DogService>();
builder.Services.Configure<DogApiOptions>(builder.Configuration.GetSection("DogApiOptions"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Development",
        builder =>
        {
            builder.WithOrigins("https://localhost:7244")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });

    options.AddPolicy("Production",
        builder =>
        {
            builder.WithOrigins("https://white-ground-029633203.3.azurestaticapps.net")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("Development");
}
else
{
    app.UseCors("Production");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
