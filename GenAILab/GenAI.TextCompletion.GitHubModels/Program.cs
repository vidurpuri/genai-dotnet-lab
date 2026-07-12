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

//string prompt = "Hello, can you tell me a long joke?";
//Console.WriteLine("Prompt -> ",prompt);

//var response =  client.GetStreamingResponseAsync(prompt);
//await foreach (var message in response)
//{
//    Console.Write(message);
//}

#endregion

#region Classification

//var classificationPrompt = """
//Please classify the following sentences into categories: 
//- 'complaint' 
//- 'suggestion' 
//- 'praise' 
//- 'other'.

//1) "I love the new layout!"
//2) "You should add a night mode."
//3) "When I try to log in, it keeps failing."
//4) "This app is decent."
//""";

//Console.WriteLine($"Classification Prompt -> , { classificationPrompt}");

//ChatResponse response = await client.GetResponseAsync(classificationPrompt);

//Console.WriteLine($"Classification Response -> , {response}");

#endregion

#region Summarization

//var summaryPrompt = """
//Summarize the following blog in 1 concise sentences:

//"Microservices architecture is increasingly popular for building complex applications, but it comes with additional overhead. It's crucial to ensure each service is as small and focused as possible, and that the team invests in robust CI/CD pipelines to manage deployments and updates. Proper monitoring is also essential to maintain reliability as the system grows."
//""";

//Console.WriteLine($"Summary Prompt -> , {summaryPrompt}");

//var response = await client.GetResponseAsync(summaryPrompt);

//Console.WriteLine($"Summary Response -> , {response}");

#endregion

#region Sentiment Analysis

var analysisPrompt = """
        You will analyze the sentiment of the following product reviews. 
        Each line is its own review. Output the sentiment of each review in a bulleted list and then provide a generate sentiment of all reviews.

        I bought this product and it's amazing. I love it!
        This product is terrible. I hate it.
        I'm not sure about this product. It's okay.
        I found this product based on the other reviews. It worked for a bit, and then it didn't.
        """;

Console.WriteLine($"Sentiment Analysis Prompt -> , {analysisPrompt}");

var response = await client.GetResponseAsync(analysisPrompt);

Console.WriteLine($"Sentiment Analysis Response -> , {response}");

#endregion