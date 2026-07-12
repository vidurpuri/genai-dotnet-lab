using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ClientModel;

//Get Creds from userSecret & Create options
IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var creds = new ApiKeyCredential(configuration["GitHubModels:Token"] ?? throw new InvalidOperationException("GitHubModels:Token is not set in user secrets."));
var options = new OpenAIClientOptions()
{
    Endpoint = new Uri("https://models.github.ai/inference")
};

//Create a chat Client
IChatClient client = new OpenAIClient(creds, options).GetChatClient("openai/gpt-4o-mini").AsIChatClient();


#region Basic Chat 
////Create a chat response using the chat client and the prompt
//ChatResponse response = await client.GetResponseAsync("Hello, can you tell me a short joke?");

//Console.WriteLine(response);
#endregion

#region Streaming Chat

string prompt = "Hello, can you tell me a long joke?";
Console.WriteLine("Prompt -> ",prompt);

var response =  client.GetStreamingResponseAsync(prompt);
await foreach (var message in response)
{
    Console.Write(message);
}

#endregion