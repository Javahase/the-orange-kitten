using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class ChatData
{
    [JsonProperty(propertyName: "sender")] public string Sender;

    [JsonProperty(propertyName: "message")]
    public string Message;

    public string ToJsonString() => JsonConvert.SerializeObject(this);
}