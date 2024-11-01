using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionsHandler : MonoBehaviour
{
    public static DecisionsHandler Instance { get; private set; }
    private int _badChoice, _goodChoice;
    public bool badPath;
    
    public int BadChoice
    {
        get => _badChoice;
        set => _badChoice = value;
    }

    public int GoodChoice
    {
        get => _goodChoice;
        set => _goodChoice = value;
    }
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        _goodChoice = 0;
        _badChoice = 0;
        Instance = this;
    }

    public void GoodChoiceTaked()
    {
        _goodChoice++;
        UpdatePath();
    }

    public void BadChoiceTaked()
    {
        _badChoice++;
        UpdatePath();
    }

    void UpdatePath()
    {
        badPath = _badChoice >= _goodChoice;
    }
    
    
}
