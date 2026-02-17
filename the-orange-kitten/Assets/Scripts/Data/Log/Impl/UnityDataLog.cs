using UnityEngine;

public class UnityDataLog : DataLog
{
    public override void Log(DataEvent ev, string payload)
    {
        Debug.Log(ev + ": " + payload);
    }
}