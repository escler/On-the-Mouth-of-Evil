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
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= UnlockPlayer;
    }

    private void UnlockPlayer(Scene scene, LoadSceneMode loadSceneMode)
    {
        PossesPlayer();
        movement.inSpot = false;
        movement.ritualCinematic = false;
    }

    public void PossesPlayer()
    {
        movement.enabled = true;
        playerCam.enabled = true;
        bobbingCamera.enabled = true;
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
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 16) return;

        actualRoom = other.GetComponent<Room>();
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
    
    
    
}
