using System;
using System.Collections.Generic;
using Data.Log;
using Models.RecipeSequence;
using Sequence;
using Services.Insights;
using Services.LLM;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.RecipeSequence
{
    /// <summary>
    /// A recipe sequence takes care of sliding through recipe statements,
    /// as well as broadcasting said slide-through for other components within
    /// this sequence to handle.
    /// </summary>
    [RequireComponent(typeof(RecipeEventLog))]
    public class RecipeSequence : GameSequence<RecipeStatement>
    {
        public delegate void OnControlStateChanged(bool controlsEnabled);

        public event OnControlStateChanged ControlStateChangedEvent;

        /// <summary>
        /// The statement text on the top
        /// </summary>
        [Header("Inner Components")] [SerializeField]
        private Image lostPopUp;

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

        [SerializeField] private DialogueSequence lostSequence;

        protected override List<RecipeStatement> SequencePoints { get; set; }

        // <-------------- INNER COMPONENTS  -------------->
        private ConnectionInfo _userInfo;
        [HideInInspector] public RecipeEventLog eventLog;
        [HideInInspector] public StatementHeader header;
        [HideInInspector] public HealthContainer healthContainer;

        /// <summary>
        /// The <see cref="ChatHistoryManager"/> controlling
        /// chat histories for each statement.
        /// </summary>
        [HideInInspector] public ChatHistoryManager chatHistoryManager;
        // <----------------------------------------------->

        public override void OnInitialize()
        {
            InitComponents();

            SequencePoints = statements;

            eventLog = GetComponent<RecipeEventLog>();

            eventLog.EnableLog(statements);
            header.Init();
            chatHistoryManager.Init(this, statements);

            InitSubsequences();

            healthContainer.DamageEvent += eventLog.IncrementQuestionCount;
            healthContainer.NoHealthEvent += () =>
            {
                lostPopUp.gameObject.SetActive(true);
                eventLog.IncrementAttemptsCount();
            };
        }

        private void InitComponents()
        {
            _userInfo = GetComponentInChildren<ConnectionInfo>(includeInactive: true);
            header = GetComponentInChildren<StatementHeader>(includeInactive: true);
            healthContainer = GetComponentInChildren<HealthContainer>(includeInactive: true);
            chatHistoryManager = GetComponentInChildren<ChatHistoryManager>(includeInactive: true);
            Debug.Log(gameObject.name);
        }

        private void InitSubsequences()
        {
            var sequenceController = GameSequenceController.GetInstance();
            solutionSequence.OnInitialize();
            solutionSequence.SequenceEndEvent += (_) =>
            {
                if (sequenceController.CurrentSequence.Equals(this))
                    sequenceController.Next();
            };

            lostSequence.OnInitialize();
            lostSequence.SequenceEndEvent += (_) =>
            {
                if (sequenceController.CurrentSequence.Equals(this))
                    lostSequence.OnDisabled();
            };
        }

        public override void OnEnabled()
        {
            var llmService = LLMService.GetInstance();
            gameObject.SetActive(true);
            
            llmService.InitStatements(statements);
            llmService.SetTopicPrompt(topicPromptLlm.text);
            InsightService.GetInstance().SetTopicPrompt(topicPromptInsights.text);
            
            DataLog.GetInstance().Log(DataLog.DataEvent.PuzzleStart, eventLog.puzzleId);
            SetSequenceStep(0);
        }

        public override void OnDisabled()
        {
            eventLog.LogInsights(InsightService.GetInstance().GetAllInsights());
            solutionSequence.OnDisabled();
            LLMService.GetInstance().ClearStatements();
            gameObject.SetActive(false);
        }

        public override void ResetSequence()
        {
            CurrentStep = 0;
            chatHistoryManager.ChatHistories.ForEach(history => history.ClearChat());
            _userInfo.ResetDisplay();
        }

        public void RecoverLoss()
        {
            _userInfo.ResetDisplay();
            healthContainer.ResetHealth();
            lostPopUp.gameObject.SetActive(false);
            lostSequence.OnEnabled();
        }

        /// <summary>
        /// Called when the player may progress beyond this sequence.
        /// </summary>
        public void OnSequenceWon() => solutionSequence.OnEnabled();


        public void SetControlsEnabled(bool enableControls) => ControlStateChangedEvent?.Invoke(enableControls);

        /// <summary>
        /// Gets the current <see cref="RecipeStatement">statement</see>.
        /// </summary>
        /// <returns>The current statement.</returns>
        public RecipeStatement GetCurrentStatement() => CurrentSequence;

        public List<RecipeStatement> GetAllStatements() => SequencePoints;
    }
}