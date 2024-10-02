using System;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public void LoadAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
    {
        StartCoroutine(LoadCoroutine(path, callback));
    }

    private System.Collections.IEnumerator LoadCoroutine<T>(string path, Action<T> callback) where T : UnityEngine.Object
    {
        ResourceRequest request = Resources.LoadAsync<T>(path);
        yield return request;

        if (request.asset != null)
        {
            callback((T)request.asset);
        }
        else
        {
            Debug.LogError($"Error loading resource at path: {path}");
        }
    }
}
