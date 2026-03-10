using Newtonsoft.Json;

namespace Models.PlayerData.Conversation
{
    /// <summary>
    /// Represents a chat message. Can be serialized into JSON.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ChatData
    {
        /// <summary>
        /// Who sent the message.
        /// </summary>
        [JsonProperty(propertyName: "sender")] public string Sender;

        /// <summary>
        /// The content of the message.
        /// </summary>
        [JsonProperty(propertyName: "message")]
        public string Message;

        /// <summary>
        /// Converts the message into a JSON-String.
        /// </summary>
        /// <returns>A JSON-String.</returns>
        public string ToJsonString() => JsonConvert.SerializeObject(this);
    }
}