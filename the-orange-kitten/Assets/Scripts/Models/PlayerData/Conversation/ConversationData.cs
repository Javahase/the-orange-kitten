using System.Collections.Generic;
using Newtonsoft.Json;

namespace Models.PlayerData.Conversation
{
    /// <summary>
    /// Represents a list of chat messages to a certain topic. Can be serialized into JSON.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ConversationData
    {
        /// <summary>
        /// The topic of the chat.
        /// </summary>
        [JsonProperty(propertyName: "topic")] public string Topic;
        /// <summary>
        /// The list of <see cref="ChatData">chat message data entries</see>.
        /// </summary>
        [JsonProperty(propertyName: "chat")] public List<ChatData> Chat;
    
        /// <summary>
        /// Converts this object into a JSON String.
        /// </summary>
        /// <returns>A JSON String.</returns>
        public string ToJson() => JsonConvert.SerializeObject(this);
    }
}