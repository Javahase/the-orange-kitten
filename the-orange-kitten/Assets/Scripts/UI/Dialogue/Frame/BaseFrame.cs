using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseFrame : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private float frameSpeed = 0.1f;
    [TextArea] public string dialogueText;

    private Coroutine _currentCoroutine;
    private DialogueSequence _sequence;
    private TMP_Text _dialogueText;
    private Image _imgContinue;

    public virtual void Init()
    {
        _dialogueText = transform.Find("DialogueTextBox/DialogueText").GetComponent<TMP_Text>();
        _imgContinue = transform.Find("ProgressDialoguePrompt").GetComponent<Image>();
        _sequence = GetComponentInParent<DialogueSequence>(includeInactive: true);
    }

    public void HandleSelected()
    {
        gameObject.SetActive(true);
        _imgContinue.gameObject.SetActive(false);
        _dialogueText.text = "";
        _currentCoroutine = StartCoroutine(StartAnimation());
    }

    public void HandleDeselected()
    {
        gameObject.SetActive(false);
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
    }

    public void SkipToEnd()
    {
        StopCoroutine(_currentCoroutine);
        _imgContinue.gameObject.SetActive(true);
        _currentCoroutine = null;
        _dialogueText.text = dialogueText;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentCoroutine != null)
        {
            SkipToEnd();
        }
        else
        {
            _sequence.NextFrame();
        }
    }


    private IEnumerator StartAnimation()
    {
        foreach (var symbol in dialogueText)
        {
            if (symbol == ' ')
            {
                _dialogueText.text += " ";
                continue;
            }

            _dialogueText.text += symbol;
            yield return new WaitForSeconds(frameSpeed);
        }
        _imgContinue.gameObject.SetActive(true);
        _currentCoroutine = null;
    }
}