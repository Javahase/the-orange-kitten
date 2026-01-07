using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO: Group Sequences together, Create Sequence Manager, let Sequences tell Sequence Manager when to progress to next.
public class DialogueSequence : LinearGameSequence<BaseFrame>
{

    protected override List<BaseFrame> SequencePoints { get; set; }
    private GameSequenceController _sequenceController;

    public override void OnInitialize()
    {
        _sequenceController = GameSequenceController.GetInstance();
        SequencePoints = GetComponentsInChildren<BaseFrame>(includeInactive: true).ToList();
        SequencePoints.ForEach(frame =>
        {
            frame.OnInitialize();
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

    protected override void OnSequenceEnded()
    {
        _sequenceController.Next();
    }

    public override void ResetSequence()
    {
        CurrentSequence.OnDisabled();
        base.ResetSequence();
    }
}
