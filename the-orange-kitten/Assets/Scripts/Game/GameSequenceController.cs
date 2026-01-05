using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSequenceController : SingletonObject<GameSequenceController>
{
    private List<IGameSequence> _sequences;
    private int _currentStep = 0;
    private IGameSequence CurrentSequence => _sequences[_currentStep];

    private void Start()
    {
        _sequences = GetComponentsInChildren<IGameSequence>(true).ToList();
        Debug.Log(_sequences.Count);
        _sequences.ForEach(seq => seq.OnInitialize());
        CurrentSequence.OnEnabled();
    }

    public bool Next()
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