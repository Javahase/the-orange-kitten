using UnityEngine;

public class ChatHistoryManager : MonoBehaviour
{

    [SerializeField] private GameObject _chatHistoryInstance;

    public ChatHistory InitNewHistory()
    {
        var obj = Instantiate(_chatHistoryInstance, transform);
        obj.SetActive(false);
        return obj.GetComponent<ChatHistory>();
    }

    public void SetHistoryVisible(int index, bool visible)
    {
        GetComponentsInChildren<ChatHistory>(includeInactive: true)[index].gameObject.SetActive(visible);
    }

    public ChatHistory GetActiveHistory()
    {
        return GetComponentInChildren<ChatHistory>();
    }
}
