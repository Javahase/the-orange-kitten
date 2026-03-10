using System.Collections.Generic;
using Newtonsoft.Json;

namespace Models.PlayerData
{
    /// <summary>
    /// Represents a players' play-through.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PlayerData
    {
        /// <summary>
        /// The participant ID of the player.
        /// </summary>
        [JsonProperty(propertyName: "participantId")] 
        public string ParticipantId;

        /// <summary>
        /// Every puzzle in the game.
        /// </summary>
        [JsonProperty(propertyName: "levelData")]
        public List<PuzzleData> LevelData;
    }
}