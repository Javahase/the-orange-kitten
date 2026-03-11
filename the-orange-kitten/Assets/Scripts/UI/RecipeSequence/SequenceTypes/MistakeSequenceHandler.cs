using System;
using System.Linq;
using Models.RecipeSequence;
using Sequence;
using Services.Insights;
using UnityEngine;
using UnityEngine.UI;

namespace UI.RecipeSequence.SequenceTypes
{
    [RequireComponent(typeof(RecipeSequence))]
    public class MistakeSequenceHandler : MonoBehaviour
    {
        private RecipeSequence _coreSequence;
        private InsightsUI _inventory;
        private InsightService _insightService;
        private GameSequenceController _sequenceController;

        /// <summary>
        /// A <see cref="DialogueSequence"/> that should play when the player
        /// made a mistake, but already has the pieces to win this sequence.
        /// One can give the player a hint in this sequence.
        /// </summary>
        [SerializeField] private DialogueSequence hintErrorSequence;

        [SerializeField] private DialogueSequence errorSequence;

        public void Start()
        {
            _coreSequence = GetComponent<RecipeSequence>();
            _sequenceController = GameSequenceController.GetInstance();
            _insightService = InsightService.GetInstance();
            foreach (var seq in new[]
                     {
                         hintErrorSequence, errorSequence
                     })
            {
                errorSequence.OnInitialize();
                errorSequence.SequenceEndEvent += (_) =>
                {
                    if (_sequenceController.CurrentSequence.Equals(_coreSequence))
                        errorSequence.OnDisabled();
                };
            }
        }

        public void PresentStatement(RecipeInsight insight)
        {
            if (insight.important
                && _coreSequence.GetCurrentStatement().statementType == RecipeStatement.StatementType.Incorrect)
            {
                _coreSequence.OnSequenceWon();
                _coreSequence.eventLog.LogContradiction(insight.info);
            }
            else
            {
                OnMistake(
                    _insightService.GetAllInsights().Any(it => it.important));
            }
        }

        /// <summary>
        /// Called when the player made a mistake.
        /// </summary>
        /// <param name="hasClues">Whether the player can be given a hint</param>
        private void OnMistake(bool hasClues) =>
            (hasClues ? hintErrorSequence : errorSequence).OnEnabled();
    }
}