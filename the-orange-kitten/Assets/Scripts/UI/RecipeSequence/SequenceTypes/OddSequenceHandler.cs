using Models.RecipeSequence;
using UnityEngine;
using Util.Object;

namespace UI.RecipeSequence.SequenceTypes
{
    [RequireComponent(typeof(RecipeSequence))]
    public class OddSequenceHandler : MonoBehaviour
    {
        public delegate void OnStatementMarkChanged(RecipeStatement statement,
            RecipeStatement.MarkingState markingState);

        public event OnStatementMarkChanged StatementMarkChangedEvent;

        public SwitchBehavior chatBoxSwitcher;

        private RecipeSequence _coreSequence;

        private void Start()
        {
            _coreSequence = GetComponent<RecipeSequence>();
            StatementMarkChangedEvent += (stmt, marked) =>
            {
                if (stmt != _coreSequence.GetCurrentStatement()) return;

                // If the current statement is marked as "pending", prevent the player from switching
                // and make it so the reason-chatbox appears.
                _coreSequence.SetControlsEnabled(marked != RecipeStatement.MarkingState.Pending);
                chatBoxSwitcher.Select(marked == RecipeStatement.MarkingState.Pending ? 1 : 0);
            };
        }

        private void SetMarkingState(RecipeStatement statement, RecipeStatement.MarkingState markingState)
        {
            statement.markingState = markingState;
            StatementMarkChangedEvent?.Invoke(statement, markingState);
            if (markingState == RecipeStatement.MarkingState.Unmarked)
            {
                _coreSequence.eventLog.LogOddUnmark(_coreSequence.GetCurrentStatement());
            }
        }

        public void SetCurrentStatementState(RecipeStatement.MarkingState markingState) =>
            SetMarkingState(_coreSequence.GetCurrentStatement(), markingState);

        public void SetCurrentStatementMarked(string reason)
        {
            SetCurrentStatementState(RecipeStatement.MarkingState.Marked);
            if (reason == null || reason.Trim() == "") return;
            _coreSequence.eventLog.LogOddMark(_coreSequence.GetCurrentStatement(), reason);
        }
    }
}