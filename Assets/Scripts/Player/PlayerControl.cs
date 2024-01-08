using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerControl : MonoBehaviour
{

    private CharacterController _controller;
    private Animator _animator;

    [SerializeField]
    [Tooltip("The player movement speed (in m/s) when walking")]
    private float _walkingSpeed;
    [SerializeField]
    [Tooltip("The player movement speed (in m/s) when running")]
    private float _runningSpeed;
    private bool _isRunning;

    public bool IsRunning
    {
        get => _isRunning;
    }

    private Vector3 _smoothDampVelocity = Vector3.zero;

    private Vector2 _direction;

    [SerializeField]
    [Tooltip("The horizontal distance made with a Roll")]
    private float _rollDistance;

    private bool _isRolling;

    public bool IsRolling
    {
        get => _isRolling;
    }

    public UnityEvent OnRollStart;
    public UnityEvent OnRollEnd;

    [SerializeField]
    [Tooltip("The vertical speed (in m/s²) obtained when jumping")]
    private float _jumpPower;
    [SerializeField]
    [Tooltip("The player's mass")]
    private float _mass;
    private float _verticalVelocity;
    private bool _wasGrounded;

    public UnityEvent OnFallStart;
    public UnityEvent OnFallEnd;
    
    private float _horizontalRotation;
    [SerializeField]
    [RangeAttribute(0,90)]
    [Tooltip("The maximum angle (in °) between forward direction and the maximum direction you can look up")]
    private float _maxUpAngle;
    [SerializeField]
    [RangeAttribute(0, 90)]
    [Tooltip("The maximum angle (in °) between forward direction and the maximum direction you can look down")]
    private float _maxDownAngle;
    private float _verticalRotation;
    [SerializeField]
    [Tooltip("The gameobject used to set the direction the character should aim at")]
    private Transform _aiming_transform;

    [SerializeField]
    [Tooltip(("The ratio of the rotation angle over mouse delta"))]
    private float _mouseSensitivity;

    #region UnityLifeCycle
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        if (!_isRolling)
        {
            transform.RotateAround(transform.position,transform.up, _horizontalRotation);

            float angle = 0;
            if (_verticalRotation > 0)
            {
                angle = Mathf.Clamp(_verticalRotation, 0, _maxUpAngle - Vector3.SignedAngle(transform.forward, _aiming_transform.forward,transform.right));
            }
            else
            {
                angle = Mathf.Clamp(_verticalRotation, -_maxDownAngle - Vector3.SignedAngle(transform.forward, _aiming_transform.forward, transform.right), 0);
            }
            _aiming_transform.RotateAround(_aiming_transform.position,_aiming_transform.right, angle);

            float aimedSpeed = _isRunning && _direction.y > 0 ? _runningSpeed : _walkingSpeed;
            Vector3 aimedVelocity = (_direction.y * transform.forward + _direction.x * transform.right) * aimedSpeed;
            Vector3 velocity = Vector3.SmoothDamp(new   Vector3(_controller.velocity.x,0,_controller.velocity.z), aimedVelocity, ref _smoothDampVelocity, 0.3f);
            velocity += new Vector3(0, _verticalVelocity, 0);

            _controller.Move(velocity*Time.fixedDeltaTime);

            bool isGrounded = _controller.isGrounded;
            if (!isGrounded)
                _verticalVelocity -= 9.8f * _mass * Time.fixedDeltaTime;

            _animator.SetBool("isGrounded", isGrounded);
            _animator.SetFloat("ForwardSpeed", Vector3.Dot(velocity,transform.forward));
            _animator.SetFloat("SideSpeed", Vector3.Dot(velocity,transform.right));
        }
        if (_controller.isGrounded)
        {
            if (!_wasGrounded)
                OnFallEnd.Invoke();
        }
        else
        {
            if (_wasGrounded)
            {
                OnFallStart.Invoke();
            }
        }
        _wasGrounded = _controller.isGrounded;
    }
    #endregion

    #region InputActionDetection
    public void MoveAction(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0) return;
        _direction = context.action.ReadValue<Vector2>().normalized;
        if (_direction.y <= 0 && _runningWasSingleTap)
        {
            _runningWasSingleTap = false;
            _isRunning = false;
        }
    }
    
    public void RotateAction(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0) return;
        Vector2 mouseDelta = context.action.ReadValue<Vector2>();

        _verticalRotation = - _mouseSensitivity * mouseDelta.y;
        _horizontalRotation = _mouseSensitivity * mouseDelta.x;

    }

    public void RollAction(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0) return;
        if (context.started && _isRolling == false)
        {
            _isRolling = true;
            StartCoroutine(RollRoutine());
            _isRolling = false;
        }
    }

    private float _lastTimeSinceRunningWasPressed = 0;
    private bool _runningWasSingleTap = false;
    private float _singleTapTimeLimit = 0.4f;
    public void RunningAction(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0) return;
        if (context.canceled)
        {
            if (Time.time - _lastTimeSinceRunningWasPressed < _singleTapTimeLimit)
            {
                _runningWasSingleTap = !_runningWasSingleTap;
            }
            else
            {
                _runningWasSingleTap = false;
            }
            _isRunning = _runningWasSingleTap;
        }
        else if (context.started)
        {
            _isRunning = true;
            _lastTimeSinceRunningWasPressed = Time.time;
        }
    }

    public void JumpAction(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0) return;
        if (context.started && _controller.isGrounded)
        {
            _verticalVelocity = _jumpPower;
        }
    }
    #endregion

    private IEnumerator RollRoutine()
    {
        _animator.SetTrigger("doRoll");
        OnRollStart.Invoke();
        if (_direction.magnitude != 0)
            transform.rotation = Quaternion.LookRotation((_direction.y * transform.forward + _direction.x * transform.right));
        float duration = _animator.GetCurrentAnimatorStateInfo(0).length;
        float speed = _rollDistance / duration;
        float t = 0f;
        while(t < duration)
        {
            t += Time.fixedDeltaTime;
            Vector3 movement = transform.forward * speed * Time.fixedDeltaTime;
            movement += new Vector3(0, _verticalVelocity*Time.fixedDeltaTime, 0);
            _controller.Move(movement);
            if (!_controller.isGrounded)
                _verticalVelocity -= 9.8f * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        OnRollEnd.Invoke();

    }
}
