using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The damage this bullet deals")]
    public int damage;
    [SerializeField]
    [Tooltip("The radius of explosion")]
    public float splashDamageRadius;
    [SerializeField]
    [Tooltip("The ratio of the damage dealt in the radius of explosion over direct hit damage ")]
    public float splashDamageRatio;

    [SerializeField]
    [Tooltip("The bullet shot by this shooter")]
    protected GameObject _bulletPrefab;
    public Bullet bullet
    {
        get => _bulletPrefab.GetComponent<Bullet>();
    }
    [SerializeField]
    [Tooltip("The bullet velocity")]
    protected float _velocity = 20f;

    [SerializeField]
    [Tooltip("The position of the cannon, acording to the transform position")]
    protected Vector3 _cannonPosition;
    [SerializeField]
    [Tooltip("This entity target")]
    protected Transform _target;
    protected Vector3 _cannonDirection;

    protected bool _isShooting;

    protected void Shoot()
    {
        Vector3 position = transform.position + _cannonPosition.x * transform.right + _cannonPosition.y * transform.up + _cannonPosition.z * transform.forward;
        if (_target)
            _cannonDirection = _target.position - position;
        else
            _cannonDirection = transform.forward;
        Bullet bullet = Instantiate(_bulletPrefab, position, Quaternion.LookRotation(_cannonDirection)).GetComponent<Bullet>();
        bullet.tag = tag;
        bullet.velocity = _velocity;
        bullet.SetDamage(damage, splashDamageRatio, splashDamageRadius);
    }


}
