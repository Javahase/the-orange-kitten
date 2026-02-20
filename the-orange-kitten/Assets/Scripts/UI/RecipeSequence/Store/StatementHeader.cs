using TMPro;
using UI.RecipeSequence;
using UnityEngine;
using UnityEngine.UI;

public class StatementHeader : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private TMP_Text statementText;
    [SerializeField] private RecipeSequence coreSequence;

    public void Init()
    {
        coreSequence.StepChangeEvent += (ev) => { statementText.text = ev.Current.message; };

        nextButton.onClick.AddListener(() => coreSequence.Next());
        previousButton.onClick.AddListener(() => coreSequence.Previous());

        coreSequence.ControlStateChangedEvent += (controlsEnabled) =>
        {
            nextButton.interactable = controlsEnabled;
            previousButton.interactable = controlsEnabled;
        };
    }
}