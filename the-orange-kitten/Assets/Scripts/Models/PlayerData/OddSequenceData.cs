using System.Collections.Generic;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class OddSequenceData : PuzzleData
{
    [JsonProperty(propertyName: "type")] public const string Type = "OddSequence";
    [JsonProperty(propertyName: "oddStatements")] public List<OddStatement> OddStatements;
    
    public string ToJson() => JsonConvert.SerializeObject(this);
}