using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHS : MonoBehaviour
{
    [SerializeField] private Transform finalSpot;

    public void MoveSpot()
    {
        StartCoroutine(MoveCor());
    }

    IEnumerator MoveCor()
    {
        var initial = transform.position;
        float time = 0;

        while (time < 1)
        {
            transform.position = Vector3.Lerp(initial, finalSpot.position, time);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
