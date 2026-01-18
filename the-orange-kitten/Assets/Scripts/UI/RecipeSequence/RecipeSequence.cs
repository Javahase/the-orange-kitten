using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.RecipeSequence
{
    public class RecipeSequence : GameSequence<RecipeStatement>
    {
        [SerializeField] private TMP_Text statementText;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private List<RecipeStatement> statements;
        [SerializeField] private DialogueSequence solutionSequence;
        [SerializeField] private DialogueSequence specialErrorSequence;
        [HideInInspector] public bool isLlmPondering = false;
        public ChatHistoryManager chatHistoryManager;
        

        protected override List<RecipeStatement> SequencePoints { get; set; }
        private GameSequenceController _sequenceController;

        public override void OnInitialize()
        {
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

        public void OnSequenceWon() => solutionSequence.OnEnabled();

        public void OnMistake(bool hasClues)
        {
            if (hasClues)
            {
                specialErrorSequence.OnEnabled();
            }
            else
            {
                _sequenceController.StartGenericErrorSequence();
            }
        }

        public RecipeStatement GetCurrentStatement() => CurrentSequence;

        private void SelectStatement(int index)
        {
            if (isLlmPondering) return;
            chatHistoryManager.SetHistoryVisible(CurrentStep, false);
            CurrentStep = index;
            statementText.text = CurrentSequence.message;
            chatHistoryManager.SetActiveHistory(CurrentStep);
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

        public override bool Next() => TraverseStatement(1) && base.Next();
        public override bool Previous() => TraverseStatement(-1) && base.Previous();

        public void NextF() => Next();
        public void PreviousF() => Previous();
    }
}