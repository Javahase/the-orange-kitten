using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeSequence : MonoBehaviour
{
    [HideInInspector] public RecipeStatement currentStatement;
    [SerializeField] private TMP_Text _statementText;
    [SerializeField] private TMP_InputField _inputField;

    public ChatHistoryManager chatHistoryManager;
    public List<RecipeStatement> statements;
    
    private int _index = 0;
    private bool _isLLMPondering = false;

    void Start()
    {
        foreach (var _ in statements)
        {
            chatHistoryManager.InitNewHistory();
        }
        SelectStatement(0);
    }

    public void Test()
    {
        chatHistoryManager.GetActiveHistory().PushMessage("ROBOT", "Why are you so cool?!?");
    }

    public void SelectStatement(int index)
    {
        if (_isLLMPondering) return;
        currentStatement = statements[index];
        chatHistoryManager.SetHistoryVisible(_index, false);
        _index = index;
        _statementText.text = currentStatement.message;
        chatHistoryManager.SetHistoryVisible(_index, true);
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
        StartCoroutine(SendChatRequest(_inputField.text));
    }

    private IEnumerator SendChatRequest(string message)
    {
        _inputField.text = "";
        _inputField.interactable = false;
        _isLLMPondering = true;
        yield return LLMService.GetInstance().CreateChatConnection(currentStatement, message, (action) =>
        {
            chatHistoryManager.GetActiveHistory().PushMessage("PLAYER", action); 
        });
        _isLLMPondering = false;
        _inputField.interactable = true;
    }
}
