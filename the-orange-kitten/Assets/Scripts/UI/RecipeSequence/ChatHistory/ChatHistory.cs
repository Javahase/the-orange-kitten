using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChatHistory : MonoBehaviour
{

    [SerializeField] private ChatMessage chatMessagePrefab;
    [SerializeField] private VerticalLayoutGroup viewport;

    public void PushMessage(string author, string content)
    {
        chatMessagePrefab.InstantiateMessage(author, content, viewport.transform);
    }

    public void ClearChat()
    {
        viewport.GetComponentsInChildren<ChatMessage>().ToList().ForEach(msg => Destroy(msg.gameObject));
    }
}
