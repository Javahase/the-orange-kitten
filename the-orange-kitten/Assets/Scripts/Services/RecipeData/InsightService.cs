using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InsightService : SingletonObject<InsightService>
{

    [SerializeField]
    private List<RecipeInsight> _defaultInsights;
    private List<RecipeInsight> _recipeInsights = new();

    void Start()
    {
        _recipeInsights.AddRange(_defaultInsights);
    }

    public void AddInsight(RecipeInsight insight)
    {
        _recipeInsights.Add(insight);
    }

    public void RemoveUselessInsights(int progression)
    {
        _recipeInsights = (from insight in _recipeInsights
                           where insight.correctSequence >= progression || insight.correctSequence == -1
                           select insight).ToList();
    }

    public List<RecipeInsight> GetAllInsights()
    {
        return _recipeInsights;
    }

    public List<RecipeInsight> GetPaged(int pageSize, int page)
    {
        var maxTargetPossible = Math.Clamp(_recipeInsights.Count - page * pageSize, 0, 9);
        return _recipeInsights.GetRange(page * pageSize, maxTargetPossible);
    }
}
