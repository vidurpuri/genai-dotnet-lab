using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Assistants;
using System.ClientModel;
using System.Numerics.Tensors;

//Load user Secret to Program.cs
IConfigurationRoot configurationRoot = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

//Create token and options for OpenAIClient
var token = new ApiKeyCredential(configurationRoot["GitHubModels:Token"] ?? throw new InvalidOperationException("GitHubModels:Token is not set in user secrets."));

var options = new OpenAIClientOptions()
{
    Endpoint = new Uri("https://models.github.ai/inference")
};

//Initialize ChatClient and EmbeddingGenerator
IChatClient chatClient = new OpenAIClient(token, options).GetChatClient("openai/gpt-4o-mini").AsIChatClient();

IEmbeddingGenerator<string,Embedding<float>> embeddingGenerator = new OpenAIClient(token, options).GetEmbeddingClient("openai/text-embedding-3-small").AsIEmbeddingGenerator();



//Generate Single Embedding Vector for a given text
//var embeddingResponse = await embeddingGenerator.GenerateVectorAsync("Hello, can you tell me a short joke?");

//Console.WriteLine($"Embedding dimensions: {embeddingResponse.Span.Length}");
//foreach (var value in embeddingResponse.Span)
//{
//    Console.Write("{0:0.00}, ", value);
//}

var catVector = await embeddingGenerator.GenerateVectorAsync("cat");
var dogVector = await embeddingGenerator.GenerateVectorAsync("dog");
var kittenVector = await embeddingGenerator.GenerateVectorAsync("kitten");
var puppyVector = await embeddingGenerator.GenerateVectorAsync("puppy");

Console.WriteLine($"cat-dog similarity: {TensorPrimitives.CosineSimilarity(catVector.Span, dogVector.Span):F2}");
Console.WriteLine($"cat-kitten similarity: {TensorPrimitives.CosineSimilarity(catVector.Span, kittenVector.Span):F2}");
Console.WriteLine($"dog-kitten similarity: {TensorPrimitives.CosineSimilarity(dogVector.Span, kittenVector.Span):F2}");
Console.WriteLine($"dog-puppy similarity: {TensorPrimitives.CosineSimilarity(dogVector.Span, puppyVector.Span):F2}");