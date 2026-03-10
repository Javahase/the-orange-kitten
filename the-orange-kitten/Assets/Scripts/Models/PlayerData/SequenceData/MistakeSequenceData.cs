using Newtonsoft.Json;
using UI.RecipeSequence.SequenceTypes;

namespace Models.PlayerData.SequenceData
{
    /// <summary>
    /// Represents the results of a <see cref="MistakeSequenceHandler">Mistake Sequence</see>.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class MistakeSequenceData : PuzzleData
    {
        /// <summary>
        /// The type of sequence this puzzle is.
        /// </summary>
        [JsonProperty(propertyName: "type")] public const string Type = "MistakeSequence";
        /// <summary>
        /// The insight used to win this puzzle.
        /// </summary>
        [JsonProperty(propertyName: "contradictReason")] public string ContradictReason;
    
        /// <summary>
        /// Converts this object into a JSON-String.
        /// </summary>
        /// <returns>A JSON-String.</returns>
        public string ToJson() => JsonConvert.SerializeObject(this);
    }
}