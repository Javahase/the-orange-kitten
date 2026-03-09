using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ChatAIInsightService : InsightService
{
    [SerializeField] private TextAsset insightRulesPrompt;
    [SerializeField] private ChatAIModel.Model model;
    private string _insightRulesPrompt;
    private string _insightPrompt;
    private string _apiKey;

    protected override void Start()
    {
        base.Start();
        _apiKey = Resources.Load<TextAsset>(ResourceConstants.ApiKey).text;
        _insightRulesPrompt = insightRulesPrompt.text;
    }

    public override IEnumerator GenerateInsight(RecipeStatement context, string question, string statement)
    {
        var message = ChatAIUtil.SendMessage(_apiKey, model, new List<ChatAIMessage>()
        {
            ChatAIMessage.CreateSystemPrompt(_insightRulesPrompt),
            ChatAIMessage.CreateSystemPrompt(_insightPrompt),
            ChatAIMessage.CreateSystemPrompt($"The user asked a question about: {context.message}"),
            ChatAIMessage.CreateUserMessage($"I asked: {question}"),
            ChatAIMessage.CreateUserMessage(
                $"What can I learn from the statement the robot made in response? The statement is: {statement}"),
        });
        yield return message;

        if (message.Current is not WebEither<string, ChatAIMessage> response) yield break;
        var content = response
            .Map((m) => m.content)
            .Map(JObject.Parse);

        if (!content.IsRight(out var json))
        {
            Debug.Log(response.GetLeft());
            yield break;
        }
        
        var insight = json.GetValue("insight")?.ToObject<string>();
        var important = json.GetValue("important")?.ToObject<bool>();
        if (insight == null || important == null) yield break;

        AddInsight(new RecipeInsight(insight,
            important.Value ? GameSequenceController.GetInstance().GetIndexOfCurrentPuzzle() : -1));
    }

    public override void SetTopicPrompt(string topicPrompt)
    {
        var stmts = LlmService.GetStatements();
        var stmtList = stmts.Select((s) => $"- {s.message}").Aggregate((a, b) => a + "\n" + b);
        var contradictoryStmt =
            stmts.Find(stmt => stmt.statementType == RecipeStatement.StatementType.Incorrect)?.message;

        _insightPrompt = string.Format(topicPrompt, stmtList, contradictoryStmt);
    }
}