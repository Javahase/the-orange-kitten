using System.Collections.Generic;
using UnityEngine;

// TODO: Clean this up :)
public class ChatHistoryManager : MonoBehaviour
{

    [SerializeField] private GameObject chatHistoryInstance;
    public readonly List<ChatHistory> ChatHistories = new();
    private ChatHistory _activeHistory;

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
        var history = ChatHistories[index];
        history.gameObject.SetActive(true);
        _activeHistory = history;
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
