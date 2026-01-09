using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueSequence : LinearGameSequence<BaseFrame>
{

    protected override List<BaseFrame> SequencePoints { get; set; }

    public override void OnInitialize()
    {
        SequencePoints = GetComponentsInChildren<BaseFrame>(includeInactive: true).ToList();
        SequencePoints.ForEach(frame =>
        {
            frame.OnInitialize();
            frame.gameObject.SetActive(false);
        });
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
