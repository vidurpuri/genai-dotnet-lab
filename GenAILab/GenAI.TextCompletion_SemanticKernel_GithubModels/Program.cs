using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.ChatCompletion;
using GenAI.TextCompletion_SemanticKernel_GithubModels.Plugins;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var token = configuration["GitHubModels:Token"] ?? throw new InvalidOperationException("GitHubModels:Token is not set in user secrets.");

var builder = Kernel.CreateBuilder();

builder.AddOpenAIChatCompletion(
    modelId: "openai/gpt-4o-mini",
    apiKey: token,
    endpoint: new Uri("https://models.github.ai/inference")
    );

Kernel kernel = builder.Build();

var history = new ChatHistory();

var chatCompletion = kernel.GetRequiredService<IChatCompletionService>();

kernel.Plugins.AddFromObject(new CustomerPlugin(httpClient: new HttpClient() { BaseAddress = new Uri("https://localhost:7020/") }));


// Enable concurrent invocation of functions to get the latest news and the current time.
FunctionChoiceBehaviorOptions options = new() { AllowConcurrentInvocation = true };


var settings = new OpenAIPromptExecutionSettings()
{
    ChatSystemPrompt = "",
    Temperature = 0.9,
    MaxTokens = 1000,
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(options: options)
};


#region Basic Chat

//var response = await kernel.InvokePromptAsync("Hello, can you tell me a short joke?");

//Console.WriteLine(response);

#endregion

#region Streaming Chat

//var response =  kernel.InvokePromptStreamingAsync("Hello, can you tell me a long joke?");

//await foreach(var message in response)
//{
//    Console.Write(message);
//};

#endregion

#region History with Streaming Response

while (true)
{
    Console.WriteLine("User: ");
    var userInput = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(userInput))
    {
        break;
    }
    string fullMessage = "";
    history.AddUserMessage(userInput);
    var response =chatCompletion.GetStreamingChatMessageContentsAsync(history,settings, kernel);
    await foreach (var message in response)
    {
        Console.Write(message.Content);
        fullMessage += message.Content;
    }
    history.AddAssistantMessage(fullMessage);
}

#endregion
