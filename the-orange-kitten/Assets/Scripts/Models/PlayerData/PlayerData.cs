using System.Collections.Generic;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class PlayerData
{
    [JsonProperty(propertyName: "participantId")] 
    public string ParticipantId;

    [JsonProperty(propertyName: "levelData")]
    public List<PuzzleData> LevelData;
}