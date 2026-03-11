using System;
using Newtonsoft.Json;

namespace Models.RecipeSequence
{
    /// <summary>
    /// A <c>RecipeInsight</c> models a potential puzzle piece
    /// for solving a recipe sequence.
    /// </summary>
    [Serializable] 
    [JsonObject(MemberSerialization.OptIn)]
    public struct RecipeInsight
    {
    
        /// <summary>
        /// Empty placeholder insight that implies the lack of an insight.
        /// </summary>
        public static readonly RecipeInsight EMPTY = new("", false);

        /// <summary>
        /// The information that can be displayed by the insight.
        /// </summary>
        [JsonProperty]
        public string info;

        /// <summary>
        /// Whether this insight is a solution to this puzzle sequence.
        /// </summary>
        [JsonProperty]
        public bool important;

        public RecipeInsight(string info, bool important)
        {
            this.info = info;
            this.important = important;
        }

        public override bool Equals(object obj)
        {
            if (obj is RecipeInsight insight)
            {
                return info == insight.info && important == insight.important;
            }

            return false;
        }
    }
}
