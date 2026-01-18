using System;
using UnityEngine;

namespace UI.RecipeSequence.SequenceTypes
{
    [RequireComponent(typeof(RecipeSequence))]
    public class OddSequenceHandler : MonoBehaviour
    {
        public delegate void OnStatementMarked(RecipeStatement statement, bool marked);
        public event OnStatementMarked StatementMarkChangedEvent;
        
        private RecipeSequence _coreSequence;

        private void Start()
        {
            _coreSequence = GetComponent<RecipeSequence>();
        }

        private void SetStatementMarked(RecipeStatement statement, bool marked)
        {
            statement.marked = marked;
            StatementMarkChangedEvent?.Invoke(statement, marked);
        }
        
        public void SetCurrentStatementMarked(bool marked) => SetStatementMarked(_coreSequence.GetCurrentStatement(), marked);
    }
}