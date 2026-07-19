using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GenAI.MEAI.TextCompletion.Production.Configuration;
using GenAI.MEAI.TextCompletion.Production.Services;
using GenAI.MEAI.TextCompletion.Production.Providers;
using GenAI.MEAI.TextCompletion.Production.Models;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

Console.WriteLine(builder.Configuration["AI:GitHubModels:Token"]);

//Register AIOptions with AI section in appsettings.json
builder.Services.Configure<AIOption>(builder.Configuration.GetSection("AI"));

Console.WriteLine(builder.Configuration["AI:GitHubModels:Endpoint"]);
Console.WriteLine(builder.Configuration["AI:GitHubModels:Model"]);

builder.Services.AddSingleton<AIRegistration>();

//Register IChatClient based on AIProvider
builder.Services.AddSingleton<IChatClient>(serviceProvider =>
{
    var aiRegistration = serviceProvider.GetRequiredService<AIRegistration>();
    return aiRegistration.RegisterAIProvider();
});

//Resgiter ChatService
builder.Services.AddSingleton<IChatService, ChatService>();

var host = builder.Build();

var chatService = host.Services.GetRequiredService<IChatService>();

Console.WriteLine("==================================");
Console.WriteLine("🤖 AI Assistant");
Console.WriteLine("Type 'exit' to quit.");
Console.WriteLine("==================================");

while (true)
{
    Console.Write("\nYou: ");
    var prompt = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(prompt))
        continue;

    if (prompt.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    if (chatService == null)
    {
        Console.WriteLine("\nAI: chatService is not initialized.");
        continue;
    }

    try
    {
        var response = chatService.SendStreamingMessageAsync(new ChatRequest
        {
            UserPrompt = prompt
        }).GetAwaiter().GetResult();

        Console.WriteLine($"\nAI: {response.AssistantResponse ?? "<no response>"}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nAI: Error calling chat service: {ex.Message}");
    }
}