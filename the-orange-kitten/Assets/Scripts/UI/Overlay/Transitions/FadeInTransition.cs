using System;
using System.Collections;
using Unity.VisualScripting;
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

    public override IEnumerator BeginTransition(IGameSequence previous, IGameSequence next)
    {
        gameObject.SetActive(true);
        previous?.OnDisabled();
        next!.OnEnabled();
        yield return FadeIn();
        gameObject.SetActive(false);
    }

    private IEnumerator FadeIn()
    {
        var segmentSeconds = fadeInSeconds / 10f;
        while (_blackScreen.color.a > 0)
        {
            _blackScreen.color = _blackScreen.color.WithAlpha(Math.Max(0, _blackScreen.color.a - 0.1f));
            yield return new WaitForSeconds(segmentSeconds);
        }
    }

    public override void ResetSequence()
    {
        _blackScreen.color = _blackScreen.color.WithAlpha(1f);
    }
}