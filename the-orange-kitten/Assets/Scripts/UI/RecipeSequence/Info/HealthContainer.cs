using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthContainer : MonoBehaviour
{
    public event OnDamage DamageEvent;
    public event NoHealth NoHealthEvent;

    public delegate void NoHealth();

    public delegate void OnDamage();


    [SerializeField] private Image healthBar;
    [SerializeField] private float maxHealth;
    private float _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    public void ResetHealth() => SetHealth(maxHealth);

    public void SetHealth(float health)
    {
        _currentHealth = Math.Clamp(health, 0, maxHealth);
        healthBar.fillAmount = health / maxHealth;
        if (_currentHealth <= 0)
        {
            NoHealthEvent?.Invoke();
        }
    }

    public void Damage()
    {
        SetHealth(_currentHealth - 1);
        DamageEvent?.Invoke();
    }
}