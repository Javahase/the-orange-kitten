using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// An <c>IGameSequence</c> is the abstraction of a step
/// within the game progression.
/// </summary>
/// <example>
/// Dialogue Sequences, Recipe Sequences.
/// </example>
public interface IGameSequence : IInitializable
{ 
    /// <summary>
    /// Resets a game sequence back to its original state.
    /// </summary>
    public abstract void ResetSequence();
}

/// <summary>
/// A transition sequence is a special form of game sequence
/// that happens inbetween two game sequences. The use-case
/// for such a sequence is, usually, a smooth visual transition
/// between two game sequences.
/// </summary>
public abstract class TransitionSequence : MonoBehaviour, IGameSequence
{
    public abstract void ResetSequence();
    
    /// <summary>
    /// Begins a transition between two sequences. <br/>
    /// <b>Note that this function is called as a Coroutine</b>
    /// </summary>
    /// <param name="previous">The game sequence before this transition sequence</param>
    /// <param name="next">The game sequence after this transition sequence</param>
    /// <returns>An IEnumerator used by the Coroutine.</returns>
    public abstract IEnumerator BeginTransition(IGameSequence previous, IGameSequence next);
    public abstract void OnInitialize();
    public void OnEnabled() {}
    public void OnDisabled() {}
}

/// <summary>
/// A Game Sequence consists of many objects that can be iterated through.
/// </summary>
/// <typeparam name="T">The type of objects this sequence deals with.</typeparam>
public abstract class GameSequence<T> : MonoBehaviour, IGameSequence
{
    
    public delegate void OnStepChanged(GameSequence<T> source, T oldSequence, T newSequence);
    
    public event OnStepChanged StepChangeEvent;
    
    /// <summary>
    /// A list of points this sequence can iterate through.
    /// </summary>
    protected abstract List<T> SequencePoints { get; set; }
    
    /// <summary>
    /// The current point this sequence is pointing to.
    /// </summary>
    protected T CurrentSequence => SequencePoints[CurrentStep];
    
    /// <summary>
    /// The index of the current point this sequence is pointing to.
    /// Modifying this modifies <see cref="CurrentSequence"/>.
    /// </summary>
    protected int CurrentStep;

    /// <summary>
    /// Called when <see cref="Next"/> or
    /// <see cref="Previous"/> is called.
    /// </summary>
    public virtual void OnStepChange() {}
    public abstract void OnEnabled();
    public abstract void OnDisabled();

    public abstract void OnInitialize();

    /// <summary>
    /// Sets the <see cref="CurrentSequence"/> to its next sequence point.
    /// </summary>
    /// <returns>Whether there was a next sequence point.</returns>
    public virtual bool Next()
    {
        StepChangeEvent?.Invoke(this, SequencePoints.ElementAtOrDefault(CurrentStep - 1), CurrentSequence);
        return true;
    }

    /// <summary>
    /// Sets the <see cref="CurrentSequence"/> to its previous sequence point.
    /// </summary>
    /// <returns>Whether there was a previous sequence point.</returns>
    public virtual bool Previous()
    {
        StepChangeEvent?.Invoke(this, SequencePoints.ElementAtOrDefault(CurrentStep + 1), CurrentSequence);
        return true;
    }
    public abstract void ResetSequence();
}

/// <summary>
/// A linear game sequence may only iterate through its objects
/// in one direction: the next-direction.
/// </summary>
/// <typeparam name="T">The type of objects this sequence deals with.</typeparam>
public abstract class LinearGameSequence<T> : GameSequence<T> where T : IInitializable
{

    /// <summary>
    /// Called when this sequence ends, i.e. when the last SequencePoint
    /// was reached by <see cref="Next"/>.
    /// </summary>
    public delegate void OnSequenceEnded(LinearGameSequence<T> source);

    public event OnSequenceEnded SequenceEndEvent;

    public override bool Next()
    {
        if (CurrentStep + 1 >= SequencePoints.Count)
        {
            SequenceEndEvent?.Invoke(this);
            return false;
        }
        
        SequencePoints[CurrentStep].OnDisabled();
        CurrentStep++;
        SequencePoints[CurrentStep].OnEnabled();
        return base.Next();
    }

    public override bool Previous() => false;

    public override void ResetSequence()
    {
        CurrentStep = 0;
    }
}