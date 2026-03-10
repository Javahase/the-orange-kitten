using System.Collections.Generic;
using Models.PlayerData.Conversation;
using Models.RecipeSequence;
using Newtonsoft.Json;

namespace Models.PlayerData
{
    /// <summary>
    /// Represents an entire level the player plays through.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PuzzleData
    {
        /// <summary>
        /// The ID of the level.
        /// </summary>
        [JsonProperty(propertyName: "id")] public string Id;

        /// <summary>
        /// The amount of questions the player needed to pass through this level.
        /// </summary>
        [JsonProperty(propertyName: "questionCount")]
        public int QuestionCount;

        /// <summary>
        /// The amount of mice-catcher instances the player encountered, aka loss-screens.
        /// </summary>
        [JsonProperty(propertyName: "attempts")]
        public int Attempts;

        /// <summary>
        /// The chat-histories of the sequences.
        /// </summary>
        [JsonProperty(propertyName: "conversationData")]
        public List<ConversationData> ConversationData;

        /// <summary>
        /// The entire insight inventory.
        /// </summary>
        [JsonProperty(propertyName: "generatedInsights")]
        public List<RecipeInsight> GeneratedInsights;
    }
}