using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    [SerializeField]
    [Tooltip("The enemy spawner")]
    private Transform _spawner;
    [SerializeField]
    [Tooltip("The enemy perfab")]
    private GameObject _enemyPrefab;
    [SerializeField]
    [Tooltip("The enemy spawning rate (amount/s)")]
    private float _startSpawnRate;
    private float _currentSpawnRate;
    [SerializeField]
    [Tooltip("How much the spawn rate increases per second")]
    private float _spawnRateIncrement;
    [SerializeField]
    [Tooltip("the maximum spawn rate reachable")]
    private float _maxSpawnRate;
    private float _spawnChrono;

    [SerializeField]
    [Tooltip("The player vital point")]
    private Transform _target;

    [SerializeField]
    private float _enemyStartHP;
    [SerializeField]
    [Tooltip("How much the enemy max hp increases per second")]
    private float _enemyHPIncement;
    private float _currentHP;
    [SerializeField]
    [Tooltip("Beginning direct damage")]
    private float _enemyStartDamage;
    [SerializeField]
    [Tooltip("How much the enemy damage increases per second")]
    private float _enemyDamageIncrement;
    private float _currentDamage;

    private void Start()
    {
        StartCoroutine(FirstSpawn());
    }

    private void Update()
    {
        if (_spawnChrono > 1f / _currentSpawnRate)
        {
            Spawn();
            _spawnChrono = 0f;
        }
        float deltaTime = Time.deltaTime;
        _spawnChrono += deltaTime;
        _currentDamage += _enemyDamageIncrement * deltaTime;
        _currentHP += _enemyHPIncement * deltaTime;
        _currentSpawnRate += _spawnRateIncrement * deltaTime;
    }

    private void Spawn()
    {
        _spawnChrono = 0;
        GameObject enemy = Instantiate(_enemyPrefab, _spawner.position,Quaternion.identity);
        enemy.GetComponent<LifeSystem>().maxHealth = Mathf.FloorToInt(_currentHP);
        enemy.GetComponent<EnemyShoot>().damage = Mathf.FloorToInt(_currentDamage);
        enemy.GetComponent<EnemyShoot>().target = _target;
    }

    private IEnumerator FirstSpawn()
    {
        _spawnChrono = -Mathf.Infinity;
        yield return new WaitForSeconds(5);
        _currentDamage = _enemyStartDamage;
        _currentHP = _enemyStartHP;
        _currentSpawnRate = _startSpawnRate;
        Spawn();
    }
    
}
