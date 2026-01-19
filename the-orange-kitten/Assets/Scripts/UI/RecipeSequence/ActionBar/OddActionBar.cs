using System;
using TMPro;
using UI.RecipeSequence.SequenceTypes;
using UnityEngine;
using UnityEngine.UI;

namespace UI.RecipeSequence.ActionBar
{
    public class OddActionBar : DefaultActionBar
    {
        [Header("Odd Sequence Actions")]
        [SerializeField] private Button endExaminationButton;
        [SerializeField] private Button markAsOddButton;
        private OddSequenceHandler _oddHandler;
        
        protected override void Start()
        {
            base.Start();
            _oddHandler = coreSequence.GetComponent<OddSequenceHandler>();
            _oddHandler.StatementMarkChangedEvent += (stmt, marked) =>
            {
                if (stmt == coreSequence.GetCurrentStatement())
                {
                    markAsOddButton.GetComponentInChildren<TMP_Text>().text
                        = marked switch
                        {
                            RecipeStatement.MarkingState.Unmarked => "Mark as Odd",
                            RecipeStatement.MarkingState.Pending => "Cancel",
                            RecipeStatement.MarkingState.Marked => "Unmark Statement",
                            _ => throw new ArgumentException("Invalid enum value")
                        };
                }
            };
            
            markAsOddButton.onClick.AddListener(MarkAsOdd);
        }

        private void MarkAsOdd()
        {
            switch (coreSequence.GetCurrentStatement().markingState)
            {
                case RecipeStatement.MarkingState.Marked:
                case RecipeStatement.MarkingState.Pending:
                    _oddHandler.SetCurrentStatementState(RecipeStatement.MarkingState.Unmarked);
                    break;
                case RecipeStatement.MarkingState.Unmarked:
                    _oddHandler.SetCurrentStatementState(RecipeStatement.MarkingState.Pending);
                    break;
                default:
                    throw new ArgumentException("Invalid enum value");
            }
        }
    }
}