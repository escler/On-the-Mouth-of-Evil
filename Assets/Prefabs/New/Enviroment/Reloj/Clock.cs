using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Clock : MonoBehaviour
{
    private const float
        hoursToDegrees = -360f / 12f,
        minutesToDegrees = -360 / 60f,
        secondsToDegrees = -360 / 60f;

    public Transform hours, minutes, seconds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DateTime time = DateTime.Now;
        hours.localRotation = Quaternion.Euler(0, 0, time.Hour * -hoursToDegrees);
        minutes.localRotation = Quaternion.Euler(0, 0, time.Minute * -minutesToDegrees);
        seconds.localRotation = Quaternion.Euler(0, 0, time.Second * -secondsToDegrees);
    }
}
