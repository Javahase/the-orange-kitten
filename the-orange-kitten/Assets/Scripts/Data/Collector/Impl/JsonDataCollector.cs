using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// Collects player metrics and saves them in JSON-Format.
///
/// Location of the JSON File: <see cref="Application.persistentDataPath"/>/<c>&lt;ParticipantID&gt;.json</c>
/// </summary>
public class JsonDataCollector : DataCollector
{
    private PlayerData _playerData;
    public override void Init(string participantId)
    {
        ParticipantId = participantId;
        if (_playerData != null) return;

        _playerData = new PlayerData()
        {
            ParticipantId = participantId,
            LevelData = new List<PuzzleData>()
        };
        var path = Path.GetFullPath(Path.Combine(Application.persistentDataPath, $"{participantId}.json"));
        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }
        Save();
        
    }

    public override void StartLevel(PuzzleData data)
    {
        _playerData.LevelData.Add(data);
    }

    public override void Save()
    { 
        using var file = new StreamWriter(Path.Combine(Application.persistentDataPath, $"{ParticipantId}.json"), false);
        var str = JsonConvert.SerializeObject(_playerData, Formatting.Indented);
        file.Write(str);
    }
}