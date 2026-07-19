using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.MEAI.TextCompletion.Production.Configuration
{
    public class AIOption
    {
        public AIProvider Provider { get; set; }
        public GitHubModelsOptions  GitHubModels { get; set; }
        public OllamaOptions Ollama { get; set; }

        public decimal Temperature { get; set; }
        public int MaxTokens { get; set; }

    }
}
