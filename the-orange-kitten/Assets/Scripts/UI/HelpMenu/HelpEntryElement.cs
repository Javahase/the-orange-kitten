using System;
using Models.HelpMenu;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.HelpMenu
{
    public class HelpEntryElement : MonoBehaviour, IPointerClickHandler
    {

        [SerializeField] private TMP_Text questionComponent;
        [SerializeField] private TMP_Text answerComponent;
        [SerializeField] private Image dropdownIndicatorIcon;
        [SerializeField] private Sprite iconClosed, iconOpened;

        private void Start()
        {
            SetExpanded(false);
        }

        public void SetContent(HelpMenuEntry helpEntry)
        {
            questionComponent.text = helpEntry.question;
            answerComponent.text = helpEntry.answer;
        }

        public void SetExpanded(bool expanded)
        {
            answerComponent.gameObject.SetActive(expanded);
            dropdownIndicatorIcon.sprite = expanded ? iconOpened : iconClosed;
        }

        public void ToggleExpansion() => SetExpanded(!answerComponent.gameObject.activeSelf);


        public void OnPointerClick(PointerEventData eventData) => ToggleExpansion();
    }
}