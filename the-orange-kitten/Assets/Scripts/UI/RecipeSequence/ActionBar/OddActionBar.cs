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
                        = marked ? "Unmark Statement" : "Mark as Odd";
                }
            };
            
            markAsOddButton.onClick.AddListener(MarkAsOdd);
        }

        private void MarkAsOdd()
        {
            if (!coreSequence.GetCurrentStatement().marked)
            {
                _oddHandler.SetCurrentStatementMarked(true);
            }
            else
            {
                _oddHandler.SetCurrentStatementMarked(false);
            }
        }
    }
}