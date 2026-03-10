using System;
using System.Collections;
using System.Collections.Generic;
using Models.RecipeSequence;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Services.Insights.impl
{
    /// <summary>
    /// Fake insight service.
    /// </summary>
    public class MockInsightService : InsightService
    {
        /// <summary>
        /// A pool of insights that can be generated from.
        /// </summary>
        [SerializeField] private List<RecipeInsight> insightPool;

        /// <summary>
        /// How the insight pool should be accessed.
        /// </summary>
        [SerializeField] private GenerationType mode;

        public override void SetTopicPrompt(string _)
        {
        }

        public override IEnumerator GenerateInsight(RecipeStatement context, string question, string statement)
        {
            switch (mode)
            {
                case GenerationType.None:
                    break;
                case GenerationType.Random:
                    GenRandom();
                    break;
                case GenerationType.All:
                    GenAll();
                    break;
            }

            yield break;
        }

        private void GenAll()
        {
            foreach (var insight in insightPool)
            {
                AddInsight(insight);
            }

            insightPool.Clear();
        }

        private void GenRandom()
        {
            if (insightPool.Count == 0) return;
            var ix = Random.Range(0, insightPool.Count);
            var insight = insightPool[ix];
            AddInsight(insight);
            insightPool.RemoveAt(ix);
        }

        [Serializable]
        public enum GenerationType
        {
            None,
            Random,
            All
        }
    }
}