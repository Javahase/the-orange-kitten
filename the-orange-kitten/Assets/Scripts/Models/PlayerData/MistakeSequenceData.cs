using System.Collections.Generic;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class MistakeSequenceData : PuzzleData
{
    [JsonProperty(propertyName: "type")] public const string Type = "MistakeSequence";
    [JsonProperty(propertyName: "contradictReason")] public string ContradictReason;
    
    public string ToJson() => JsonConvert.SerializeObject(this);
}