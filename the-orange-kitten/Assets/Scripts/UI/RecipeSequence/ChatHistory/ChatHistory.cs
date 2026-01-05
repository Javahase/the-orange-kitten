using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChatHistory : MonoBehaviour
{

    [SerializeField] private ChatMessage _chatMessagePrefab;
    [SerializeField] private VerticalLayoutGroup _viewport;

    public void PushMessage(string author, string content)
    {
        _chatMessagePrefab.InstantiateMessage(author, content, _viewport.transform);
    }

    public void ClearChat()
    {
        _viewport.GetComponentsInChildren<ChatMessage>().ToList().ForEach(msg => Destroy(msg.gameObject));
    }
}
