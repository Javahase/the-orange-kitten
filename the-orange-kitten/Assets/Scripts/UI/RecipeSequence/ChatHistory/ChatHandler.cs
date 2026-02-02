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
            coreSequence.ControlStateChangedEvent += (controlsEnabled) =>
            {
                inputField.interactable = controlsEnabled;
                buttonSubmit.enabled = controlsEnabled;
            };
        }

        private void PressStatement()
        {
            StartCoroutine(SendChatRequest(inputField.text));
        }
        
        private IEnumerator SendChatRequest(string message)
        {
            inputField.text = "";
            coreSequence.SetControlsEnabled(false);
            coreSequence.chatHistoryManager.GetActiveHistory().PushMessage("PLAYER", message);
            yield return _llmService.SendMessageToConversation(coreSequence.GetCurrentStatement(), message,
                (action) => { coreSequence.chatHistoryManager.GetActiveHistory().PushMessage("ROBOT", action); }
            );
            coreSequence.SetControlsEnabled(true);
        }
    }
}