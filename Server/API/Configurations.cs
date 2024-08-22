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

        public static void ConfigureCors(this IApplicationBuilder app)
        {
            app.UseCors("Default");
        }
    }
}
