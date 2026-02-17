using System;
using UI.RecipeSequence;
using UnityEngine;
using Util.Object;

public class ConnectionInfo : MonoBehaviour
{

    [SerializeField] private RecipeSequence coreSequence;
    private SwitchBehavior _connectionDisplay;

    private void Start()
    {
        _connectionDisplay = transform.Find("ConnectionState").GetComponent<SwitchBehavior>();
        coreSequence.healthContainer.NoHealthEvent += OnLost;
    }

    public void OnLost() => _connectionDisplay.Select(1);

    public void ResetDisplay() => _connectionDisplay.Select(0);
}