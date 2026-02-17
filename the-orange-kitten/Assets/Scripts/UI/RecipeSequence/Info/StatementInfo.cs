using System;
using TMPro;
using UI.RecipeSequence.SequenceTypes;
using UnityEngine;
using Util.Object;

namespace UI.RecipeSequence.Info
{
    public class StatementInfo : MonoBehaviour
    {
        [SerializeField] private RecipeSequence coreSequence;
        [SerializeField] private SwitchBehavior statementInfoText;

        private void Start()
        {
            var oddSequenceHandler = coreSequence.GetComponent<OddSequenceHandler>();
            oddSequenceHandler.StatementMarkChangedEvent += (stmt, state) =>
            {
                if (stmt == coreSequence.GetCurrentStatement())
                {
                    statementInfoText.Select((int)state);
                }
            };
            coreSequence.StepChangeEvent += (ev) =>
            {
                // Will there be even situations where this is not true?
                if (ev.Source == coreSequence)
                    statementInfoText.Select((int)ev.Current.markingState);
            };
        }
    }
}