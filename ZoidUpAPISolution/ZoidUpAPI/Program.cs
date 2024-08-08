using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ZoidUpAPI.Data;
using ZoidUpAPI.Data.Services.UserService;
using System.Text;
using AutoMapper;
using ZoidUpAPI.Utilities.Tokens_Hashers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//SignalR
builder.Services.AddSignalR();

//Authentication and httpcontext
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings:SecretKey").Value)),
            ValidateIssuer = true,
            ValidateAudience = true
        };

        opt.Events = new JwtBearerEvents
        {
            //we need this so we can use users with signal r
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddHttpContextAccessor();

//POSTGRESQL
builder.Services.AddDbContext<AppDbContext>(opt =>
opt.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);



//AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

//CORS
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
