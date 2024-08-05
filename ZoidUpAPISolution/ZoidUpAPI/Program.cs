using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ZoidUpAPI.Data;
using ZoidUpAPI.Data.Services.UserService;
using System.Text;
using AutoMapper;
using ZoidUpAPI.Utilities.Tokens_Hashers;
using ZoidUpAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//SignalR
builder.Services.AddSignalR();

//Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings:SecretKey").Value))
        };
    });

//PostgreSQL
builder.Services.AddDbContext<AppDbContext>(opt =>
opt.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);

//Add Cookies

//Add JWT

//AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

//Cors
builder.Services.AddCors(options =>
  options.AddPolicy("Default", builder =>
  {
      builder
      .WithOrigins("http://localhost:4200")
      .AllowAnyHeader()
      .AllowAnyMethod()
      .AllowCredentials();
  }));

//App Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<Hash>();
builder.Services.AddScoped<TokenAuth>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Default");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//Our hubs
app.MapHub<ChatHub>("/chathub");

app.Run();
