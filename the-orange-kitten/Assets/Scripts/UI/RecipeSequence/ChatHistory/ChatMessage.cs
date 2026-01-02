using TMPro;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text _senderNameField;
    [SerializeField] private TMP_Text _senderMessageField;

    public void InstantiateMessage(string senderName, string senderText, Transform parent)
    {
        var clone = Instantiate(gameObject, parent).GetComponent<ChatMessage>();

        clone._senderNameField.text = senderName;
        clone._senderMessageField.text = senderText;

    }
}
