using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance { get; private set; }
    
    private IEvent _currentEvent;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }
    
    public void SetCurrentEvent(IEvent actualEvent)
    {
        if (_currentEvent != null) return;

        _currentEvent = actualEvent;
        _currentEvent.StartEvent();
    }

    private void CheckEventEnd()
    {
        if (_currentEvent == null || !_currentEvent.CheckEventState()) return;
        
        _currentEvent.EndEvent();
        _currentEvent = null;

    }

    private void Update()
    {
        CheckEventEnd();
    }
}
