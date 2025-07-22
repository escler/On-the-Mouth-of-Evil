using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideCanvas : MonoBehaviour
{
    [SerializeField] private Canvas canvasToHide;

    void Start()
    {
        ToggleCanvasGraphics(canvasToHide, false); // Ocultar
    }

    void ToggleCanvasGraphics(Canvas canvas, bool visible)
    {
        Graphic[] graphics = canvas.GetComponentsInChildren<Graphic>(true);
        foreach (var g in graphics)
        {
            g.enabled = visible;
        }
    }
}
