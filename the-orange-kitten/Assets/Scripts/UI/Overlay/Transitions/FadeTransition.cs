using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : TransitionSequence
{
    [SerializeField] private AnimationCurve fadeOutCurve;
    [SerializeField] private float fadedOutDuration;
    [SerializeField] private AnimationCurve fadeInCurve;
    private Image _blackScreen;

    public override void OnInitialize()
    {
        _blackScreen = transform.Find("BlackScreen").GetComponent<Image>();
    }

    public override IEnumerator BeginTransition(IGameSequence previous, IGameSequence next)
    {
        gameObject.SetActive(true);
        yield return FadeOut();
        yield return new WaitForSeconds(fadedOutDuration);
        previous?.OnDisabled();
        next?.OnEnabled();
        yield return FadeIn();
        gameObject.SetActive(false);
    }
    
    private IEnumerator Fade(AnimationCurve curve)
    {
        var time = curve.GetMinTime();
        while (time < curve.GetMaxTime())
        {
            _blackScreen.color = _blackScreen.color.WithAlpha(curve.Evaluate(time));
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public IEnumerator FadeIn() => Fade(fadeInCurve);
    public IEnumerator FadeOut() => Fade(fadeOutCurve);

    public override void ResetSequence()
    {
        _blackScreen.color = _blackScreen.color.WithAlpha(fadeOutCurve.Evaluate(fadeOutCurve.GetMinTime()));
    }
}