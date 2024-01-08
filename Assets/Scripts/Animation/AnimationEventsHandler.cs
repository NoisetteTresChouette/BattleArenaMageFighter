using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventsHandler : MonoBehaviour
{

    public UnityEvent OnHit;
    public UnityEvent OnEndDeath;

    public void Hit()
    {
        OnHit.Invoke();
    }

    public void EndDeath()
    {
        OnEndDeath.Invoke();
    }
}