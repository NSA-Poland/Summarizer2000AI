using Azure.AI.OpenAI;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using WebApi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapPost("api/summarize", async ([FromBody] TextToSummarize textToSummarize) =>
{
    var kernelBuilder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(
    deploymentName: AiOptions.DeploymentName,
    endpoint: AiOptions.Endpoint,
    apiKey: AiOptions.Key
    );
    var kernel = kernelBuilder.Build();

    var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();


    kernel.Plugins.AddFromType<TextSourcePlugin>("TextSourcePlugin");

    var history = new ChatHistory();
    history.AddUserMessage($"Summarize following text: {textToSummarize.Text}");

    var result = await chatCompletionService.GetChatMessageContentAsync(
        history,
        executionSettings: new OpenAIPromptExecutionSettings
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        },
        kernel: kernel);

    return Results.Ok(result.Content);
})
.WithName("Summarize")
.WithDescription("Summarize description")
.WithSummary("Summarize summary")
.WithOpenApi();

app.Run();


public record TextToSummarize(string Text);