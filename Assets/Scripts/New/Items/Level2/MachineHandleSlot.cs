using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineHandleSlot : MonoBehaviour, IInteractable, IInteractObject
{
    [SerializeField] private Transform pivot, finalPos;
    HandleMachine _handleMachine;
    public void PlaceHandle(HandleMachine handleMachine)
    {
        handleMachine.transform.parent = pivot;
        handleMachine.transform.localPosition = Vector3.zero;
        handleMachine.transform.localRotation = Quaternion.identity;
        handleMachine.GetComponent<BoxCollider>().enabled = false;
        handleMachine.GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(WaitCor(handleMachine));
    }

    IEnumerator WaitCor(HandleMachine handleMachine)
    {
        yield return new WaitForSecondsRealtime(1f);
        _handleMachine = handleMachine;
    }

    public void OnInteractItem()
    {
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
    {
        if (_handleMachine == null) return;
        GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(RotHandler());
    }

    IEnumerator RotHandler()
    {
        var initial = pivot.localRotation;
        var initialX = pivot.localRotation.x;
        var finalX = finalPos.localRotation.x;
        float time = 0;

        while (time < 1)
        {
            initial.x = Mathf.Lerp(initialX, finalX, time);
            pivot.localRotation = initial;
            time += Time.deltaTime;
            yield return null;
        }
        print("Listo");
    }

    public string ShowText()
    {
        return "";
    }

    public bool CanShowText()
    {
        return false;
    }

    public void OnInteractWithThisObject()
    {
        OnInteractWithObject();
    }
}
