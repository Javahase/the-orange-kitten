using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseFrame : MonoBehaviour, IPointerClickHandler, IInitializable
{
    [SerializeField] private float frameSpeed = 0.1f;
    [TextArea] public string dialogueText;
    public bool locationText = false;

    private Coroutine _currentCoroutine;
    private DialogueSequence _sequence;
    private TMP_Text _dialogueText;
    private Image _imgContinue;
    private readonly char[] _controlChars = { '\\', '*', '(', ')' };

    public virtual void OnInitialize()
    {
        _dialogueText = transform.Find("DialogueTextBox/DialogueText").GetComponent<TMP_Text>();
        _imgContinue = transform.Find("ProgressDialoguePrompt").GetComponent<Image>();
        _sequence = GetComponentInParent<DialogueSequence>(includeInactive: true);
        if (locationText) _dialogueText.alignment = TextAlignmentOptions.Center;
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
        }
    }

    private void SkipToEnd()
    {
        StopCoroutine(_currentCoroutine);
        _imgContinue.gameObject.SetActive(true);
        _currentCoroutine = null;
        var withoutBreaks = dialogueText.Replace("^1", "").Replace("^2", "");
        _dialogueText.text = GetTokens(withoutBreaks).Select((s) => s.Item1)
            .Aggregate(string.Concat);
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

    private List<(string, bool)> GetTokens(string sequence)
    {
        var tokens = new List<(string, bool)>();
        var decorated = false;

        for (var i = 0; i < sequence.Length; i++)
        {
            var symbol = sequence[i];
            switch (symbol)
            {
                // Empty chars do not count towards small breaks between characters.
                case ' ':
                    tokens.Add((symbol.ToString(), false));
                    break;
                // Escape opportunity for control symbols. Only one \ or invalid control symbols next will output normally. Will do small break.
                case '\\':
                    if (_controlChars.Contains(sequence[i + 1]))
                    {
                        i++;
                    }

                    tokens.Add((sequence[i].ToString(), true));
                    break;
                // Toggles the orange highlighting. Will not break.
                case '*':
                    decorated = !decorated;
                    tokens.Add((!decorated ? "</color>" : "<color=#de8b26>", false));
                    break;
                // Starts a blue thinking-highlight. The '('-token will count as a small break.
                case '(':
                    tokens.Add(("<color=#4a86cf>", false));
                    tokens.Add(("(", true));
                    break;
                // Ends a blue thinking-highlight. The ')'-token will count as a small break.
                case ')':
                    tokens.Add((")", true));
                    tokens.Add(("</color>", false));
                    break;
                // Normal behavior.
                default:
                    tokens.Add((symbol.ToString(), true));
                    break;
            }
        }

        return tokens;
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
        var tokens = GetTokens(sequence);
        foreach (var (token, shouldBreak) in tokens)
        {
            _dialogueText.text += token;
            if (shouldBreak)
                yield return new WaitForSeconds(frameSpeed);
        }
    }
}