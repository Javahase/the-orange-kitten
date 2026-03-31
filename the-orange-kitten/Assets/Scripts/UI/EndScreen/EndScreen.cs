using System;
using Data.Collector;
using Data.Log;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    private void Start()
    {
        DataLog.GetInstance().Log(DataLog.DataEvent.SessionEnd, $"Session ended for {DataCollector.GetInstance().participantId}");
    }

    public void EndProgram() => Application.Quit();
}
