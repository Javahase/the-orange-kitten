using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatHistory : MonoBehaviour
{

    [HideInInspector] public List<TMP_Text> chatMessages;

    public void SetChatHistory(List<TMP_Text> chatMessages)
    {
        this.chatMessages = chatMessages;
    }
    
    void PushMessage(string author, string content)
    {
        
    }
}
