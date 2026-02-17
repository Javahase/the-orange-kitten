using UnityEngine;
using UnityEngine.UI;

namespace UI.RecipeSequence.ActionBar
{
    public class MistakeActionBar : DefaultActionBar
    {

        [Header("Mistake Section Actions")]
        [SerializeField] private Button mistakeButton;
        
        protected override void Start()
        {
            base.Start();
            mistakeButton.onClick.AddListener(OnPointOutClicked);
        }

        private void OnPointOutClicked() => Insights.Open(true);
    }
}