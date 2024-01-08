using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{

    private float _verticalVelocity;
    [SerializeField]
    [Tooltip("The agent mass")]
    private float _mass;

    [SerializeField]
    [Tooltip("The walking speed of this entity")]
    private float _walkingSpeed;
    [SerializeField]
    [Tooltip("The running speed of this entity")]
    private float _runningSpeed;

    public bool isGrounded
    {
        get => Physics.Raycast(transform.position, Vector3.down,1, LayerMask.GetMask("Terrain"));
    }

    private bool _isRunning;
    public bool isRunning
    {
        get => _isRunning;
    }

    private NavMeshAgent _agent;
    private Animator _animator;
    private EnemyShoot _shooter;
    private Transform _target;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _shooter = GetComponent<EnemyShoot>();
    }

    private void Start()
    {
        _target = _shooter.target;
    }

    private void FixedUpdate()
    {
        if (_agent.isOnNavMesh && _target)
        {
            _agent.SetDestination(_target.position);

            Vector3 direction = _target.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
            float distance = Vector3.Distance(transform.position, _target.position);
            if (distance > _shooter.maxRange || !HasTargetOnSite())
            {
                _agent.speed = _runningSpeed;
                _isRunning = true;
            }
            else
            {
                _agent.speed = _walkingSpeed;
                _isRunning = false;
            }
        }
        if (!isGrounded)
        {
            _verticalVelocity -= 9.8f * _mass * Time.fixedDeltaTime;
            _agent.Move(_verticalVelocity * Vector3.up * Time.fixedDeltaTime);
        }
        
        _animator.SetBool("isGrounded", isGrounded);
        _animator.SetFloat("ForwardSpeed", Vector3.Dot(_agent.velocity,transform.forward));
        _animator.SetFloat("SideSpeed", Vector3.Dot(_agent.velocity, transform.right));

    }

    public bool HasTargetOnSite()
    {
        Vector3 direction = _target.position - transform.position;
        float distance = direction.magnitude;
        bool hit = Physics.Raycast(transform.position,direction,out RaycastHit hitInfo,distance,LayerMask.GetMask("Terrain"));
        return !hit || hitInfo.distance > distance ;
    }
}
