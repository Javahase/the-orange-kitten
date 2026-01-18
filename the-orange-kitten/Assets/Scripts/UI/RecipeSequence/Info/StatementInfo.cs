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
            oddSequenceHandler.StatementMarkChangedEvent += (stmt, marked) =>
            {
                if (stmt == coreSequence.GetCurrentStatement())
                {
                    statementInfoText.SetOn(marked);
                }
            };
            coreSequence.StepChangeEvent += (src, _, next) =>
            {
                // Will there be even situations where this is not true?
                if (src == coreSequence)
                    statementInfoText.SetOn(next.marked);
            };
        }
    }
}