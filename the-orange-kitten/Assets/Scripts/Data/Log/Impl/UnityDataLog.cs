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

        public override void LogString(string message) => Debug.Log(message);

        public override void Init(string participantId)
        {
            Log(DataEvent.SessionStart, $"Starting session for {participantId}.");
        }
    }
}