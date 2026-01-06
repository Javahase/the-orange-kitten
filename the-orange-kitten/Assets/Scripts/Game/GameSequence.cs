using System.Collections.Generic;
using UnityEngine;

public interface IGameSequence : IInitializable
{ 
    public abstract void ResetSequence();
}

public abstract class TransitionSequence : MonoBehaviour, IGameSequence
{
    public abstract void ResetSequence();
    public abstract bool BeginTransition(IGameSequence previous, IGameSequence next);
    public abstract void OnInitialize();

    public virtual void OnEnabled() {}
    public virtual void OnDisabled() {}
}

public abstract class GameSequence<T> : MonoBehaviour, IGameSequence
{
    protected abstract List<T> SequencePoints { get; set; }
    protected T CurrentSequence => SequencePoints[CurrentStep];
    protected int CurrentStep;

    public virtual void OnStepChange() {}
    public abstract void OnEnabled();
    public abstract void OnDisabled();
    public abstract void OnInitialize();
    public abstract bool Next();
    public abstract bool Previous();
    public abstract void ResetSequence();
}

public abstract class LinearGameSequence<T> : GameSequence<T> where T : IInitializable
{
    protected abstract void OnSequenceEnded();

    public override bool Next()
    {
        SequencePoints[CurrentStep].OnDisabled();
        CurrentStep++;
        if (CurrentStep >= SequencePoints.Count)
        {
            OnSequenceEnded();
            return false;
        }

        SequencePoints[CurrentStep].OnEnabled();
        OnStepChange();
        return true;
    }

    public override bool Previous() => false;

    public override void ResetSequence()
    {
        CurrentStep = 0;
    }
}