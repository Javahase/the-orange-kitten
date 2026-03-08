
/// <summary>
/// Logs game events in real-time.
/// </summary>
public abstract class DataLog : SingletonObject<DataLog>
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public abstract void Log(DataEvent ev, string payload);


    /// <summary>
    /// Interesting events that happened in game.
    /// </summary>
    public enum DataEvent
    {
        /// <summary>
        /// Event that monitors when a new puzzle sequence started.
        /// </summary>
        PuzzleStart,
        
        /// <summary>
        /// Event that monitor when the player or the LLM sent a chat message.
        /// </summary>
        ChatMessage,
        
        /// <summary>
        /// Event that monitors when the count of questions increased.
        /// </summary>
        QuestionCountIncrease,
        
        /// <summary>
        /// Event that monitors when the player encountered a loss-sequence.
        /// </summary>
        AttemptCountIncrease,
        
        /// <summary>
        /// Event that monitors when an <see cref="RecipeInsight">Insight</see> is generated.
        /// </summary>
        InsightGenerated,
        
        /// <summary>
        /// Event that monitors when the player successfully pointed out a contradiction.
        /// </summary>
        Contradict,
        
        /// <summary>
        /// Event that monitors when the player marked a <see cref="RecipeStatement"/> as odd.
        /// </summary>
        StatementMarked,
        
        /// <summary>
        /// Event that monitors when the player unmarked a <see cref="RecipeStatement"/> from being odd.
        /// </summary>
        StatementUnmarked
    }
}