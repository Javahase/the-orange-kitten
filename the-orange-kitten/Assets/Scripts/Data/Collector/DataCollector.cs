using System;

public abstract class DataCollector: SingletonObject<DataCollector>
{

    protected string ParticipantId;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public abstract void Init(string participantId);

    public abstract void StartLevel(PuzzleData data);

    public abstract void Save();

}