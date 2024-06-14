using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TypeManager : MonoBehaviour
{
    private Queue<string> _typeSequence = new Queue<string>();

    public bool keyPressed, sequenceGenerated, success, canType;
    public string lastKeyPressed;
    public int count;
    public Action onResult;
    public float _actualTime;
    public float timeToResolve;

    private string[] _avaiblesKeys = 
        { "q", "w", "e", "r", "a", "s", "d", "f" };
    public static TypeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!canType) return;
            SetLastKey(Input.inputString);
        }
        
        if (sequenceGenerated)
        {
            _actualTime -= Time.deltaTime;
            if(_actualTime <= 0) SetLastKey("error");
        }
    }
    private void SetLastKey(string key)
    {
        lastKeyPressed = key;
        keyPressed = true;
    }

    public void GenerateNewSequence(int lenght)
    {
        _typeSequence.Clear();
        count = 0;
        for (int i = 0; i < lenght; i++)
        {
            var randomKey = Random.Range(0, _avaiblesKeys.Length);
            _typeSequence.Enqueue(_avaiblesKeys[randomKey]);
        }
        sequenceGenerated = true;
        DebugQueue();
    }


    private void DebugQueue()
    {
        var cloneQueue = _typeSequence.ToArray();
        if (cloneQueue.Length <= 0) return;
        var panel = KeyUIGenerator.Instance;
        panel.ShowPanel();
        for (int i = 0; i < cloneQueue.Length; i++)
        {
            panel.AddKey(cloneQueue[i]);
        }
        StartCoroutine(ResolveSequence());
        _actualTime = timeToResolve;
        StartCoroutine(WaitForType());
    }

    public bool ResultOfType()
    {
        Player.Instance.PossesControls();
        KeyUIGenerator.Instance.DeleteKeys();
        canType = false;
        return success;
    }

    IEnumerator ResolveSequence()
    {
        var lenghtOfQueue = _typeSequence.Count;
        if (lenghtOfQueue <= 0) yield break;
        for (int i = 0; i < lenghtOfQueue; i++)
        {
            var key = _typeSequence.Dequeue();
            keyPressed = false;
            KeyUIGenerator.Instance.ChangeColor(count);
            yield return new WaitUntil(() => keyPressed);
            if (lastKeyPressed != key) break;
            count++;
        }

        if (count == lenghtOfQueue) success = true;
        else success = false;
        sequenceGenerated = false;
        onResult?.Invoke();
    }

    IEnumerator WaitForType()
    {
        yield return new WaitForNextFrameUnit();
        canType = true;
    }
}
