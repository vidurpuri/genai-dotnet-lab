using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.MEAI.TextCompletion.Production.Configuration
{
    public class GitHubModelsOptions
    {
        [Required]
        public string Endpoint { get; set; }

        [Required]
        public string Models { get; set; }
    }
}
