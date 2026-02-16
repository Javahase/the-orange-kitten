using System.Collections.Generic;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class ConversationData
{
    [JsonProperty(propertyName: "topic")] public string Topic;
    [JsonProperty(propertyName: "chat")] public List<ChatData> Chat;
    
    public string ToJson() => JsonConvert.SerializeObject(this);
}