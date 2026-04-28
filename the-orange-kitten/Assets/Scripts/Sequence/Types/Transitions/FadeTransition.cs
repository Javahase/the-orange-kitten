using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Sequence.Types.Transitions
{
    /// <summary>
    /// Transition sequence that fades in a blackscreen between two sequences.
    /// </summary>
    public class FadeTransition : TransitionSequence
    {
        /// <summary>
        /// The curve used to fade out the blackscreen.
        /// </summary>
        [SerializeField] private AnimationCurve fadeOutCurve;

        /// <summary>
        /// The amount of time the blackscreen stays faded out after a fade-in.
        /// </summary>
        [SerializeField] private float fadedOutDuration;

        /// <summary>
        /// The curve used to fade in the blackscreen.
        /// </summary>
        [SerializeField] private AnimationCurve fadeInCurve;

        /// <summary>
        /// The blackscreen inner-component.
        /// </summary>
        private Image _blackScreen;

        public override void OnInitialize()
        {
            _blackScreen = transform.Find("BlackScreen").GetComponent<Image>();
        }

        public override IEnumerator BeginTransition(IGameSequence previous, Action next)
        {
            gameObject.SetActive(true);
            yield return FadeOut();
            yield return new WaitForSeconds(fadedOutDuration);
            previous?.OnDisabled();
            next.Invoke();
            yield return FadeIn();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// General fade-function for the blackscreen.
        /// </summary>
        /// <param name="curve">The curve used.</param>
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

        /// <summary>
        /// Fades the blackscreen in.
        /// </summary>
        public IEnumerator FadeIn() => Fade(fadeInCurve);

        /// <summary>
        /// Fades the blackscreen out.
        /// </summary>
        public IEnumerator FadeOut() => Fade(fadeOutCurve);

        public override void ResetSequence()
        {
            _blackScreen.color = _blackScreen.color.WithAlpha(fadeOutCurve.Evaluate(fadeOutCurve.GetMinTime()));
        }
    }
}