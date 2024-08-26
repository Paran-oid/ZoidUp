using API;
using API.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure services using extension methods
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureAppServices();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();


app.ConfigureSwagger();

//configure signalR
app.MapHub<AppHub>("chathub");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
