using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    /// The system prompt the LLM should receive.
    /// "{0}" will be replaced with a list the <see cref="_statements">recipe statements</see>.
    /// </summary>
    [SerializeField] private TextAsset prompt;


    /// <summary>
    /// Keeps track of conversations per recipe statement.
    /// </summary>
    private readonly Dictionary<RecipeStatement, List<ChatAIMessage>> _conversationMap = new();

    /// <summary>
    /// The secret API key located under <c>Secrets/api_key.txt</c>.
    /// </summary>
    private string _apiKey;

    /// <summary>
    /// The final prompt text post-formatting.
    /// </summary>
    private string _prompt;

    private string _rulePrompt;

    private void Start()
    {
        var file = Resources.Load<TextAsset>("Secrets/api_key");
        _apiKey = file.text;
        _rulePrompt = rulePrompt.text;
    }

    public override void InitStatements(List<RecipeStatement> statements)
    {
        _statements = statements;
        var stmtTexts = statements.Select((stmt) => "- " + stmt.message).Aggregate((a, b) => a + "\n" + b);
        _prompt = string.Format(prompt.text, stmtTexts);
        InsightService.GetInstance().Init();
    }

    public override void ClearStatements()
    {
        _conversationMap.Clear();
    }

    public override IEnumerator SendMessageToConversation(RecipeStatement stmt, string request, Action<string> callback)
    {
        if (!_conversationMap.ContainsKey(stmt))
        {
            _conversationMap.Add(stmt, new List<ChatAIMessage>()
            {
                ChatAIMessage.CreateSystemPrompt(_rulePrompt),
                ChatAIMessage.CreateSystemPrompt(_prompt),
                ChatAIMessage.CreateSystemPrompt($"You will now answer questions related to this step you took: {stmt.message}")
            });
        }

        if (!_conversationMap.TryGetValue(stmt, out var messages)) yield break;

        messages.Add(ChatAIMessage.CreateUserMessage(request));

        var message = ChatAIUtil.SendMessage(
            _apiKey,
            model,
            messages);
        yield return message;

        if (message.Current is WebEither<string, ChatAIMessage> either)
        {
            if (either.IsLeft())
            {
                yield return either.GetLeft();
            }
            else
            {
                var llmResponse = either.GetRight();
                messages.Add(llmResponse);
                yield return llmResponse.content;
            }
        }
        else
        {
            yield return "Unknown Error!";
        }
    }

    public override List<RecipeStatement> GetStatements() => _statements;
}