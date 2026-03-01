using UnityEngine;

public class ParallelSequencer : MonoBehaviour, IGameSequence
{
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