using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubBell : MonoBehaviour, IInteractable
{
    public AudioSource bell;
    public float timeForBell;
    private float _actualTime;
    public Transform paperPos, finalPos;
    private bool _interactUsed;
    private GameObject _paperMission;
    
    public GameObject PaperMission => _paperMission; 
   
    private void Awake()
    {
        CheckPaperMission();
        SetMissionPaper();
        _actualTime = timeForBell / 2;
    }

    private void SetMissionPaper()
    {
        if (_paperMission == null) return;
        _paperMission.GetComponent<BoxCollider>().enabled = false;
        _paperMission.GetComponent<Rigidbody>().isKinematic = true;
        _paperMission.transform.position = paperPos.transform.position;
        _paperMission.transform.rotation = paperPos.transform.rotation;
    }

    private void CheckPaperMission()
    {
        var level = PlayerPrefs.GetInt("RespectLevel");
        if (level > 1)
        {
            var mission2Complete = PlayerPrefs.GetInt("Mission2Complete");
            if (mission2Complete == 1)
            {
                DisableObject();
                return;
            }
            _paperMission = Instantiate(ProgressManager.Instance.missions[1]);
            return;
        }
        
        var mission1Complete = PlayerPrefs.GetInt("Mission1Complete");
        if (mission1Complete == 1)
        {
            DisableObject();
            return;
        }
        _paperMission = Instantiate(ProgressManager.Instance.missions[0]);
    }

    private void DisableObject()
    {
        GetComponent<BoxCollider>().enabled = false;
        gameObject.layer = 1;
        enabled = false;
    }

    void Update()
    {
        if (_interactUsed) return;
        _actualTime -= Time.deltaTime;
        if (_actualTime < 0)
        {
             bell.Play();
            _actualTime = timeForBell;
           
        }
    }

    IEnumerator MovePaper()
    {
        float ticks = 0;
        Vector3 originalPos = paperPos.position;
        while (ticks < 1)
        {
            ticks += Time.deltaTime;
            _paperMission.transform.position = Vector3.Lerp(originalPos, finalPos.position, ticks);
            yield return null;
        }

        _paperMission.GetComponent<BoxCollider>().enabled = true;
        _paperMission.GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<BoxCollider>().enabled = false;
        enabled = false;
    }

    public void OnInteractItem()
    {
        if (!_interactUsed)
        {
            StartCoroutine(MovePaper());
            if (TutorialHub.Instance != null) TutorialHub.Instance.confessionaryReached = true;
        }
        _interactUsed = true;
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
    {
    }

    public string ShowText()
    {
        return "";
    }

    public bool CanShowText()
    {
        return true;
    }
}
