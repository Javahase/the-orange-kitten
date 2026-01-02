using System;

[Serializable]
public class RecipeStatement
{

    public string message;

    public bool hidden;

    public StatementType statementType;

    [Serializable]
    public enum StatementType
    {
        Correct, TechnicallyTrue, Incorrect
    }
}  
