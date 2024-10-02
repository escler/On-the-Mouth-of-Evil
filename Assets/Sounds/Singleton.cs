using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log(typeof(T).Name + " instance is not initialized yet");
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<T>();

            if (instance == null)
            {
                GameObject singletonObject = new GameObject(typeof(T).Name);
                instance = singletonObject.AddComponent<T>();
                DontDestroyOnLoad(singletonObject);
            }
            else
            {
                DontDestroyOnLoad(instance.gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
