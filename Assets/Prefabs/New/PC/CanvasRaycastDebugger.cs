using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CanvasRaycastDebugger : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster raycaster; // Asigná el canvas
    [SerializeField] private EventSystem eventSystem;    // Asigná el EventSystem

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (raycaster == null || eventSystem == null)
            {
                Debug.LogWarning("Falta asignar GraphicRaycaster o EventSystem.");
                return;
            }

            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            if (results.Count == 0)
            {
                Debug.Log("No se encontró ningún elemento de UI en el click.");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"🖱️ Raycast UI hits ({results.Count}) en posición {Input.mousePosition}:");

            foreach (RaycastResult result in results)
            {
                string path = GetFullHierarchyPath(result.gameObject.transform);
                sb.AppendLine($"→ {path} (Layer: {result.gameObject.layer})");
            }

            Debug.Log(sb.ToString());
        }
    }

    string GetFullHierarchyPath(Transform t)
    {
        List<string> pathParts = new List<string>();
        while (t != null)
        {
            pathParts.Insert(0, t.name);
            t = t.parent;
        }
        return string.Join("/", pathParts);
    }
}

