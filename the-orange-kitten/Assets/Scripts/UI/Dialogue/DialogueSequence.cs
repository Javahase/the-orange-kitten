using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO: Group Sequences together, Create Sequence Manager, let Sequences tell Sequence Manager when to progress to next.
public class DialogueSequence : MonoBehaviour
{

    private List<BaseFrame> _frames;
    private int _currentFrameIndex;

    private void Start()
    {
        _frames = GetComponentsInChildren<BaseFrame>(includeInactive: true).ToList();
        _frames.ForEach(frame =>
        {
            frame.Init();
            frame.gameObject.SetActive(false);
        });
        gameObject.SetActive(false);
    }

    public void StartSequence()
    {
        gameObject.SetActive(true);
        _frames[_currentFrameIndex].HandleSelected();
    }

    public void NextFrame()
    {
        _frames[_currentFrameIndex].HandleDeselected();
        _currentFrameIndex += 1;
        _frames[_currentFrameIndex].HandleSelected();
    }
}
