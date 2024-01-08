using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private Slider _slider;
    private RectTransform _rectTransform;
        
    [SerializeField]
    [Tooltip("The life system this bar is tracking")]
    private LifeSystem _lifeSystem;

    private Vector3 _scale;

    [SerializeField]
    [Tooltip("How long does the bar stay displayed when value is changed")]
    private float _displayDuration;
    private float _displayChrono;

    [SerializeField]
    [Tooltip("Should this bar always show")]
    private bool _isAlwaysVisible;

    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _scale = _rectTransform.localScale;
        if (!_isAlwaysVisible)
            _rectTransform.localScale = Vector3.zero;

        _lifeSystem.OnGetHit.AddListener(UpdateValue);
        _lifeSystem.OnHeal.AddListener(UpdateValue);
    }

    public void UpdateValue()
    {
        float t = Mathf.Max(((float)_lifeSystem.health) / _lifeSystem.maxHealth);
        _slider.value = t;
    }

    public void Display()
    {
        StartCoroutine(DisplayRoutine());
    }

    public IEnumerator DisplayRoutine()
    {
        _rectTransform.localScale = _scale;
        _displayChrono = Time.time;
        yield return new WaitForSeconds(_displayDuration);
        if (Time.time - _displayChrono > _displayDuration)
            _rectTransform.localScale = Vector3.zero;
    }

}
