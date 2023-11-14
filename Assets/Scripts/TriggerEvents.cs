using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : GameBehaviour
{
    public UnityEvent triggerEnterEvent;
    public UnityEvent triggerStayEvent;
    public UnityEvent triggerExitEvent;
    public string eventTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(eventTrigger))
            triggerEnterEvent?.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(eventTrigger))
            triggerStayEvent?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(eventTrigger))
            triggerExitEvent?.Invoke();
    }
}
