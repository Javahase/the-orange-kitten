using System;
using UnityEngine;

namespace Data.Log.Impl
{
    /// <summary>
    /// The Unity Data Log logs all information within the Unity Console.
    /// <seealso cref="DataLog"/>
    /// </summary>
    public class UnityDataLog : DataLog
    {
        private string _currentPuzzle;

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
}