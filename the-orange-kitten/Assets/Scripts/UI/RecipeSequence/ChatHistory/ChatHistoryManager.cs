using System.Collections.Generic;
using UI.RecipeSequence;
using UnityEngine;

// TODO: Clean this up :)
public class ChatHistoryManager : MonoBehaviour
{
    [SerializeField] private GameObject chatHistoryInstance;
    public readonly List<ChatHistory> ChatHistories = new();
    private ChatHistory _activeHistory;
    private RecipeSequence _coreSequence;

    public void Init(RecipeSequence coreSequence, List<RecipeStatement> sequences)
    {
        _coreSequence = coreSequence;
        foreach (var _ in sequences) InitNewHistory();
        _coreSequence.StepChangeEvent += (ev) => SetActiveHistory(ev.CurrentStep);
    }

    public ChatHistory InitNewHistory()
    {
        var obj = Instantiate(chatHistoryInstance, transform);
        obj.SetActive(false);
        var chatHistory = obj.GetComponent<ChatHistory>();
        ChatHistories.Add(chatHistory);
        return chatHistory;
    }

    public void SetActiveHistory(int index)
    {
        if (_activeHistory)
            _activeHistory.gameObject.SetActive(false);
        _activeHistory = ChatHistories[index];
        _activeHistory.gameObject.SetActive(true);
    }

    public void SetHistoryVisible(int index, bool visible)
    {
        ChatHistories[index].gameObject.SetActive(visible);
    }

    public ChatHistory GetActiveHistory()
    {
        return _activeHistory;
    }
}