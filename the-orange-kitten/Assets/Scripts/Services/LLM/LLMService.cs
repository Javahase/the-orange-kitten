using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Provides functionality of an LLM, handling chat connections
/// and saving the current recipe sequence as a list of statements.
/// </summary>
public abstract class LLMService : SingletonObject<LLMService>
{

    /// <summary>
    /// Sets the current recipe statements for the LLM to consider.
    /// </summary>
    /// <param name="statements">The list of recipe statements.</param>
    public abstract void InitStatements(List<RecipeStatement> statements);
    
    /// <summary>
    /// Clears the list of recipe statements.
    /// </summary>
    public abstract void ClearStatements();
    
    /// <summary>
    /// Creates a chat connection, if it doesn't exist,
    /// and expands on an existing one, if it does.
    /// </summary>
    /// <param name="stmt">The statement the connection belongs to</param>
    /// <param name="request">The message to be sent to the LLM</param>
    /// <param name="callback">The function that is called when the LLM made a response</param>
    /// <returns>An IEnumerator</returns>
    public abstract IEnumerator SendMessageToConversation(RecipeStatement stmt, string request, Action<string> callback);
    
    public abstract List<RecipeStatement> GetStatements();
}
