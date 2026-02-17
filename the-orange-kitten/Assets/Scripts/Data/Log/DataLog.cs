public abstract class DataLog : SingletonObject<DataLog>
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public abstract void Log(DataEvent ev, string payload);


    public enum DataEvent
    {
        PuzzleStart,
        ChatMessage,
        QuestionCountIncrease,
        AttemptCountIncrease,
        InsightGenerated,
        Contradict,
        StatementMarked,
        StatementUnmarked
    }
}