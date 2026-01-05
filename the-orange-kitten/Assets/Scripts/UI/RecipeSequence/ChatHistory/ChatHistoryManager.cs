using System.Collections.Generic;
using UnityEngine;

// TODO: Clean this up :)
public class ChatHistoryManager : MonoBehaviour
{

    [SerializeField] private GameObject chatHistoryInstance;
    private readonly List<ChatHistory> _chatHistories = new();
    private ChatHistory _activeHistory;

    public ChatHistory InitNewHistory()
    {
        var obj = Instantiate(chatHistoryInstance, transform);
        obj.SetActive(false);
        var chatHistory = obj.GetComponent<ChatHistory>();
        _chatHistories.Add(chatHistory);
        return chatHistory;
    }

    public void SetActiveHistory(int index)
    {
        var history = _chatHistories[index];
        history.gameObject.SetActive(true);
        _activeHistory = history;
    }

    public void SetHistoryVisible(int index, bool visible)
    {
        _chatHistories[index].gameObject.SetActive(visible);
    }

    public ChatHistory GetActiveHistory()
    {
        return _activeHistory;
    }
}
