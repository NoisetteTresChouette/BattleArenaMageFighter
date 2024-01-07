using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : Shooter
{

    private Animator _animator;
    private PlayerControl _playerControl;
    private CharacterController _controller;

    [SerializeField]
    [Tooltip("The target at which bullets are shot")]
    private Transform _target;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerControl = GetComponent<PlayerControl>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 position = transform.position + _cannonPosition.x * transform.right + _cannonPosition.y * transform.up + _cannonPosition.z * transform.forward;
        _cannonDirection = _target.position - _cannonPosition + transform.position;
        if (_isShooting)
        {
            float epsilon = 0.01f;
            if (!_controller.isGrounded || (_controller.velocity.magnitude - 5) > epsilon || _playerControl.IsRolling)
                _animator.SetLayerWeight(1, 0);
            else
                _animator.SetLayerWeight(1, 1);
        }
    }

    public void ShootAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _animator.SetBool("isShooting", true);
            _isShooting = true;
        }
        if (context.canceled)
        {
            _animator.SetBool("isShooting", false);
            _animator.SetLayerWeight(1, 0);
            _isShooting = false;
        }
    }

    public void HitAction()
    {
        Shoot();
    }
}
