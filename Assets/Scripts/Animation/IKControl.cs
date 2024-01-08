using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]

public class IKControl : MonoBehaviour
{

    protected Animator _animator;
    [SerializeField]
    [Tooltip("Is the ik active")]
    private bool _ikActive = false;

    public bool IkActive
    {
        get => _ikActive;
        set => _ikActive = value;
    }

    [SerializeField]
    [Tooltip("The obect to look at")]
    private Transform _lookObj = null;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        if (_animator)
        {

            if (_ikActive)
            {

                if (_lookObj != null)
                {
                    _animator.SetLookAtWeight(1);
                    _animator.SetLookAtPosition(_lookObj.position);
                }
            }

            else
            {
                _animator.SetLookAtWeight(0);
            }
        }
    }
}

