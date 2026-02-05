using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChatHistory : MonoBehaviour
{

    [SerializeField] private ChatMessage chatMessagePrefab;
    [SerializeField] private VerticalLayoutGroup viewport;
    private ScrollRect _scrollRect;

    private void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    public void PushMessage(string author, string content)
    {
        StartCoroutine(StartPushMessageRoutine(author, content));
    }

    public IEnumerator StartPushMessageRoutine(string author, string content)
    {
        yield return chatMessagePrefab.InstantiateMessageAsync(author, content, viewport.transform);
        _scrollRect.normalizedPosition = Vector2.zero;
    }

    public void ClearChat()
    {
        viewport.GetComponentsInChildren<ChatMessage>().ToList().ForEach(msg => Destroy(msg.gameObject));
    }
}
