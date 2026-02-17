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
    [RequireComponent(typeof(RecipeEventLog), typeof(StatementInfoStore))]
    public class RecipeSequence : GameSequence<RecipeStatement>
    {
        public delegate void OnControlStateChanged(bool controlsEnabled);

        public event OnControlStateChanged ControlStateChangedEvent;

        /// <summary>
        /// The statement text on the top
        /// </summary>
        [Header("Inner Components")] [SerializeField]
        private Button prevButton;

        [SerializeField] private Button nextButton;

        public HealthContainer healthContainer;

        [SerializeField] private ConnectionInfo userInfo;

        [SerializeField] private Image lostPopUp;

        /// <summary>
        /// The <see cref="ChatHistoryManager"/> controlling
        /// chat histories for each statement.
        /// </summary>
        public ChatHistoryManager chatHistoryManager;

        /// <summary>
        /// A list of <see cref="RecipeStatement">statements</see> to be shown.
        /// </summary>
        [Header("Data Fields")] [SerializeField]
        private List<RecipeStatement> statements;

        [SerializeField] private TextAsset topicPromptLlm;
        [SerializeField] private TextAsset topicPromptInsights;

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

        [SerializeField] private DialogueSequence lostSequence;

        protected override List<RecipeStatement> SequencePoints { get; set; }

        /// <summary>
        /// The global <see cref="GameSequenceController">sequence controller</see>
        /// that handles the game progression.
        /// </summary>
        private GameSequenceController _sequenceController;

        private LLMService _llmService;
        private InsightService _insightService;

        [HideInInspector] public RecipeEventLog eventLog;
        [HideInInspector] public StatementInfoStore infoStore;

        public override void OnInitialize()
        {
            _sequenceController = GameSequenceController.GetInstance();
            _llmService = LLMService.GetInstance();
            _insightService = InsightService.GetInstance();

            SequencePoints = statements;

            eventLog = GetComponent<RecipeEventLog>();
            infoStore = GetComponent<StatementInfoStore>();

            eventLog.EnableLog(statements);
            infoStore.Init();
            chatHistoryManager.Init(this, statements);

            InitSubsequences();
            InitButtons();

            healthContainer.DamageEvent += eventLog.IncrementQuestionCount;
            healthContainer.NoHealthEvent += () =>
            {
                lostPopUp.gameObject.SetActive(true);
                eventLog.IncrementAttemptsCount();
            };
        }

        private void InitButtons()
        {
            nextButton.onClick.AddListener(() => Next());
            prevButton.onClick.AddListener(() => Previous());

            ControlStateChangedEvent += (controlsEnabled) =>
            {
                nextButton.interactable = controlsEnabled;
                prevButton.interactable = controlsEnabled;
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
            foreach (var sequence in new[] { specialErrorSequence, lostSequence })
            {
                sequence.OnInitialize();
                sequence.SequenceEndEvent += (_) =>
                {
                    if (_sequenceController.CurrentSequence.Equals(this))
                        sequence.OnDisabled();
                };
            }
        }

        public override void OnEnabled()
        {
            gameObject.SetActive(true);
            _llmService.InitStatements(statements);
            _llmService.SetTopicPrompt(topicPromptLlm.text);
            _insightService.SetTopicPrompt(topicPromptInsights.text);
            SetSequenceStep(0);
        }

        public override void OnDisabled()
        {
            eventLog.LogInsights(InsightService.GetInstance().GetAllInsights());
            solutionSequence.OnDisabled();
            _llmService.ClearStatements();
            gameObject.SetActive(false);
        }

        public override void ResetSequence()
        {
            CurrentStep = 0;
            chatHistoryManager.ChatHistories.ForEach(history => history.ClearChat());
            userInfo.ResetDisplay();
        }

        public void RecoverLoss()
        {
            userInfo.ResetDisplay();
            healthContainer.ResetHealth();
            lostPopUp.gameObject.SetActive(false);
            lostSequence.OnEnabled();
        }

        /// <summary>
        /// Called when the player may progress beyond this sequence.
        /// </summary>
        public void OnSequenceWon() => solutionSequence.OnEnabled();

        /// <summary>
        /// Called when the player made a mistake.
        /// </summary>
        /// <param name="hasClues">Whether the player can be given a hint</param>
        public void OnMistake(bool hasClues) =>
            (hasClues ? specialErrorSequence : _sequenceController.errorSequence).OnEnabled();


        public void SetControlsEnabled(bool enableControls) => ControlStateChangedEvent?.Invoke(enableControls);

        /// <summary>
        /// Gets the current <see cref="RecipeStatement">statement</see>.
        /// </summary>
        /// <returns>The current statement.</returns>
        public RecipeStatement GetCurrentStatement() => CurrentSequence;
    }
}