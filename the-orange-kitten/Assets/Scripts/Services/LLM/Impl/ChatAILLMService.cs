using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models.LLM;
using Models.RecipeSequence;
using UnityEngine;

namespace Services.LLM.Impl
{
    /// <summary>
    /// LLM Service by Chat AI.
    /// </summary>
    public class ChatAILLMService : LLMService
    {
        /// <summary>
        /// Chat AI general API endpoint.
        /// </summary>
        private const string ApiEndpoint = "https://chat-ai.academiccloud.de/v1";

        /// <summary>
        /// The completions endpoint for completing messages.
        /// </summary>
        private const string CompletionsEndpoint = ApiEndpoint + "/chat/completions";

        /// <summary>
        /// A list of statements of the current level.
        /// </summary>
        private List<RecipeStatement> _statements = new();

        /// <summary>
        /// The LLM model that we want to use.
        /// </summary>
        [SerializeField] private ChatAIModel.Model model;

        [SerializeField] private TextAsset rulePrompt;

        /// <summary>
        /// Keeps track of conversations per recipe statement.
        /// </summary>
        private readonly Dictionary<RecipeStatement, List<ChatAIMessage>> _conversationMap = new();

        /// <summary>
        /// The secret API key located under <c>Secrets/api_key.txt</c>.
        /// </summary>
        private string _apiKey;

        /// <summary>
        /// The system prompt the LLM should receive regarding the current topic.
        /// "{0}" will be replaced with a list the <see cref="_statements">recipe statements</see>.
        /// </summary>
        private string _topicPrompt;

        /// <summary>
        /// The rule prompt
        /// </summary>
        private string _rulePrompt;

        private void Start()
        {
            var file = Resources.Load<TextAsset>(ResourceConstants.ApiKey);
            _apiKey = file.text;
            _rulePrompt = rulePrompt.text;
        }

        public override void InitStatements(List<RecipeStatement> statements)
        {
            _statements = statements;
        }

        public override void SetTopicPrompt(string prompt)
        {
            _topicPrompt = string.Format(prompt,
                _statements.Select((stmt) => "- " + stmt.message).Aggregate((a, b) => a + "\n" + b));
        }

        public override void ClearStatements()
        {
            _conversationMap.Clear();
        }

        private List<ChatAIMessage> GetConversationHeader(RecipeStatement stmt) => new List<ChatAIMessage>()
        {
            ChatAIMessage.CreateSystemPrompt(_rulePrompt),
            ChatAIMessage.CreateSystemPrompt(_topicPrompt),
            ChatAIMessage.CreateSystemPrompt(
                $"You will now answer questions related to this step you took: {stmt.message}")
        };

        public override IEnumerator SendMessageToConversation(RecipeStatement stmt, string request)
        {
            if (!_conversationMap.ContainsKey(stmt)) _conversationMap.Add(stmt, GetConversationHeader(stmt));
            if (!_conversationMap.TryGetValue(stmt, out var messages)) yield break;

            messages.Add(ChatAIMessage.CreateUserMessage(request));

            var message = ChatAIUtil.SendMessage(
                _apiKey,
                model,
                messages, 2 * 60);
            yield return message;

            if (message.Current is not WebEither<string, ChatAIMessage>)
                yield return "Unknown Error!";

            var either = (WebEither<string, ChatAIMessage>)message.Current!;
            switch (either)
            {
                case Left<string, ChatAIMessage> l:
                    yield return l.Error;
                    break;
                case Right<string, ChatAIMessage> r:
                    messages.Add(r.Value);
                    yield return r.Value.content;
                    break;
            }
        }

        public override List<RecipeStatement> GetStatements() => _statements;
    }
}