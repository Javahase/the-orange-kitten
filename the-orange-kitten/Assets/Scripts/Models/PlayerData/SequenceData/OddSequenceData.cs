using System.Collections.Generic;
using Models.PlayerData.OddSequenceData;
using Newtonsoft.Json;
using UI.RecipeSequence.SequenceTypes;

namespace Models.PlayerData.SequenceData
{
    /// <summary>
    /// Represents the result of an <see cref="OddSequenceHandler">Odd Sequence</see>.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class OddSequenceData : PuzzleData
    {
        /// <summary>
        /// The type of sequence this puzzle is.
        /// </summary>
        [JsonProperty(propertyName: "type")] public const string Type = "OddSequence";
        
        /// <summary>
        /// All statements within this puzzle marked as odd.
        /// </summary>
        [JsonProperty(propertyName: "oddStatements")] public List<OddStatement> OddStatements;
    
        /// <summary>
        /// Converts this object into a JSON-String.
        /// </summary>
        /// <returns>A JSON-String.</returns>
        public string ToJson() => JsonConvert.SerializeObject(this);
    }
}