using GenAI.MEAI.TextCompletion.Production.Models;
using Microsoft.Extensions.AI;
using OpenAI.Graders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.MEAI.TextCompletion.Production.Services
{
    public class ChatService : IChatService
    {
        private readonly List<ChatMessage> _chatHistory = [];
        private readonly IChatClient _chatClient;

        public ChatService(IChatClient chatClient)
        {
            _chatClient = chatClient;
            _chatHistory = new()
            {
                new ChatMessage(ChatRole.System, """
                    You are a friendly hiking enthusiast who helps people discover fun hikes in their area.
                    You introduce yourself when first saying hello.
                    When helping people out, you always ask them for this information
                    to inform the hiking recommendation you provide:

                    1. The location where they would like to hike
                    2. What hiking intensity they are looking for

                    You will then provide three suggestions for nearby hikes that vary in length
                    after you get that information. You will also share an interesting fact about
                    the local nature on the hikes when making a recommendation. At the end of your
                    response, ask if there is anything else you can help with.
                """)
            };
        }
        public async Task<Models.ChatResponse> SendMessageAsync(ChatRequest request)
        {
            //Validate the request
            ValidateRequest(request);

            //Add User Response to ChatHistory
            AddUserMessageToChatHistory(request);

            // Create IChatClient and send the request to the provider and get the response
            var response = await _chatClient.GetResponseAsync(_chatHistory);

            //Add Assistnat Response to ChatHistory
            AddAssistantMessageToChatHistory(response);

            return new Models.ChatResponse
            {
                AssistantResponse = response.Text,
                TimeStamp = DateTime.UtcNow
            };

        }

        private void ValidateRequest(ChatRequest request)
        {
            if (request != null && string.IsNullOrWhiteSpace(request.UserPrompt))
            {
                throw new ArgumentException("User prompt cannot be null or empty.", nameof(request.UserPrompt));
            }
        }

        private void AddUserMessageToChatHistory(ChatRequest request)
        {
            // Logic to add user response to chat history
            _chatHistory.Add(new ChatMessage(ChatRole.User, request.UserPrompt));
        }

        private void AddAssistantMessageToChatHistory(Microsoft.Extensions.AI.ChatResponse response)
        {
            _chatHistory.Add(new ChatMessage(ChatRole.Assistant, response.Text));
        }
    }
}
