using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.RecipeSequence.ActionBar
{
    public class DefaultActionBar : MonoBehaviour
    {
        [SerializeField] protected RecipeSequence coreSequence;

        [Header("Default Actions")] [SerializeField]
        protected Button editRecipeButton;

        [SerializeField] protected Button checkRecipeInfoButton;
        [SerializeField] protected Button checkInsightsButton;
        protected InsightsUI insightsUI;

        protected virtual void Start()
        {
            insightsUI = InsightsUI.GetInstance();
            editRecipeButton.onClick.AddListener(OnEditRecipeClicked);
            checkRecipeInfoButton.onClick.AddListener(OnCheckRecipeInfoClicked);
            checkInsightsButton.onClick.AddListener(OnCheckInsightsClicked);
        }

        private void OnEditRecipeClicked() => Debug.Log("TODO");

        private void OnCheckRecipeInfoClicked() => Debug.Log("TODO");

        private void OnCheckInsightsClicked() => insightsUI.Open(false);
    }
}