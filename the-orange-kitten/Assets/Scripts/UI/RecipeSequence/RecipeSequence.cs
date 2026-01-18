using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.RecipeSequence
{
    /// <summary>
    /// A recipe sequence takes care of sliding through recipe statements,
    /// as well as broadcasting said slide-through for other components within
    /// this sequence to handle.
    /// </summary>
    public class RecipeSequence : GameSequence<RecipeStatement>
    {
        /// <summary>
        /// The statement text on the top
        /// </summary>
        [SerializeField] private TMP_Text statementText;
        /// <summary>
        /// A list of <see cref="RecipeStatement">statements</see> to be shown.
        /// </summary>
        [SerializeField] private List<RecipeStatement> statements;
        
        /// <summary>
        /// A <see cref="DialogueSequence"/> that should play when the player
        /// did win the sequence.
        /// </summary>
        [SerializeField] private DialogueSequence solutionSequence;
        
        /// <summary>
        /// A <see cref="DialogueSequence"/> that should play when the player
        /// made a mistake, but already has the pieces to win this sequence.
        /// One can give the player a hint in this sequence.
        /// </summary>
        [SerializeField] private DialogueSequence specialErrorSequence;
        
        /// <summary>
        /// Whether other components should wait for the LLM to finish processing
        /// the message.
        /// </summary>
        [HideInInspector] public bool isLlmPondering = false;
        
        /// <summary>
        /// The <see cref="ChatHistoryManager"/> controlling
        /// chat histories for each statements.
        /// </summary>
        public ChatHistoryManager chatHistoryManager;
        
        protected override List<RecipeStatement> SequencePoints { get; set; }
        
        /// <summary>
        /// The global <see cref="GameSequenceController">sequence controller</see>
        /// that handles the game progression.
        /// </summary>
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
            solutionSequence.SequenceEndEvent += (_) =>
            {
                if (_sequenceController.CurrentSequence.Equals(this))
                    _sequenceController.Next();
            };
            specialErrorSequence.OnInitialize();
            specialErrorSequence.SequenceEndEvent += (_) =>
            {
                if (_sequenceController.CurrentSequence.Equals(this))
                    specialErrorSequence.OnDisabled();
            };
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

        /// <summary>
        /// Called when the player may progress beyond this sequence.
        /// </summary>
        public void OnSequenceWon() => solutionSequence.OnEnabled();

        /// <summary>
        /// Called when the player made a mistake.
        /// </summary>
        /// <param name="hasClues">Whether the player can be given a hint</param>
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

        /// <summary>
        /// Gets the current <see cref="RecipeStatement">statement</see>.
        /// </summary>
        /// <returns>The current statement.</returns>
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