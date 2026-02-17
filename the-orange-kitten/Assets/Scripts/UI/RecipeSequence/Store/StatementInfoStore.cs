using TMPro;
using UI.RecipeSequence;
using UnityEngine;

[RequireComponent(typeof(RecipeSequence))]
public class StatementInfoStore : MonoBehaviour
{
    [HideInInspector] public RecipeStatement currentStatement;
    [SerializeField] private TMP_Text statementText;
    
    private RecipeSequence _coreSequence;

    public void Init()
    {
        _coreSequence = GetComponent<RecipeSequence>();
        _coreSequence.StepChangeEvent += (ev) =>
        {
            currentStatement = ev.Current;
            statementText.text = ev.Current.message;
        };
    }
}