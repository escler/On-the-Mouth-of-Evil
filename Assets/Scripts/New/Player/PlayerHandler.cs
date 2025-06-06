using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler Instance { get; private set; }

    public PlayerMovement movement;
    public PlayerCam playerCam;
    public BobbingCamera bobbingCamera;
    public Transform handPivot, cameraPos, puzzlePivot, closeFocusPos,farFocusPos, farthestFocusPos;
    public Mission actualMission;
    public Room actualRoom;
    public bool cantPressInventory;
    public Transform particlePivot;
    public Animator animator;
    public bool cantInteract;
    public bool movingObject;
    public GameObject particleStun;

    public delegate void PlayerInDanger();
    public event PlayerInDanger OnPlayerInDanger;

    public delegate void PlayerInDangerEnd();
    public event PlayerInDangerEnd OnPlayerInDangerEnd;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        Instance = this;
        movement = GetComponent<PlayerMovement>();
        SceneManager.sceneLoaded += UnlockPlayer;
        SceneManager.sceneLoaded += DestroyPlayer;
        cantInteract = false;
        StartCoroutine(WaitForLock());
    }

    private void DestroyPlayer(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name != "Menu") return;
        
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= UnlockPlayer;
        SceneManager.sceneLoaded -= DestroyPlayer;

    }

    private void UnlockPlayer(Scene scene, LoadSceneMode loadSceneMode)
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.None;
        PossesPlayer();
        movement.inSpot = false;
        movement.ritualCinematic = false;
        movement.voodooMovement = false;
        movement.absorbEnd = false;
        movement.inVoodooPos = false;
        playerCam.ticks = 0;
        StartCoroutine(WaitForLock());
    }

    IEnumerator WaitForLock()
    {
        yield return new WaitForSeconds(0.01f);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }

    public void PossesPlayer()
    {
        movement.enabled = true;
        playerCam.enabled = true;
        bobbingCamera.enabled = true;
        cantInteract = false;
        playerCam.CameraLock = false;
    }

    public void ChangePlayerPosses(bool state)
    {
        movement.enabled = state;
        playerCam.enabled = state;
        bobbingCamera.enabled = state;
    }

    public void UnPossesPlayer()
    {
        movement.enabled = false;
        playerCam.enabled = false;
        bobbingCamera.enabled = false;
        cantInteract = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 16) return;

        if (actualRoom == null) return;
        if (other != actualRoom.GetComponent<BoxCollider>()) return;

        actualRoom = null;
        InfectionHandler.Instance.ActualRoom = null;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 16) return;

        actualRoom = other.GetComponent<Room>();
        InfectionHandler.Instance.ActualRoom = actualRoom;
    }

    public void HeadGrabbed(Transform target)
    {
        StartCoroutine(LookEnemy(target));
    }

    IEnumerator LookEnemy(Transform point)
    {
        print("LookEnemy");
        UnPossesPlayer();
        while (HouseEnemy.Instance.grabHead)
        {
            Quaternion lookDirection = Quaternion.LookRotation(point.position - transform.position).normalized;
            lookDirection.x = transform.rotation.x;
            lookDirection.z = transform.rotation.z;

            transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, 10f * Time.deltaTime);
            

            yield return new WaitForSeconds(0.01f);
        }
        
        PossesPlayer();
    }

    public void PlayerOnDanger()
    {
        OnPlayerInDanger?.Invoke();
    }

    public void PlayerEndDanger()
    {
        OnPlayerInDangerEnd?.Invoke();
    }

    public void Vomit()
    {
        
    }
    
}
