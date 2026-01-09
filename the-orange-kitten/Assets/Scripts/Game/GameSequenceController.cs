using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The Game Sequence controller is responsible for the core
/// gameplay interaction. It contains subsequences that can call
/// upon the Game Sequence Controller to enter the next sequence,
/// once the subsequence is done.
/// </summary>
public class GameSequenceController : SingletonObject<GameSequenceController>
{
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
    private IGameSequence CurrentSequence => _sequences[_currentStep];

    private void Start()
    {
        _sequences = GetComponentsInChildren<IGameSequence>(true).ToList();
        Debug.Log(_sequences.Count);
        _sequences.ForEach(seq => seq.OnInitialize());
        _sequences.Where(seq => seq is DialogueSequence)
            .Cast<DialogueSequence>()
            .ToList()
            .ForEach(seq => seq.SequenceEndEvent += (_) => Next());
        CurrentSequence.OnEnabled();
    }

    /// <summary>
    /// Progresses to the next sequence.
    /// </summary>
    /// <returns>Whether there was a next sequence.</returns>
    public bool Next()
    {
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
    /// Coroutine that calls the <see cref="TransitionSequence.BeginTransition"/>
    /// Coroutine.
    /// </summary>
    /// <param name="t">The <see cref="TransitionSequence"/> that is being started.</param>
    private IEnumerator ProgressTransition1(TransitionSequence t)
    {
        _currentStep++;
        yield return t.BeginTransition(_sequences.ElementAtOrDefault(_currentStep - 1), 
            _sequences.ElementAtOrDefault(_currentStep + 1));
        _currentStep++;
    }
    
    /// <summary>
    /// Progresses a default <see cref="IGameSequence"/>.
    /// If the sequence is a <see cref="MonoBehaviour"/>,
    /// it hides the previous sequence.
    /// </summary>
    /// <returns>Whether there was a next sequence.</returns>
    private bool ProgressDefault()
    {
        CurrentSequence.OnDisabled();
        if (CurrentSequence is MonoBehaviour monoBehaviour)
            monoBehaviour.gameObject.SetActive(false);
        _currentStep++;
        if (_currentStep >= _sequences.Count)
        {
            Debug.Log("Sorry, pal, last sequence");
            return false;
        }
        CurrentSequence.OnEnabled();
        return true;
    }


}