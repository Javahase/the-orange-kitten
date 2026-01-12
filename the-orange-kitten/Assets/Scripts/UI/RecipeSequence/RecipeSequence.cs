using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RecipeSequence : GameSequence<RecipeStatement>
{
    [SerializeField] private TMP_Text statementText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private List<RecipeStatement> statements;
    [SerializeField] private DialogueSequence solutionSequence;
    [SerializeField] private DialogueSequence specialErrorSequence;
    public ChatHistoryManager chatHistoryManager;
    
    protected override List<RecipeStatement> SequencePoints { get; set; }
    private LLMService _llmService;
    private InsightService _insightService;
    private bool _isLlmPondering = false;
    private GameSequenceController _sequenceController;

    public override void OnInitialize()
    {
        _llmService = LLMService.GetInstance();
        _insightService = InsightService.GetInstance();
        _sequenceController = GameSequenceController.GetInstance();
        SequencePoints = statements;
        foreach (var _ in SequencePoints)
        {
            chatHistoryManager.InitNewHistory();
        }
        solutionSequence.OnInitialize();
        solutionSequence.SequenceEndEvent += (_) => _sequenceController.Next();
        specialErrorSequence.OnInitialize();
        specialErrorSequence.SequenceEndEvent += (_) => specialErrorSequence.OnDisabled();
    }

    public override void OnEnabled()
    {
        gameObject.SetActive(true);
        SelectStatement(0);
    }

    public override void OnDisabled()
    {
        solutionSequence.OnDisabled();
        gameObject.SetActive(false);
    }

    public override void ResetSequence()
    {
        CurrentStep = 0;
        chatHistoryManager.ChatHistories.ForEach(history => history.ClearChat());
    }

    public void PresentStatement(RecipeInsight insight)
    {
        var currentSequence = _sequenceController.GetIndexOfPuzzle(this);
        if (insight.correctSequence == currentSequence
            && CurrentSequence.statementType == RecipeStatement.StatementType.Incorrect)
        {
            solutionSequence.OnEnabled();
        }
        else if (_insightService.GetAllInsights().Any(it => it.correctSequence == currentSequence))
        {
            specialErrorSequence.OnEnabled();
        }
        else
        {
            _sequenceController.StartGenericErrorSequence();
        }
    }

    private void SelectStatement(int index)
    {
        if (_isLlmPondering) return;
        chatHistoryManager.SetHistoryVisible(CurrentStep, false);
        CurrentStep = index;
        statementText.text = CurrentSequence.message;
        chatHistoryManager.SetActiveHistory(CurrentStep);
        base.OnStepChange();
    }

    private bool TraverseStatement(int direction)
    {
        if (CurrentStep + direction < 0 || CurrentStep + direction >= SequencePoints.Count)
            return false;
        var traversed = SequencePoints[CurrentStep + direction];
        while (traversed.hidden)
        {
            direction += Math.Sign(direction);
        }
        SelectStatement(CurrentStep + direction);
        return true;
    }

    public override bool Next() => TraverseStatement(1);
    public override bool Previous() => TraverseStatement(-1);

    public void NextF() => Next();
    public void PreviousF() => Previous();

    public void PressStatement()
    {
        StartCoroutine(SendChatRequest(inputField.text));
    }

    private IEnumerator SendChatRequest(string message)
    {
        inputField.text = "";
        inputField.interactable = false;
        _isLlmPondering = true;
        yield return _llmService.CreateChatConnection(CurrentSequence, message, (action) =>
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
