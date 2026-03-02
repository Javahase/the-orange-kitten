using System;
using UnityEngine;

/// <summary>
/// A <c>RecipeStatement</c> provides one statement section for
/// a recipe sequence.
/// </summary>
[Serializable]
public class RecipeStatement
{

    /// <summary>
    /// A message showcasing the step that was performed.
    /// </summary>
    [TextArea] public string message;

    /// <summary>
    /// Whether the recipe sequence should display this statement.
    /// </summary>
    public bool hidden;

    /// <summary>
    /// Whether this statement was marked as odd by the player.
    /// </summary>
    public MarkingState markingState;

    /// <summary>
    /// Which <c>StatementType</c> the recipe is.
    /// </summary>
    /// <seealso cref="StatementType"/>
    public StatementType statementType;

    /// <summary>
    /// A Statement may be <see cref="Correct"/>, <see cref="TechnicallyTrue"/> or <see cref="Incorrect"/>
    /// </summary>
    [Serializable]
    public enum StatementType
    {   
        /// <summary>
        /// A correct statement is a statement that needs
        /// no further pressing and should be equal to
        /// the corresponding real recipe step.
        /// </summary>
        Correct, 
        
        /// <summary>
        /// A technically-true statement is a statement
        /// that does fulfill the requirements of the
        /// real recipe step, however, the solution
        /// was performed in a terrible way.
        /// <example>
        /// "To cool down the ice, I traveled all the way to antarctica and placed it there."
        /// </example>
        /// </summary>
        TechnicallyTrue, 
        
        /// <summary>
        /// A statement that does not fulfill the requirements
        /// of the real recipe step or even a misstep.
        /// </summary>
        Incorrect
    }

    [Serializable]
    public enum MarkingState
    {
        Unmarked = 0,
        Pending = 1,
        Marked = 2
    }
}  
