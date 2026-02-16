using System.Collections.Generic;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public struct OddSequenceData
{
    [JsonProperty(propertyName: "id")] public string Id;
    [JsonProperty(propertyName: "type")] public const string Type = "OddSequence";
    [JsonProperty(propertyName: "questionCount")] public int QuestionCount;
    [JsonProperty(propertyName: "attempts")] public int Attempts;
    [JsonProperty(propertyName: "oddStatements")] public List<OddStatement> OddStatements;
    [JsonProperty(propertyName: "evenStatements")] public List<string> EvenStatements;
    [JsonProperty(propertyName: "generatedInsights")] public List<string> GeneratedInsights;
    
    public string ToJson() => JsonConvert.SerializeObject(this);
}