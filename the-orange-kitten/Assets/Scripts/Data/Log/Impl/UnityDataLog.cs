using System;
using UnityEngine;

public class UnityDataLog : DataLog
{
    private string _currentPuzzle = null;

    public override void Log(DataEvent ev, string payload)
    {
        if (ev == DataEvent.PuzzleStart)
        {
            _currentPuzzle = payload;
        }

        var currentTime = DateTime.Now.ToString("HH:mm:ss");

        Debug.Log(_currentPuzzle != null
            ? $"[{currentTime}] [{ev}/{_currentPuzzle}]: {payload}"
            : $"[{currentTime}] [{ev}]: {payload}");
    }
}