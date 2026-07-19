using GenAI.MEAI.TextCompletion.Production.Configuration;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OpenAI;
using OllamaSharp;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.MEAI.TextCompletion.Production.Providers
{
    public class AIRegistration
    {
        private readonly IOptions<AIOption> _options;

        public AIRegistration(IOptions<AIOption> options)
        {
            _options = options;
        }

        public IChatClient RegisterAIProvider()
        {
            switch (_options.Value.Provider)
            {
                case AIProvider.GitHubModels:
                    return RegisterGitHubModelsProvider(_options.Value.GitHubModels);
                case AIProvider.Ollama:
                    return RegisterOllamaProvider(_options.Value.Ollama);
                default:
                    throw new InvalidOperationException($"Unsupported AI Provider '{_options.Value.Provider}'.");
            }
        }

        private IChatClient RegisterGitHubModelsProvider(GitHubModelsOptions options)
        {
            
            var modelOptions = new OpenAIClientOptions()
            {
                Endpoint = new Uri(options.Endpoint)
            };

            IChatClient client = new OpenAIClient(new ApiKeyCredential(options.Token), modelOptions).GetChatClient(options.Model).AsIChatClient();

            return client;
        }

        private IChatClient RegisterOllamaProvider(OllamaOptions options)
        {
            IChatClient client = new OllamaApiClient(new Uri(options.Endpoint), options.Model);
            return client;
        }
    }
}
