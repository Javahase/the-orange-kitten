using System.Collections.Generic;
using UnityEngine;

// TODO: Clean this up :)
public class ChatHistoryManager : MonoBehaviour
{

    [SerializeField] private GameObject chatHistoryInstance;
    private readonly List<ChatHistory> _chatHistories = new();

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
        
        SetHistoryVisible(index, true);
    }

    public void SetHistoryVisible(int index, bool visible)
    {
        var target = GetComponentsInChildren<ChatHistory>()[index];
        _chatHistories[index].gameObject.SetActive(visible);
        
    }

    public ChatHistory GetActiveHistory()
    {
        return _chatHistories.Find(chatHistory => chatHistory.gameObject.activeSelf);
    }
}
