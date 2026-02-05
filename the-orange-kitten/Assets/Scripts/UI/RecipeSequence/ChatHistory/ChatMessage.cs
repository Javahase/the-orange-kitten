using System.Collections;
using TMPro;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text _senderNameField;
    [SerializeField] private TMP_Text _senderMessageField;
    
    /// <summary>
    /// Instantiates a message.
    /// </summary>
    /// <param name="senderName">Who sent the message?</param>
    /// <param name="senderText">What is the message?</param>
    /// <param name="parent">The parent object this message should be spawned in.</param>
    public void InstantiateMessage(string senderName, string senderText, Transform parent)
    {
        var clone = Instantiate(gameObject, parent).GetComponent<ChatMessage>();

        clone._senderNameField.text = senderName;
        clone._senderMessageField.text = senderText;
    }

    /// <summary>
    /// Instantiates a message asynchronously, making it possible to wait until the
    /// instantiation is done.
    /// </summary>
    /// <param name="senderName">Who sent the message?</param>
    /// <param name="senderText">What is the message?</param>
    /// <param name="parent">The parent object this message should be spawned in.</param>
    /// <returns>An IEnumerator containing the message clone at latest.</returns>
    public IEnumerator InstantiateMessageAsync(string senderName, string senderText, Transform parent)
    {
        var spawnClone = InstantiateAsync(gameObject, parent);
        yield return spawnClone;
        var clone = spawnClone.Result[0].GetComponent<ChatMessage>();
        clone._senderNameField.text = senderName;
        clone._senderMessageField.text = senderText;
        yield return clone;
    }
}
