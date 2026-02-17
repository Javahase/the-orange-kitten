using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MockInsightService : InsightService
{
    [SerializeField] private List<RecipeInsight> insightPool;
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