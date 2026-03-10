using Newtonsoft.Json;
using UI.RecipeSequence.SequenceTypes;

namespace Models.PlayerData.OddSequenceData
{
    /// <summary>
    /// Represent data for an odd statement within an <see cref="OddSequenceHandler">Odd Sequence</see>.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class OddStatement
    {
        /// <summary>
        /// The content of the statement.
        /// </summary>
        [JsonProperty(propertyName: "topic")] public string Topic;
        /// <summary>
        /// The reason this statement has been marked as odd.
        /// </summary>
        [JsonProperty(propertyName: "reason")]  public string Reason;
    
        /// <summary>
        /// Converts this object into a JSON-String.
        /// </summary>
        /// <returns>A JSON-String.</returns>
        public string ToJson() => JsonConvert.SerializeObject(this);
    }
}