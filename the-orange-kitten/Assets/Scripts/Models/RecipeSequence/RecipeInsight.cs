
using System;

/// <summary>
/// A <c>RecipeInsight</c> models a potential puzzle piece
/// for solving a recipe sequence.
/// </summary>
[Serializable] 
public struct RecipeInsight
{
    
    /// <summary>
    /// Empty placeholder insight that implies the lack of an insight.
    /// </summary>
    public static readonly RecipeInsight EMPTY = new("", -1);

    /// <summary>
    /// The information that can be displayed by the insight.
    /// </summary>
    public string info;

    /// <summary>
    /// Which sequence this insight might be a solution to. (-1 for no solution).
    /// </summary>
    public int correctSequence;

    public RecipeInsight(string info, int correctSequence)
    {
        this.info = info;
        this.correctSequence = correctSequence;
    }

    public override bool Equals(object obj)
    {
        if (obj is RecipeInsight insight)
        {
            return info == insight.info && correctSequence == insight.correctSequence;
        }

        return false;
    }
}
