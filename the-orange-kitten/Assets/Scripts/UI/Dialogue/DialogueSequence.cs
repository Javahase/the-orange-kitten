using System;
using System.Collections.Generic;
using System.Linq;
using Sequence;
using UnityEngine;

public class DialogueSequence : LinearGameSequence<CharacterFrame>
{
    
    protected bool Initialized;
    
    protected override List<CharacterFrame> SequencePoints { get; set; }

    public override void OnInitialize()
    {
        if (Initialized)
        {
            return;
        }
        SequencePoints = GetComponentsInChildren<CharacterFrame>(includeInactive: true).ToList();
        SequencePoints.ForEach(frame =>
        {
            frame.OnInitialize();
            frame.gameObject.SetActive(false);
        });
        Initialized = true;
    }

    public override void OnEnabled()
    {
        gameObject.SetActive(true);
        CurrentSequence.OnEnabled();
    }

    public override void OnDisabled()
    {
        ResetSequence();
        gameObject.SetActive(false);
    }
    
    public override void ResetSequence()
    {
        CurrentSequence.OnDisabled();
        base.ResetSequence();
    }
}
