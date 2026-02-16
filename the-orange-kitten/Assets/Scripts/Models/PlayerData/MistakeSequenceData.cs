using System.Collections.Generic;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public struct MistakeSequenceData
{
    [JsonProperty(propertyName: "id")] public string Id;
    [JsonProperty(propertyName: "type")] public const string Type = "MistakeSequence";
    [JsonProperty(propertyName: "questionCount")] public int QuestionCount;
    [JsonProperty(propertyName: "attempts")] public int Attempts;
    [JsonProperty(propertyName: "contradictReason")] public string ContradictReason;
    [JsonProperty(propertyName: "generatedInsights")] public List<string> GeneratedInsights;
    
    public string ToJson() => JsonConvert.SerializeObject(this);
}