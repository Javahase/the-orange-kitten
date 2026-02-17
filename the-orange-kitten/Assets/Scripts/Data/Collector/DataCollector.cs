using System;

public abstract class DataCollector: SingletonObject<DataCollector>
{

    protected string ParticipantId;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init("P01"); //TODO: No.
    }
    
    public abstract void Init(string participantId);

    public abstract void StartLevel(PuzzleData data);

    public abstract void Save();

}