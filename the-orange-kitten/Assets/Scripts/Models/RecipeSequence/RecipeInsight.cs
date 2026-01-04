
using System;

[Serializable]
public struct RecipeInsight
{
    
    public static readonly RecipeInsight EMPTY = new("", -1);

    public string info;
    public int correctSequence;

    public RecipeInsight(string info, int correctSequence)
    {
        this.info = info;
        this.correctSequence = correctSequence;
    }
}
