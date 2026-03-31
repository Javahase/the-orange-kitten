using System.IO;
using UnityEngine;

namespace Data.Log.Impl
{
    public class FileDataLog : DataLog
    {
        private string _logFilePath;

        public override void Init(string participantId)
        {
            _logFilePath = Path.GetFullPath(Path.Combine(Application.persistentDataPath, $"{participantId}.log"));
            if (File.Exists(_logFilePath))
            {
                File.Delete(_logFilePath);
            }
            Log(DataEvent.SessionStart, $"Starting session for {participantId}");
        }

        public override void LogString(string message)
        {
            using var file = new StreamWriter(_logFilePath, true);
            file.WriteLine(message);
        }
    }
}