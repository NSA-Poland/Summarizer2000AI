using Azure.AI.OpenAI;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
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

app.MapPost("api/summarize", async () =>
{
    return Results.Ok("wdaawdwdadwa");
});

app.MapGet("/api/weatherforecast", async () =>
{
    var kernelBuilder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(
        deploymentName: "summarizer2000ai",
        endpoint: "https://summarizer2000ai.openai.azure.com/",
        apiKey: "aacf442553ac43be9dace87e93a94f96"
        );
    var kernel = kernelBuilder.Build();

    var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

    var prompt = @"
    You are an intelligent assistant that summarizes text. Summarize the following input text in a concise and informative manner, highlighting the main points:
 
    Input:
    {{input}}
 
    Summary:
    "
    ;

    var text = await File.ReadAllTextAsync("Sample.txt");

    var client = new OpenAIClient(new Uri("https://summarizer2000ai.openai.azure.com/"), new AzureKeyCredential("aacf442553ac43be9dace87e93a94f96"));

    // Create the request
    var opt = new CompletionsOptions("summarizer2000ai", [prompt.Replace("{{input}}", text)]);
    opt.MaxTokens  = 100;

    var summaryResult = await client.GetCompletionsAsync(opt);



    //var result = await kernel.InvokeAsync(summarize, new() { ["input"] = text });

    // Add a plugin (the LightsPlugin class is defined below)
    //kernel.Plugins.AddFromType<LightsPlugin>("Lights");

    //var request = new CompletionRequest(prompt, new { input = inputText });
    //var result = await _kernel.Completions.CreateCompletionAsync(request);


    return Results.Ok(summaryResult.Value.Choices[0].Text.Trim());
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();
