using Liga_IT.Application;
using Liga_IT.Infrastructure;

namespace Liga_IT.AI.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
            
            var openAiKey = builder.Configuration["OpenAI:ApiKey"] ?? throw new InvalidOperationException("OpenAI API Key not configured");
            builder.Services.AddScoped<Liga_IT.Application.Services.AI.AIService>(provider => 
            {
                var dataService = provider.GetRequiredService<Liga_IT.Application.Services.AI.ILigaITDataService>();
                var vectorService = provider.GetRequiredService<Liga_IT.Application.Services.AI.IVectorService>();
                return new Liga_IT.Application.Services.AI.AIService(openAiKey, dataService, vectorService);
            });

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
