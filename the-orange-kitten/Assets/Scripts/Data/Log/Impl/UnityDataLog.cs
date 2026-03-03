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

        if (_currentPuzzle != null)
        {
            Debug.Log($"[{ev}/{_currentPuzzle}]: {payload}");
        }
        else
        {
            Debug.Log($"[{ev}]: {payload}");
        }
    }
}