using System;
using UnityEngine;
using Util.Object;

public class ConnectionInfo : MonoBehaviour
{
    private SwitchBehavior _connectionDisplay;

    private void Start()
    {
        _connectionDisplay = transform.Find("ConnectionState").GetComponent<SwitchBehavior>();
    }

    public void OnLost() => _connectionDisplay.Select(1);

    public void ResetDisplay() => _connectionDisplay.Select(0);
}