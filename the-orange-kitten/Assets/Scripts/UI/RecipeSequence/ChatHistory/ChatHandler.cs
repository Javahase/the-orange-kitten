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
        private InsightService _insightService;

        private void Start()
        {
            _llmService = LLMService.GetInstance();
            _insightService = InsightService.GetInstance();
            buttonSubmit.onClick.AddListener(PressStatement);
            coreSequence.ControlStateChangedEvent += (controlsEnabled) =>
            {
                inputField.interactable = controlsEnabled;
                buttonSubmit.interactable = controlsEnabled;
            };
        }

        private void PressStatement()
        {
            StartCoroutine(SendChatRequest(inputField.text));
        }

        private IEnumerator SendChatRequest(string message)
        {
            var currentHistory = coreSequence.chatHistoryManager.GetActiveHistory();
            inputField.text = "";
            coreSequence.SetControlsEnabled(false);

            // In case instantiation is taking too long. Will prevent the UI from being messed up.
            yield return currentHistory.StartPushMessageRoutine("PLAYER", message);
            var response= _llmService.SendMessageToConversation(
                coreSequence.GetCurrentStatement(),
                message,
                (_) => {}
            );
            yield return response;
            if (response.Current is not string llmMessage) yield break;
            yield return currentHistory.StartPushMessageRoutine("ROBOT", llmMessage);
            yield return _insightService.GenerateInsight(coreSequence.GetCurrentStatement(), message, llmMessage);
            
            coreSequence.SetControlsEnabled(true);
        }
    }
}