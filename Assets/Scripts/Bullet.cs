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
    [Tooltip("The time the bullet still has to live")]
    private float _timeSpan;

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
            Debug.Log("BOOM");
            Destroy(gameObject);
        }
    }

}
