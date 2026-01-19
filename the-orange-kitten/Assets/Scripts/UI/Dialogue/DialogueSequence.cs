using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueSequence : LinearGameSequence<BaseFrame>
{

    protected bool Initialized;
    
    protected override List<BaseFrame> SequencePoints { get; set; }

    public override void OnInitialize()
    {
        if (Initialized)
        {
            return;
        }
        SequencePoints = GetComponentsInChildren<BaseFrame>(includeInactive: true).ToList();
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
