﻿using Azure;
using Azure.AI.OpenAI;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace WebApi;

public class TextSourcePlugin
{
    [KernelFunction("generate_summary")]
    [Description("Generate a summary of the input text.")]
    [return: Description("A summary of provided text")]
    public async Task<string> GenerateSummary(string inputText)
    {
        var prompt = @"
            You are an intelligent assistant that summarizes text. Summarize the following input text in a concise and informative manner, highlighting the main points
 
            Input:
            {{input}}
 
            Summary:
            "
        ;

        var client = new OpenAIClient(new Uri(AiOptions.Endpoint), new AzureKeyCredential(AiOptions.Key));

        // Create the request
        var opt = new CompletionsOptions(AiOptions.DeploymentName, [prompt.Replace("{{input}}", inputText)]);
        opt.MaxTokens = 100;

        var summaryResult = await client.GetCompletionsAsync(opt);

        return summaryResult.Value.Choices[0].Text.Trim();
    }
}

//public class TextSource
//{
//    public string Text { get; set; }
//    public string Summary { get; set; }
//}
