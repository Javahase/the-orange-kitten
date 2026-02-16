using System;

public abstract class DataCollector: SingletonObject<DataCollector>
{

    private string _participantId;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public abstract void InitLevel(string levelId);
    
}