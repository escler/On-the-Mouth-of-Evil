using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    public static Oven Instance { get; private set; }
    
    public AudioSource _hornilla;
    [SerializeField] private Knob[] _knobs;
    [SerializeField] private OvenDoor _ovenDoor;
    [SerializeField] private ParticleSystem[] _particles;
    [SerializeField] List<int> numbers = new List<int>();
    [SerializeField] private GameObject key;
    public GameObject bear;
    public string code;
    private bool _bearInOven;

    public bool BearInOven
    {
        set{_bearInOven = value;}
    }
    

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        ResetFiresPs();

    }

    public void CheckCode()
    {
        if (numbers.Count < 4 || _ovenDoor.Open || !_bearInOven)
        {
            WrongAnswer();
            return;
        }

        var actualCode = "";
        foreach (var n in numbers)
        {
            actualCode += n;
        }
        
        if(actualCode != code)WrongAnswer();
        else CorrectAnswer();
    }

    private void CorrectAnswer()
    {
        key.gameObject.SetActive(true);
        bear.gameObject.SetActive(false);
        foreach (var knob in _knobs)
        {
            knob.enabled = false;
        }
        
        _ovenDoor.OpendDoor();
        _ovenDoor.enabled = false;
        print("Good");
    }
    private void WrongAnswer()
    {
        numbers.Clear();
        ResetKnobs();
        ResetFiresPs();
    }

    private void ResetKnobs()
    {
        foreach (var k in _knobs)
        {
            k.ResetKnob();
        }
        
        _ovenDoor.ResetDoor();
    }
    private void ResetFiresPs()
    {
        foreach (var p in _particles)
        {
            var ps = p.emission;
            ps.enabled = false;
        }
    }

    public void AddKnob(Knob knob)
    {
        numbers.Add(knob.number);
    }

    public void RemoveKnob(Knob knob)
    {
        if (!numbers.Contains(knob.number)) return;
        numbers.Remove(knob.number);
    }
    
    public void ChangeFireState(Knob knob)
    {
        for (int i = 0; i < _knobs.Length; i++)
        {
            if(_knobs[i] != knob) continue;

            var ps = _particles[i].emission;
            ps.enabled = !ps.enabled;
            break;
        }
    }
    
}
