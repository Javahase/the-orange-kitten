using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseFrame : MonoBehaviour, IPointerClickHandler, IInitializable
{
    [SerializeField]
    private float frameSpeed = 0.1f;
    [TextArea] public string dialogueText;

    private Coroutine _currentCoroutine;
    private DialogueSequence _sequence;
    private TMP_Text _dialogueText;
    private Image _imgContinue;

    public virtual void OnInitialize()
    {
        _dialogueText = transform.Find("DialogueTextBox/DialogueText").GetComponent<TMP_Text>();
        _imgContinue = transform.Find("ProgressDialoguePrompt").GetComponent<Image>();
        _sequence = GetComponentInParent<DialogueSequence>(includeInactive: true);
    }

    public void OnEnabled()
    {
        gameObject.SetActive(true);
        _imgContinue.gameObject.SetActive(false);
        _dialogueText.text = "";
        _currentCoroutine = StartCoroutine(StartAnimation());
    }

    public void OnDisabled()
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
        _dialogueText.text = dialogueText.Replace("^1", "").Replace("^2", "");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentCoroutine != null)
        {
            SkipToEnd();
        }
        else
        {
            _sequence.Next();
        }
    }


    private IEnumerator StartAnimation()
    {
        var breaksLarge = dialogueText.Split("^1");
        var breaks = breaksLarge.Select((s) => s.Split("^2")).ToList();
        foreach (var smallBreaks in breaks)
        {
            foreach (var sequence in smallBreaks)
            {
                yield return ReadSequence(sequence);
                yield return new WaitForSeconds(2 * frameSpeed);
            }

            yield return new WaitForSeconds(4 * frameSpeed);
        }
        _imgContinue.gameObject.SetActive(true);
        _currentCoroutine = null;
    }

    private IEnumerator ReadSequence(string sequence)
    {
        foreach (var symbol in sequence)
        {
            if (symbol == ' ')
            {
                _dialogueText.text += " ";
                continue;
            }

            _dialogueText.text += symbol;
            yield return new WaitForSeconds(frameSpeed);
        }
    }
}