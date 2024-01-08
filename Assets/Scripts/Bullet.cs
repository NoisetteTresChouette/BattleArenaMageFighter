using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour
{
    [Tooltip("The speed at which the bullet travels")]
    public float velocity;
    [SerializeField]
    [Tooltip("The damage this bullet deals")]
    private int _damage;
    public int damage
    {
        get => _damage;
    }
    [SerializeField]
    [Tooltip("The radius of explosion")]
    private float _splashDamageRadius;
    public float splashDamageRadius
    {
        get => splashDamageRadius;
    }
    [SerializeField]
    [Tooltip("The ratio of the damage dealt in the radius of explosion over direct hit damage ")]
    private float _splashDamageRatio;
    public float splashDamageRatio
    {
        get => splashDamageRatio;
    }

    public void SetDamage(int directDamage,float splashDamage, float splashRadius)
    {
        _damage = directDamage;
        _splashDamageRatio = splashDamage;
        _splashDamageRadius = splashRadius;
    }

    [SerializeField]
    [Tooltip("The time the bullet still has to live")]
    private float _timeSpan;

    [SerializeField]
    [Tooltip("The explosion spawning when the bullet is hiiting something")]
    private GameObject _explosionPrefab;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rb.velocity = transform.forward * velocity;
    }

    private void Update()
    {
        _timeSpan -= Time.deltaTime;
        if (_timeSpan <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != tag)
        {
            Instantiate(_explosionPrefab, transform.position,Quaternion.identity);


            int splashDamage = 0;
            if (_splashDamageRadius > 0)
            {
                splashDamage = Mathf.FloorToInt(_splashDamageRatio * _damage);
                Collider[] objects = Physics.OverlapSphere(transform.position, _splashDamageRadius);
                foreach (Collider o in objects)
                {
                    if (o.tag != tag && o.TryGetComponent<LifeSystem>(out LifeSystem lf2))
                    {
                        lf2.GetDamaged(splashDamage);
                    }
                }
            }

            if (other.TryGetComponent<LifeSystem>(out LifeSystem lf))
            {
                lf.GetDamaged(_damage);
            }
           
            if (other.tag != "Dead")
                Destroy(gameObject);
        }
    }

}
