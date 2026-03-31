using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models.RecipeSequence;
using Services.LLM;
using UI.RecipeSequence;
using UnityEngine;

namespace Services.Insights
{
    /// <summary>
    /// Persists a bunch of insights, until the current scene ends.
    /// </summary>
    /// <seealso cref="RecipeInsight"/>
    public abstract class InsightService : SingletonObject<InsightService>
    {
    
        public delegate void InsightGained(RecipeInsight insight);
    
        public event InsightGained OnInsightGained;

        /// <summary>
        /// Base insights that the player has available right away.
        /// </summary>
        [SerializeField]
        private List<RecipeInsight> defaultInsights;
        /// <summary>
        /// The actual insight pool.
        /// </summary>
        private List<RecipeInsight> _recipeInsights = new();

        protected LLMService LlmService;

        protected virtual void Start()
        {
            _recipeInsights.AddRange(defaultInsights);
            LlmService = LLMService.GetInstance();
        }

        /// <summary>
        /// Sets the topic prompt for this insight LLM to consider.
        /// {0} yields a list of statements from the current <see cref="RecipeSequence"/>,
        /// whilst {1} yields the incorrect statement from the recipe sequence.
        /// </summary>
        /// <param name="topicPrompt">The prompt data.</param>
        public abstract void SetTopicPrompt(string topicPrompt);

        public abstract IEnumerator GenerateInsight(RecipeStatement context, string question, string statement, bool notify);
    
        /// <summary>
        /// Adds a <see cref="RecipeInsight"/> to the pool.
        /// </summary>
        /// <param name="insight">The recipe insight.</param>
        /// <param name="notify">Whether a notification about the newly added insight should be shown.</param>
        public void AddInsight(RecipeInsight insight, bool notify = true)
        {
            _recipeInsights.Add(insight);
            if (notify)
                OnInsightGained?.Invoke(insight);
        }

        public void ClearAllInsights() => _recipeInsights.Clear();
    
        /// <summary>
        /// Gets all insights.
        /// </summary>
        /// <returns>All insights.</returns>
        public List<RecipeInsight> GetAllInsights()
        {
            return _recipeInsights;
        }
    
        /// <summary>
        /// Gets a certain section of the insights, useful for the <see cref="InsightsUI"/>.
        /// </summary>
        /// <param name="pageSize">How many insights fit onto a page.</param>
        /// <param name="page">Which page should be retrieved.</param>
        /// <returns>A certain section of the insights.</returns>
        public List<RecipeInsight> GetPaged(int pageSize, int page)
        {
            var maxTargetPossible = Math.Clamp(_recipeInsights.Count - page * pageSize, 0, 9);
            return _recipeInsights.GetRange(page * pageSize, maxTargetPossible);
        }
    }
}
