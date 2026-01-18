using System;
using System.Linq;
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

        public void Start()
        {
            _coreSequence = GetComponent<RecipeSequence>();
            _sequenceController = GameSequenceController.GetInstance();
            _insightService = InsightService.GetInstance();
        }

        public void PresentStatement(RecipeInsight insight)
        {
            var currentSequence = _sequenceController.GetIndexOfPuzzle(_coreSequence);
            if (insight.correctSequence == currentSequence
                && _coreSequence.GetCurrentStatement().statementType == RecipeStatement.StatementType.Incorrect)
            {
                _coreSequence.OnSequenceWon();
            }
            else
            {
                _coreSequence.OnMistake(
                    _insightService.GetAllInsights().Any(it => it.correctSequence == currentSequence));
            }
        }
    }
}