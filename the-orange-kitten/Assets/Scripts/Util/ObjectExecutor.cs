using System;
using UnityEngine;
using UnityEngine.Events;

public class ObjectExecutor : MonoBehaviour
{
    [SerializeField] private UnityEvent enableAction;
    [SerializeField] private UnityEvent disableAction;

    private void OnEnable() => enableAction?.Invoke();

    private void OnDisable() => disableAction?.Invoke();
}