using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.RecipeSequence
{
    /// <summary>
    /// A recipe sequence takes care of sliding through recipe statements,
    /// as well as broadcasting said slide-through for other components within
    /// this sequence to handle.
    /// </summary>
    public class RecipeSequence : GameSequence<RecipeStatement>
    {
        public delegate void OnControlStateChanged(bool controlsEnabled);

        public event OnControlStateChanged ControlStateChangedEvent;

        /// <summary>
        /// The statement text on the top
        /// </summary>
        [Header("Inner Components")]
        [SerializeField] private TMP_Text statementText;

        [SerializeField] private Button prevButton;
        
        [SerializeField] private Button nextButton;
        
        /// <summary>
        /// The <see cref="ChatHistoryManager"/> controlling
        /// chat histories for each statement.
        /// </summary>
        public ChatHistoryManager chatHistoryManager;
        
        /// <summary>
        /// A list of <see cref="RecipeStatement">statements</see> to be shown.
        /// </summary>
        [Header("Data Fields")]
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

        protected override List<RecipeStatement> SequencePoints { get; set; }

        /// <summary>
        /// The global <see cref="GameSequenceController">sequence controller</see>
        /// that handles the game progression.
        /// </summary>
        private GameSequenceController _sequenceController;

        private LLMService _llmService;

        public override void OnInitialize()
        {
            _sequenceController = GameSequenceController.GetInstance();
            _llmService = LLMService.GetInstance();
            SequencePoints = statements;
            foreach (var _ in SequencePoints)
            {
                chatHistoryManager.InitNewHistory();
            }
            InitSubsequences();
            InitButtons();
        }

        private void OnEnable()
        {
            _llmService.InitStatements(statements);
        }

        private void OnDisable()
        {
            _llmService.ClearStatements();
        }

        private void InitButtons()
        {
            nextButton.onClick.AddListener(() => Next());
            prevButton.onClick.AddListener(() => Previous());
            ControlStateChangedEvent += (controlsEnabled) =>
            {
                nextButton.enabled = controlsEnabled;
                prevButton.enabled = controlsEnabled;
            };
        }

        private void InitSubsequences()
        {
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
        
        public void SetControlsEnabled(bool enableControls) => ControlStateChangedEvent?.Invoke(enableControls);

        /// <summary>
        /// Gets the current <see cref="RecipeStatement">statement</see>.
        /// </summary>
        /// <returns>The current statement.</returns>
        public RecipeStatement GetCurrentStatement() => CurrentSequence;
        
        private void SelectStatement(int index)
        {
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
    }
}