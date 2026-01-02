using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RecipeSequence : MonoBehaviour
{
    public ChatHistoryManager chatHistoryManager;
    [HideInInspector] public RecipeStatement currentStatement;

    [SerializeField] private TMP_Text statementText;
    public List<RecipeStatement> statements;
    private int _index = 0;


    void Start()
    {
        foreach (var _ in statements)
        {
            chatHistoryManager.InitNewHistory();
        }
        SelectStatement(0);
    }

    public void SelectStatement(int index)
    {
        currentStatement = statements[index];
        chatHistoryManager.SetHistoryVisible(_index, false);
        _index = index;
        statementText.text = currentStatement.message;
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


}
