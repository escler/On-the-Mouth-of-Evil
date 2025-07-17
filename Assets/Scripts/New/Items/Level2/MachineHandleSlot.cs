using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineHandleSlot : MonoBehaviour, IInteractable, IInteractObject
{
    [SerializeField] private Transform pivot, finalPos;
    HandleMachine _handleMachine;
    [SerializeField] private GameObject goodAura;
    [SerializeField] private GameObject elecPs;
    [SerializeField] private AudioSource placeLever, moveLever, machineLoop;
    [SerializeField] private float speedRot;
    public void PlaceHandle(HandleMachine handleMachine)
    {
        placeLever.Play();
        handleMachine.transform.parent = transform;
        handleMachine.transform.localPosition = Vector3.zero;
        handleMachine.transform.localRotation = Quaternion.identity;
        handleMachine.GetComponent<BoxCollider>().enabled = false;
        handleMachine.GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(WaitCor(handleMachine));
    }

    IEnumerator WaitCor(HandleMachine handleMachine)
    {
        yield return new WaitForSecondsRealtime(.2f);
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
        StartCoroutine(ShowDialog());
        if (_handleMachine == null) return;
        GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(RotHandler());
    }

    IEnumerator ShowDialog()
    {
        yield return new WaitForSeconds(0.2f);
        
        if (_handleMachine == null)
        {
            DialogHandler.Instance.ChangeText("A wheel should fit here perfectly. Now where is it?");
            
        }
    }

    IEnumerator RotHandler()
    {
        goodAura.SetActive(true);
        moveLever.Play();
        GetComponent<BoxCollider>().enabled = false;

        float time = 0;
        bool electActive = false;
        
        while (time < moveLever.clip.length)
        {
            if (time > moveLever.clip.length / 2 && !electActive)
            {
                DialogHandler.Instance.ChangeText("There we go… the ovens are back online.");
                elecPs.SetActive(true);
                electActive = true;
            }
            time += Time.deltaTime;
            transform.Rotate(0,0, speedRot * Time.deltaTime);
            yield return null;
        }
        
        machineLoop.Play();
        GoodRitual.Instance.leverActivated = true;
        GoodRitual.Instance.StartFireOven();
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
