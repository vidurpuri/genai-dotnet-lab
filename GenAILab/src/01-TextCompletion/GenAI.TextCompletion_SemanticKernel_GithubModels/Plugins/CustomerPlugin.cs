using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.TextCompletion_SemanticKernel_GithubModels.Plugins
{
    public class CustomerPlugin
    {
        private readonly HttpClient _httpClient;

        public CustomerPlugin(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [KernelFunction]
        [Description("Retrieves all customers from the Customer API.")]
        public async Task<string> GetAllCustomersAsync()
        {
            var response = await _httpClient.GetAsync("api/customer");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
