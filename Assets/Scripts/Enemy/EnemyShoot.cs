using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShoot : Shooter
{

    [SerializeField]
    [Tooltip("The maximum distance at which this entity start shooting at the target")]
    private float _maxRange;
    public float maxRange
    {
        get => _maxRange;
    }
    [SerializeField]
    [Tooltip("The minimum distance at which this entity stop getting closer to the target")]
    private float _minRange;
    public float minRange
    {
        get => _minRange;
    }

    private NavMeshAgent _agent;
    private EnemyControl _controller;
    private Animator _animator;

    public Transform target
    {
        get => _target;
        set => _target = value;
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _controller = GetComponent<EnemyControl>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        float distance;
        if (_target == null)
            distance = 0;
        else
            distance = Vector3.Distance(_target.position, transform.position);

        if (_controller.HasTargetOnSite() && distance < _maxRange)
        {
            _isShooting = true;
            _agent.stoppingDistance = _minRange;
        }
        else
        {
            _isShooting = false;
            _agent.stoppingDistance = 0;
        }
        _animator.SetBool("isShooting", _isShooting);

        int layerWeight = (_isShooting && _controller.isGrounded && !_controller.isRunning) ? 1 : 0;
        _animator.SetLayerWeight(1, layerWeight);
    }

    public void HitAction()
    {
        Shoot();
    }

}
