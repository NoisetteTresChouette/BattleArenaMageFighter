using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeSystem : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The maximum health amount")]
    public int maxHealth;
    protected int _health;

    public int health
    {
        get => _health;
    }

    protected bool _isDead;

    public UnityEvent OnGetHit;
    public UnityEvent OnDeath;
    public UnityEvent OnHeal;

    private void Start()
    {
        _health = maxHealth;
    }

    public void GetDamaged(int damageAmount)
    {
        if (!enabled) return;
        _health -= damageAmount;
        OnGetHit.Invoke();

        if (_health <= 0)
        {
            OnDeath.Invoke();
            _isDead = true;
            tag = "Dead";
        }
    }

    public int Heal(int healAmount)
    {
        int actualHeal = Mathf.Min(healAmount, maxHealth - _health);
        _health += actualHeal;
        OnHeal.Invoke();
        return actualHeal;
    }
}
