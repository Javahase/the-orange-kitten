using System;
using System.Collections.Generic;
using UI.RecipeSequence;
using UI.RecipeSequence.SequenceTypes;
using UnityEngine;

public class RecipeEventLog : MonoBehaviour
{
    [SerializeField] private string puzzleId;
    private PuzzleData _playerData;

    public void EnableLog(List<RecipeStatement> statements)
    {
        if (gameObject.GetComponent<OddSequenceHandler>() != null)
        {
            _playerData = new OddSequenceData()
            {
                Attempts = 0,
                QuestionCount = 0,
                Id = puzzleId,
                ConversationData = new List<ConversationData>(),
                OddStatements = new List<OddStatement>(),
                GeneratedInsights = new List<string>()
            };
        }
        else
        {
            _playerData = new MistakeSequenceData() 
            {
                Attempts = 0,
                QuestionCount = 0,
                Id = puzzleId,
                ConversationData = new List<ConversationData>(),
                ContradictReason = "",
                GeneratedInsights = new List<string>()
            };
        }

        foreach (var stmt in statements)
        {
            _playerData.ConversationData.Add(new ConversationData()
            {
                Topic = stmt.message,
                Chat = new List<ChatData>()
            });
        }
        DataCollector.GetInstance().StartLevel(_playerData);
    }

    public void LogStatement(RecipeStatement statement, string sender, string message)
    {
        var conversation = _playerData.ConversationData.Find((data) => data.Topic == statement.message);
        conversation?.Chat.Add(new ChatData()
        {
            Sender = sender, Message = message
        });
        DataLog.GetInstance().Log(DataLog.DataEvent.ChatMessage, "Sent by: " + sender + ", Message: " + message);
        DataCollector.GetInstance().Save();
    }

    public void IncrementQuestionCount()
    {
        _playerData.QuestionCount++;
        DataLog.GetInstance().Log(DataLog.DataEvent.QuestionCountIncrease, "Question Count: " + _playerData.QuestionCount);
        DataCollector.GetInstance().Save();
    }

    public void IncrementAttemptsCount()
    {
        _playerData.Attempts++;
        DataLog.GetInstance().Log(DataLog.DataEvent.AttemptCountIncrease, "Attempt Count: " + _playerData.Attempts);
        DataCollector.GetInstance().Save();
    }

    public void LogOddMark(RecipeStatement statement, string reason)
    {
        if (_playerData is not OddSequenceData oddSequenceData) return;
        oddSequenceData.OddStatements.Add(new OddStatement() { Topic = statement.message, Reason = reason });
        DataLog.GetInstance().Log(DataLog.DataEvent.StatementMarked, "Marked Statement: " + statement.message + " with reason: " + reason);
        DataCollector.GetInstance().Save();
    }

    public void LogOddUnmark(RecipeStatement statement)
    {
        if (_playerData is not OddSequenceData oddSequenceData) return;
        var stmt = oddSequenceData.OddStatements.FindIndex((odd) => odd.Topic == statement.message);
        if (stmt == -1) return;
        oddSequenceData.OddStatements.RemoveAt(stmt);
        DataLog.GetInstance().Log(DataLog.DataEvent.StatementUnmarked, "Unmarked Statement: " + statement.message);
        DataCollector.GetInstance().Save();
    }

    public void LogContradiction(string reason)
    {
        if (_playerData is not MistakeSequenceData mistakeSequenceData) return;
        mistakeSequenceData.ContradictReason = reason;
        DataLog.GetInstance().Log(DataLog.DataEvent.Contradict, "Successfully pointed out contradiction, used insight: " + reason);
        DataCollector.GetInstance().Save();
    }
    
}