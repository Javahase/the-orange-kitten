using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.RecipeSequence.ChatHistory
{
    public class ChatHandler : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button buttonSubmit;
        [SerializeField] private RecipeSequence coreSequence;
        private LLMService _llmService;

        private void Start()
        {
            _llmService = LLMService.GetInstance();
            buttonSubmit.onClick.AddListener(PressStatement);
        }

        private void PressStatement()
        {
            StartCoroutine(SendChatRequest(inputField.text));
        }

        // TODO: Let Chat History Manager handle this logic
        private IEnumerator SendChatRequest(string message)
        {
            inputField.text = "";
            inputField.interactable = false;
            coreSequence.isLlmPondering = true;
            yield return _llmService.CreateChatConnection(coreSequence.GetCurrentStatement(), message,
                (action) => { coreSequence.chatHistoryManager.GetActiveHistory().PushMessage("PLAYER", action); }
            );
            coreSequence.isLlmPondering = false;
            inputField.interactable = true;
        }
    }
}