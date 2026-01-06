using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInTransition : TransitionSequence
{

    [SerializeField] private float fadeInSeconds = 1;
    private Image _blackScreen;
    
    public override void OnInitialize()
    {
        _blackScreen = transform.Find("BlackScreen").GetComponent<Image>();
    }

    public override bool BeginTransition(IGameSequence previous, IGameSequence next)
    {
        if (next == null)
            throw new ArgumentException("Next sequence must not be null.");
        next.OnEnabled();
        var done = false;
        StartCoroutine(FadeIn(() => done = true));
        while (!done);
        return true;
    }

    private IEnumerator FadeIn(Action onDone)
    {
        var segmentSeconds = fadeInSeconds / 10f;
        while (_blackScreen.fillAmount > 0)
        {
            _blackScreen.fillAmount -= Math.Max(0, _blackScreen.fillAmount - 0.1f);
            yield return new WaitForSeconds(segmentSeconds);
        }
        onDone.Invoke();
    }

    public override void ResetSequence()
    {
    }
}