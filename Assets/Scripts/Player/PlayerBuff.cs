using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff : MonoBehaviour
{

    private PlayerLife _life;
    private PlayerShoot _shooter;
    [SerializeField]
    [Tooltip("The gameobject managing the stats ui")]
    private StatUI _statsUI;

    private void Awake()
    {
        _life = GetComponent<PlayerLife>();
        _shooter = GetComponent<PlayerShoot>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Buff" && other.TryGetComponent<Buff>(out Buff buff))
        {
            string text = "";
            switch (buff.type) {
                case BuffType.HEAL:
                    _life.Heal(Mathf.FloorToInt(buff.amount));
                     text = $"{_life.health} \n/ {_life.maxHealth}";
                    break;
                case BuffType.MAX_HEALTH:
                    _life.maxHealth += Mathf.FloorToInt(buff.amount);
                    _life.OnHeal.Invoke();
                    text = $"{_life.health} \n/ {_life.maxHealth}";
                    break;
                case BuffType.DIRECT_DAMAGE:
                    _shooter.damage += Mathf.FloorToInt(buff.amount);
                    text = $":  {_shooter.damage}";
                    break;
                case BuffType.SPLASH_DAMAGE:
                    _shooter.splashDamageRatio += buff.amount;
                    text = $": {_shooter.splashDamageRatio}";
                    break;
                case BuffType.EXPLOSION_RADIUS:
                    _shooter.splashDamageRadius += buff.amount;
                    text = $": {_shooter.splashDamageRadius}";
                    break;
            }

            _statsUI.UpdateStats(buff.type.ToString(), text);
            Destroy(other.gameObject);
        }
    }

}
