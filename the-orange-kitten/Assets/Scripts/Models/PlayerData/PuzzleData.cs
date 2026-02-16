using System.Collections.Generic;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class PuzzleData
{
    [JsonProperty(propertyName: "id")] public string Id;
    [JsonProperty(propertyName: "questionCount")] public int QuestionCount;
    [JsonProperty(propertyName: "attempts")] public int Attempts;
    [JsonProperty(propertyName: "conversationData")] public List<ConversationData> ConversationData;
    [JsonProperty(propertyName: "generatedInsights")] public List<string> GeneratedInsights;
}