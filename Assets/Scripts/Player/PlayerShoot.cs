using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : Shooter
{

    private Animator _animator;
    private PlayerControl _playerControl;
    private CharacterController _controller;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerControl = GetComponent<PlayerControl>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
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
