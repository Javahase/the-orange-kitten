using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class OddStatement
{
    [JsonProperty(propertyName: "topic")] public string Topic;
    [JsonProperty(propertyName: "reason")]  public string Reason;
    
    public string ToJson() => JsonConvert.SerializeObject(this);
}