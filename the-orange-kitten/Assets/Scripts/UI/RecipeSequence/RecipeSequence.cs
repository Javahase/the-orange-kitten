using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class RecipeSequence : MonoBehaviour
{
    [HideInInspector] public RecipeStatement currentStatement;
    [SerializeField] private TMP_Text statementText;
    [SerializeField] private TMP_InputField inputField;

    public ChatHistoryManager chatHistoryManager;
    public List<RecipeStatement> statements;

    private LLMService _llmService;
    private int _index = 0;
    private bool _isLlmPondering = false;

    private void Start()
    {
        _llmService = LLMService.GetInstance();
        foreach (var _ in statements)
        {
            chatHistoryManager.InitNewHistory();
        }
        SelectStatement(0);
    }

    public void SelectStatement(int index)
    {
        if (_isLlmPondering) return;
        currentStatement = statements[index];
        chatHistoryManager.SetHistoryVisible(_index, false);
        _index = index;
        statementText.text = currentStatement.message;
        chatHistoryManager.SetActiveHistory(_index);
    }

    private void TraverseStatement(int direction)
    {
        if (_index + direction < 0 || _index + direction >= statements.Count)
            return;
        var traversed = statements[_index + direction];
        if (traversed.hidden)
        {
            TraverseStatement(direction + Math.Sign(direction));
            return;
        }
        SelectStatement(_index + direction);
    }

    public void SelectNextStatement() => TraverseStatement(1);
    public void SelectPrevStatement() => TraverseStatement(-1);

    public void PressStatement()
    {
        StartCoroutine(SendChatRequest(inputField.text));
    }

    private IEnumerator SendChatRequest(string message)
    {
        inputField.text = "";
        inputField.interactable = false;
        _isLlmPondering = true;
        yield return _llmService.CreateChatConnection(currentStatement, message, (action) =>
        {
            chatHistoryManager.GetActiveHistory().PushMessage("PLAYER", action); 
        });
        _isLlmPondering = false;
        inputField.interactable = true;
    }

    public void OpenInsightInfo(bool contradictionMode)
    {
        InsightsUI.GetInstance().Open(contradictionMode);
    }
}
