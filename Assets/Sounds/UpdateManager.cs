using UnityEngine;
using System;
using System.Collections.Generic;

public class UpdateManager : Singleton<UpdateManager>
{
    private List<Action> updateListeners = new List<Action>();

    private void Update()
    {
        foreach (var listener in updateListeners)
        {
            listener?.Invoke();
        }
    }

    public void AddUpdateListener(Action listener)
    {
        if (!updateListeners.Contains(listener))
        {
            updateListeners.Add(listener);
        }
    }

    public void RemoveUpdateListener(Action listener)
    {
        if (updateListeners.Contains(listener))
        {
            updateListeners.Remove(listener);
        }
    }
}
