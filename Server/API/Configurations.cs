using API.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace API
{
    public static class Configurations
    {
        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            if (app.ApplicationServices.GetService<IHostEnvironment>().IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }
    }
}
