using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The bullet shot by this shooter")]
    protected GameObject _bulletPrefab;
    [SerializeField]
    [Tooltip("The bullet velocity")]
    protected float _velocity = 20f;

    [SerializeField]
    [Tooltip("The position of the cannon, acording to the transform position")]
    protected Vector3 _cannonPosition;
    [SerializeField]
    [Tooltip("The direction of the cannon, acording to the transform position")]
    protected Vector3 _cannonDirection;

    protected bool _isShooting;

    protected void Shoot()
    {
        Vector3 position = transform.position + _cannonPosition.x * transform.right + _cannonPosition.y * transform.up + _cannonPosition.z * transform.forward;
        Bullet bullet = Instantiate(_bulletPrefab, position, Quaternion.LookRotation(_cannonDirection)).GetComponent<Bullet>();
        bullet.velocity = _velocity;
    }


}
