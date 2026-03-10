using System.Collections;
using System.Collections.Generic;
using Models.RecipeSequence;

namespace Services.LLM
{
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
        /// Sets the prompt for the current topic the player discusses with the LLM.
        /// "{0}" will be replaced with a list of the <see cref="RecipeStatement">recipe statements</see>.
        /// </summary>
        /// <param name="prompt"></param>
        public abstract void SetTopicPrompt(string prompt);

        /// <summary>
        /// Creates a chat connection, if it doesn't exist,
        /// and expands on an existing one, if it does.
        /// </summary>
        /// <param name="stmt">The statement the connection belongs to</param>
        /// <param name="request">The message to be sent to the LLM</param>
        /// <returns>An IEnumerator</returns>
        public abstract IEnumerator
            SendMessageToConversation(RecipeStatement stmt, string request);

        public abstract List<RecipeStatement> GetStatements();
    }
}