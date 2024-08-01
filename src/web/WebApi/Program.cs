using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/api/weatherforecast", () =>
{
    var kernelBuilder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(
        deploymentName: "summarizer2000ai",
        endpoint: "https://summarizer2000ai.openai.azure.com/",
        apiKey: "aacf442553ac43be9dace87e93a94f96"
        );
    var kernel = kernelBuilder.Build();

    var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

    // Add a plugin (the LightsPlugin class is defined below)
    //kernel.Plugins.AddFromType<LightsPlugin>("Lights");
    return Results.Ok();
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();
