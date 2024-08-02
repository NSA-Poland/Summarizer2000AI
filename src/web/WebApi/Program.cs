using Azure.AI.OpenAI;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/api/weatherforecast", async () =>
{
    var kernelBuilder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(
        deploymentName: "summarizer2000ai",
        endpoint: "https://summarizer2000ai.openai.azure.com/",
        apiKey: "aacf442553ac43be9dace87e93a94f96"
        );
    var kernel = kernelBuilder.Build();

    var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();


    var text = await File.ReadAllTextAsync("Sample.txt");

    //var result = await kernel.InvokeAsync(summarize, new() { ["input"] = text });

    // Add a plugin (the LightsPlugin class is defined below)
    //kernel.Plugins.AddFromType<LightsPlugin>("Lights");

    //var request = new CompletionRequest(prompt, new { input = inputText });
    //var result = await _kernel.Completions.CreateCompletionAsync(request);

    kernel.Plugins.AddFromType<TextSourcePlugin>("TextSourcePlugin");

    var history = new ChatHistory();
    history.AddUserMessage($"Summarize following text: {text}");

    var result = await chatCompletionService.GetChatMessageContentsAsync(
        history,
        executionSettings: new OpenAIPromptExecutionSettings
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        },
        kernel: kernel);

    return Results.Ok(result);
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();
