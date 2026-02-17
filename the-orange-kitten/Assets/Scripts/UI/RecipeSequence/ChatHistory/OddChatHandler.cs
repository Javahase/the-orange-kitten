using TMPro;
using UI.RecipeSequence.SequenceTypes;
using UnityEngine;
using UnityEngine.UI;

public class OddChatHandler : MonoBehaviour
{
    [SerializeField] private OddSequenceHandler oddSequence;
    [SerializeField] private TMP_InputField text;
    [SerializeField] private Button send;

    private void Start()
    {
        send.onClick.AddListener(() => oddSequence.SetCurrentStatementMarked(text.text));
    }
}