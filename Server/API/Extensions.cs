﻿using API.Data.Services.AuthService;
using API.Data;
using API.Utilities.Tokens_Hashers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Data.Services.RequestFriendshipService;
using API.Data.Services.MessageService;
using API.Models;
using API.Data.Services.FriendshipService;

namespace API
{
    public static class Extensions
    {
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings:SecretKey").Value)),
                };
            });

        }

        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
                opt.UseNpgsql(configuration.GetConnectionString("Default")));
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddPolicy("Default", builder =>
                {
                    builder
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                }));
        }

        public static void ConfigureAppServices(this IServiceCollection services)
        {
            services.AddScoped<IRequestFriendshipService, RequestFriendshipService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IFriendshipService, FriendshipService>();
            services.AddScoped<Hash>();
            services.AddScoped<TokenAuth>();
        }
    }
}