using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsCount : MonoBehaviour
{
    private float fps;
    private float updateTimer = 0;
    [SerializeField] private TextMeshProUGUI fpsTitle;


    private void Awake()
    {
        fpsTitle = GetComponent<TextMeshProUGUI>();
    }
    
    private void Update()
    {
        UpdateFPSDisplay();
    }

    void UpdateFPSDisplay()
    {
        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0f)
        {
            fps = 1f / Time.unscaledDeltaTime;
            fpsTitle.text = "FPS: " + MathF.Round(fps);
            updateTimer = 0.2f;
        }
    }
}
