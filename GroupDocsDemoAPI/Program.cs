namespace GroupDocsDemoAPI;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string licensePath = "wwwroot/myLicense.lic";
        using (FileStream licenseStream = File.OpenRead(licensePath))
        {
            // Set license for all products
            GroupDocs.Total.License.SetLicense(licenseStream);

            // Or set license for specific products
            // GroupDocs.Viewer.License licenseViewer = new GroupDocs.Viewer.License();
            // licenseViewer.SetLicense(licenseStream);
            //
            // GroupDocs.Conversion.License licenseConversion = new GroupDocs.Conversion.License();
            // licenseConversion.SetLicense(licenseStream);
        }

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        // builder.Services.AddSwaggerGen();

        // Add CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });

        // Register your services here
        // builder.Services.AddScoped<ConversionService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }

        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseAuthorization();
        app.MapControllers();
        app.UseStaticFiles();

        // var summaries = new[]
        // {
        //     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        // };

        // app.MapGet("/weatherforecast", () =>
        //     {
        //         var forecast = Enumerable.Range(1, 5).Select(index =>
        //                 new WeatherForecast
        //                 (
        //                     DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //                     Random.Shared.Next(-20, 55),
        //                     summaries[Random.Shared.Next(summaries.Length)]
        //                 ))
        //             .ToArray();
        //         return forecast;
        //     })
        //     .WithName("GetWeatherForecast");

        app.Run();

        // record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
        // {
        //     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        // }
    }
}