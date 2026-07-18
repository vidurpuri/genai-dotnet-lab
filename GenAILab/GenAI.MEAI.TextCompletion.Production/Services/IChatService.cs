using GenAI.MEAI.TextCompletion.Production.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.MEAI.TextCompletion.Production.Services
{
    internal interface IChatService
    {
        public Task<ChatResponse> SendMessageAsync(ChatRequest request);
    }
}
