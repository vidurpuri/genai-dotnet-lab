using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ClientModel;

//Get Creds from userSecret & Create options
IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var creds = new ApiKeyCredential(configuration["GitHubModels:Token"] ?? throw new InvalidOperationException("OpenAI:Token is not set in user secrets."));

var options = new OpenAIClientOptions()
{
    Endpoint = new Uri("https://models.github.ai/inference")
};

//Create a chat Client & enable function invocation
IChatClient client = new ChatClientBuilder(new OpenAIClient(creds, options).GetChatClient("openai/gpt-4o-mini").AsIChatClient()).UseFunctionInvocation().Build();

var chatOptions = new ChatOptions
{
    Tools = [AIFunctionFactory.Create((string location, string unit) => 
    {
        //Call Weather API to get the current weather for a given location
        var temp = Random.Shared.Next(5,20);
        var condition = Random.Shared.Next(0,2) == 0 ? "sunny" : "cloudy";
        return $"The current weather in {location} is {temp}°C and {condition}.";
    },
    "get_current_weather",
    "Get the current weather in a given location")]
};

List<ChatMessage> chatHistory = [new(ChatRole.System, """
    You are a hiking enthusiast who helps people discover fun hikes in their area. 
    You are upbeat and friendly.
    """)];

// Weather conversation relevant to the registered function.
chatHistory.Add(new(ChatRole.User, """
    I live in Istanbul and I'm looking for a moderate intensity hike. 
    What's the current weather like? 
    """));

Console.WriteLine($"{chatHistory.Last().Role} >>> {chatHistory.Last()}");

ChatResponse response = await client.GetResponseAsync(chatHistory, chatOptions);

chatHistory.Add(new(ChatRole.Assistant, response.Text));

Console.WriteLine($"{chatHistory.Last().Role} >>> {chatHistory.Last()}");