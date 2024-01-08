using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    HEAL,
    MAX_HEALTH,
    DIRECT_DAMAGE,
    SPLASH_DAMAGE,
    EXPLOSION_RADIUS
}

public class Buff : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The time it takes for this buff to instantiate")]
    private float _spawnDuration;
    [SerializeField]
    [Tooltip("The speed at which the buffs rotate on itself")]
    private float _angularSpeed;
    [SerializeField]
    [Tooltip("The duration (in s) of 1 animation loop")]
    private float _animationLoopDuration;
    private float _animationTime;
    [SerializeField]
    [Tooltip("Vertical offset during animation")]
    private AnimationCurve _verticalOffset;
    private Vector3 _initialPosition;

    [SerializeField]
    [Tooltip("What type of buff it is")]
    private BuffType _type;

    public BuffType type
    {
        get => _type;
    }

    [SerializeField]
    [Tooltip("The amount of buff applied")]
    private float _amount;

    public float amount
    {
        get => _amount;
    }

    private void Start()
    {
        _initialPosition = transform.position;
        StartCoroutine(SpawnRoutine());
    }

    private void Update()
    {
        transform.Rotate(Vector3.up,_angularSpeed * Time.deltaTime,Space.World);

        _animationTime += Time.deltaTime;
        if (_animationTime > _animationLoopDuration) _animationTime -= _animationLoopDuration;
        transform.position = _initialPosition + _verticalOffset.Evaluate(_animationTime / _animationLoopDuration)*Vector3.up;
    }

    private IEnumerator SpawnRoutine()
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.enabled = false;
        Vector3 scale = transform.localScale;
        transform.localScale = Vector3.zero;
        float time = 0;
        while (time < _spawnDuration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, scale, time);
            yield return new WaitForEndOfFrame();
        }
        collider.enabled = true;
    }
}
