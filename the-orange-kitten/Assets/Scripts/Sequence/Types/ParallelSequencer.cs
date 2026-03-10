using UnityEngine;

namespace Sequence.Types
{
    /// <summary>
    /// A Parallel Sequencer may reference another <see cref="DialogueSequence"/>
    /// to enable. The parent-sequence should afterward simply move on to the next sequence.
    /// Especially useful for overlaying scripted dialogue (such as the tutorial dialogue).
    /// </summary>
    public class ParallelSequencer : MonoBehaviour, IGameSequence
    {
        /// <summary>
        /// The Sequence to enable.
        /// </summary>
        [SerializeField] private DialogueSequence parallelSequence;
    
        public void OnInitialize()
        {
            parallelSequence.OnInitialize();
            parallelSequence.SequenceEndEvent += (_) => parallelSequence.OnDisabled();
        }

        public void OnEnabled()
        {
            gameObject.SetActive(true);
            parallelSequence.OnEnabled();
        }

        public void OnDisabled()
        {
            gameObject.SetActive(false);
        }

        public void ResetSequence()
        {
            parallelSequence.ResetSequence();
        }
    }
}