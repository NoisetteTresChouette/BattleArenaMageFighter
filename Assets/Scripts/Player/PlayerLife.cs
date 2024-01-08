using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : LifeSystem
{

    private Animator _animator;

    [SerializeField]
    [Tooltip("The stats UI game object")]
    private StatUI _statsUI;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void DeathEvent()
    {
        _animator.SetLayerWeight(1, 0);
        _animator.SetTrigger("doDie");
    }

    public void UpdateLifeStat()
    {
        _statsUI.UpdateStats("HEAL", $"{Mathf.Max(_health,0)} \n/ {maxHealth}");
    }

}
