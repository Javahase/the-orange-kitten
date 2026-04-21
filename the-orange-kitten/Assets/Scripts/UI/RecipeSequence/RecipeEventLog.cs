using System;
using System.Collections.Generic;
using Data.Collector;
using Data.Log;
using Models.PlayerData;
using Models.PlayerData.Conversation;
using Models.PlayerData.OddSequenceData;
using Models.PlayerData.SequenceData;
using Models.RecipeSequence;
using UI.RecipeSequence;
using UI.RecipeSequence.SequenceTypes;
using UnityEngine;

/// <summary>
/// A component that is attached to a <see cref="RecipeSequence"/>.<br/>
/// Creates and provides data log functionality for a specific <see cref="RecipeSequence"/>.
/// </summary>
public class RecipeEventLog : MonoBehaviour
{
    /// <summary>
    /// The ID that the log refers to when mentioning this sequence.
    /// </summary>
    public string puzzleId;
    
    /// <summary>
    /// Whether the log should measure this <see cref="RecipeSequence"/>.
    /// </summary>
    [SerializeField] private bool doMeasure = true;
    
    /// <summary>
    /// The <see cref="PuzzleData"/> that is updated throughout the puzzle and afterward saved.
    /// </summary>
    private PuzzleData _playerData;
    
    /// <summary>
    /// An instance of the <see cref="DataCollector"/>.
    /// </summary>
    private DataCollector _collector;

    /// <summary>
    /// Enables logging functionality, initializing the necessary data stores.
    /// </summary>
    /// <param name="statements">The statements to be put into the data store.</param>
    public void EnableLog(List<RecipeStatement> statements)
    {
        _collector = DataCollector.GetInstance();
        if (!doMeasure)
            return;
        if (gameObject.GetComponent<OddSequenceHandler>() != null)
        {
            _playerData = new OddSequenceData()
            {
                Attempts = 0,
                QuestionCount = 0,
                Id = puzzleId,
                ConversationData = new List<ConversationData>(),
                OddStatements = new List<OddStatement>(),
                GeneratedInsights = new List<RecipeInsight>()
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
                GeneratedInsights = new List<RecipeInsight>()
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
        _collector.StartLevel(_playerData);
    }

    /// <summary>
    /// Logs a message the player or the chat bot made within a certain conversation topic.
    /// </summary>
    /// <param name="statement">The conversation topic.</param>
    /// <param name="sender">The name of the sender.</param>
    /// <param name="message">The message the sender sent.</param>
    public void LogStatement(RecipeStatement statement, string sender, string message)
    {
        if (!doMeasure)
            return;
        var conversation = _playerData.ConversationData.Find((data) => data.Topic == statement.message);
        conversation?.Chat.Add(new ChatData()
        {
            Sender = sender, Message = message
        });
        DataLog.GetInstance().Log(DataLog.DataEvent.ChatMessage, "Sent by: " + sender + ", Message: " + message);
        _collector.Save();
    }

    /// <summary>
    /// Logs an increment of the amount of questions the player aksed.
    /// </summary>
    public void IncrementQuestionCount()
    {
        if (!doMeasure)
            return;
        _playerData.QuestionCount++;
        DataLog.GetInstance().Log(DataLog.DataEvent.QuestionCountIncrease, "Question Count: " + _playerData.QuestionCount);
        _collector.Save();
    }

    /// <summary>
    /// Logs an increment of the amount of "game overs" the player had.
    /// </summary>
    public void IncrementAttemptsCount()
    {
        if (!doMeasure)
            return;
        _playerData.Attempts++;
        DataLog.GetInstance().Log(DataLog.DataEvent.AttemptCountIncrease, "Attempt Count: " + _playerData.Attempts);
        _collector.Save();
    }
    
    /// <summary>
    /// Logs the player marking a statement as odd.
    /// </summary>
    /// <param name="statement">The statement that got marked as odd.</param>
    /// <param name="reason">The reason the player provides for the marking.</param>
    public void LogOddMark(RecipeStatement statement, string reason)
    {
        if (!doMeasure)
            return;
        if (_playerData is not OddSequenceData oddSequenceData) return;
        oddSequenceData.OddStatements.Add(new OddStatement() { Topic = statement.message, Reason = reason });
        DataLog.GetInstance().Log(DataLog.DataEvent.StatementMarked, "Marked Statement: " + statement.message + " with reason: " + reason);
        _collector.Save();
    }

    /// <summary>
    /// Logs the player unmarking a statement from being odd.
    /// </summary>
    /// <param name="statement">The statement that got unmarked.</param>
    public void LogOddUnmark(RecipeStatement statement)
    {
        if (!doMeasure)
            return;
        if (_playerData is not OddSequenceData oddSequenceData) return;
        var stmt = oddSequenceData.OddStatements.FindIndex((odd) => odd.Topic == statement.message);
        if (stmt == -1) return;
        oddSequenceData.OddStatements.RemoveAt(stmt);
        DataLog.GetInstance().Log(DataLog.DataEvent.StatementUnmarked, "Unmarked Statement: " + statement.message);
        _collector.Save();
    }

    /// <summary>
    /// Logs the player pointing out a contradiction.
    /// </summary>
    /// <param name="reason">The reason the player provided for the inconsistency.</param>
    public void LogContradiction(string reason)
    {
        if (!doMeasure || _playerData is not MistakeSequenceData mistakeSequenceData) return;
        mistakeSequenceData.ContradictReason = reason;
        DataLog.GetInstance().Log(DataLog.DataEvent.Contradict, "Successfully pointed out contradiction, used insight: " + reason);
        _collector.Save();
    }

    /// <summary>
    /// Pushes a list of insights into the log.
    /// </summary>
    /// <param name="insights">The insights to push into the log.</param>
    public void LogInsights(List<RecipeInsight> insights)
    {
        if (!doMeasure)
            return;
        _playerData.GeneratedInsights = new List<RecipeInsight>(insights);
        _collector.Save();
    }
    
}