using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sequence.Types;
using Sequence.Types.Transitions;
using UI.RecipeSequence;
using UnityEngine.SceneManagement;

namespace Sequence
{
    /// <summary>
    /// The Game Sequence controller is responsible for the core
    /// gameplay interaction. It contains subsequences that can call
    /// upon the Game Sequence Controller to enter the next sequence,
    /// once the subsequence is done.
    /// </summary>
    public class GameSequenceController : SingletonObject<GameSequenceController>
    {
        public FadeTransition genericFader;

        /// <summary>
        /// A list of sequences that should be direct children of the <see cref="GameSequenceController"/>.
        /// </summary>
        private List<IGameSequence> _sequences;

        /// <summary>
        /// At which location within <see cref="_sequences"/> the controller
        /// currently is.<br/>
        ///
        /// Modifying this also modifies <see cref="CurrentSequence"/>.
        /// </summary>
        private int _currentStep;

        /// <summary>
        /// The current sequence the controller currently points to.
        /// </summary>
        public IGameSequence CurrentSequence => _sequences[_currentStep];

        private void Start()
        {
            _sequences = GetComponentsInChildren<IGameSequence>(true).ToList();
            _sequences.ForEach(seq => seq.OnInitialize());
            _sequences.Where(seq => seq is DialogueSequence)
                .Cast<DialogueSequence>()
                .ToList()
                .ForEach(seq => seq.SequenceEndEvent += (_) => Next());
        
            genericFader.OnInitialize();
            ForceFadeIn();
            CurrentSequence.OnEnabled();
        }
    
        /// <summary>
        /// Retrieves the index of a <see cref="RecipeSequence"/>
        /// in a list of only <see cref="RecipeSequence">RecipeSequences</see>
        /// </summary>
        /// <param name="sequence">The RecipeSequence to get the index from.</param>
        /// <returns>The index of a RecipeSequence.</returns>
        public int GetIndexOfPuzzle(RecipeSequence sequence)
            => _sequences.Where(s => s is RecipeSequence)
                .ToList()
                .IndexOf(sequence);
    
        /// <summary>
        /// Retrieves the index of the current puzzle.
        /// <seealso cref="GetIndexOfPuzzle"/>
        /// </summary>
        /// <returns>The index of the current puzzle.</returns>
        public int GetIndexOfCurrentPuzzle() => CurrentSequence is RecipeSequence s ? GetIndexOfPuzzle(s) : -1;

        /// <summary>
        /// Progresses to the next sequence.
        /// </summary>
        /// <returns>Whether there was a next sequence.</returns>
        public bool Next()
        {
            if (_currentStep + 1 >= _sequences.Count)
            {
                ForceFadeOut();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                return false;
            }

            var next = _sequences[_currentStep + 1];
            return ProgressSequence(next);
        }

        /// <summary>
        /// Checks what type of sequence the next sequence is
        /// and calls its corresponding progression-function.
        /// </summary>
        /// <param name="sequence">The next sequence to be started.</param>
        /// <returns>Whether there was a next sequence.</returns>
        private bool ProgressSequence(IGameSequence sequence)
            => sequence switch
            {
                TransitionSequence t => ProgressTransition(t),
                ParallelSequencer s => ProgressParallel(s),
                _ => ProgressDefault()
            };

        /// <summary>
        /// Starts a <see cref="TransitionSequence"/>.
        /// </summary>
        /// <param name="t">The <see cref="TransitionSequence"/> that is being started.</param>
        /// <returns>True, always.</returns>
        private bool ProgressTransition(TransitionSequence t)
        {
            StartCoroutine(ProgressTransition1(t));
            return true;
        }

        /// <summary>
        /// Starts a <see cref="ParallelSequencer"/>.
        /// </summary>
        /// <param name="s">The <see cref="ParallelSequencer"/> that is being started.</param>
        /// <returns>Whether the next sequence could be started.</returns>
        private bool ProgressParallel(ParallelSequencer s)
        {
            CurrentSequence.OnDisabled();
            _currentStep += 1;
            s.OnEnabled();
            s.OnDisabled();
            return Next();
        }

        /// <summary>
        /// Progresses a default <see cref="IGameSequence"/>.
        /// </summary>
        /// <returns>Whether there was a next sequence.</returns>
        private bool ProgressDefault()
        {
            CurrentSequence.OnDisabled();
            _currentStep++;
            CurrentSequence.OnEnabled();
            return true;
        }

        /// <summary>
        /// Coroutine that calls the <see cref="TransitionSequence.BeginTransition"/>
        /// Coroutine.
        /// </summary>
        /// <param name="t">The <see cref="TransitionSequence"/> that is being started.</param>
        private IEnumerator ProgressTransition1(TransitionSequence t)
        {
            _currentStep++;
            yield return t.BeginTransition(_sequences.ElementAtOrDefault(_currentStep - 1),
                () => Next());
        }

        /// <summary>
        /// Sets the currently displayed sequence.
        /// </summary>
        /// <param name="ix">The index of the sequence to display.</param>
        public void SetSequence(int ix)
        {
            CurrentSequence.OnDisabled();
            _currentStep = ix;
            CurrentSequence.OnEnabled();
        }

        /// <summary>
        /// Creates a fade-in effect.
        /// </summary>
        public void ForceFadeIn() => StartCoroutine(ForceFadeInCoroutine());
    
        /// <summary>
        /// Creates a fade-out effect.
        /// </summary>
        public void ForceFadeOut() => StartCoroutine(ForceFadeOutCoroutine());
    

        /// <summary>
        /// Coroutine for the fade-in effect.
        /// </summary>
        private IEnumerator ForceFadeInCoroutine()
        {
            genericFader.gameObject.SetActive(true);
            yield return genericFader.FadeIn();
            genericFader.gameObject.SetActive(false);
        }

        /// <summary>
        /// Coroutine for the fade-out effect.
        /// </summary>
        private IEnumerator ForceFadeOutCoroutine()
        {
            genericFader.gameObject.SetActive(true);
            yield return genericFader.FadeOut();
        }

    }
}