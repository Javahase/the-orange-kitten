using System;
using System.Collections;
using UnityEngine;

namespace Sequence.Types
{
    /// <summary>
    /// Waits a specific amount of time and does nothing.
    /// Automatically resumes.
    /// </summary>
    public class AwaitSequence : TransitionSequence
    {
        /// <summary>
        /// How long should be waited for?
        /// </summary>
        [SerializeField] private float time;

        /// <summary>
        /// Should the previous sequence be disabled before waiting?
        /// </summary>
        [SerializeField] private bool disablePreviousBeforeWait;

        public override void ResetSequence()
        {
        }

        public override IEnumerator BeginTransition(IGameSequence previous, Action next)
        {
            if (disablePreviousBeforeWait) previous?.OnDisabled();
            yield return new WaitForSeconds(time);
            if (!disablePreviousBeforeWait) previous?.OnDisabled();
            next?.Invoke();
        }

        public override void OnInitialize()
        {
        }
    }
}