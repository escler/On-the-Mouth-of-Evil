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
    public bool incenseProtect;
    public float incenseTimer;
    private float _actualTimer;
    public Transform incensePivot;
    public bool focusView;

    [SerializeField] private AudioSource heartBeat;

    public delegate void PlayerInDanger();
    public event PlayerInDanger OnPlayerInDanger;

    public delegate void PlayerInDangerEnd();
    public event PlayerInDangerEnd OnPlayerInDangerEnd;


    public void Unfocus()
    {
        Inventory.Instance.cantSwitch = true;
        var selected = Inventory.Instance.selectedItem;
        if (selected != null)
        {
            if (selected.TryGetComponent(out PaperMission1 paper))
            {
                selected.FocusObject();
                return;
            }

            if (selected.TryGetComponent(out PieceTarotCard piece))
            {
                piece.FocusObject();
                return;
            }
        }

        var slotsCubes = CubePuzzle.Instance.Slots;
        foreach (var s in slotsCubes)
        {
            if (s.RotatingPhase)
            {
                s.ExitRotCube();
                print("Me llame");
            }
        }


    }
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
        OnPlayerInDanger += PlayHeartSound;
        OnPlayerInDangerEnd += StopHeartSound;
        SceneManager.sceneLoaded += UnlockPlayer;
        SceneManager.sceneLoaded += DestroyPlayer;
        SceneManager.sceneLoaded += StopHeartSound;
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
        SceneManager.sceneLoaded -= StopHeartSound;
        OnPlayerInDanger -= PlayHeartSound;
        OnPlayerInDangerEnd -= StopHeartSound;
    }

    private void UnlockPlayer(Scene scene, LoadSceneMode loadSceneMode)
    {
        incenseProtect = false;
        _actualTimer = 0;
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

    public void StunPlayer()
    {
        movement.enabled = false;
        bobbingCamera.enabled = false;
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
            Vector3 direction = point.position - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion lookDirection = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, 10f * Time.deltaTime);
            }

            yield return null;
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

    public void Vomit(float splashTime)
    {
        VomitSplashUI.Instance.AddTime(splashTime);
    }

    IEnumerator IncenseActivateCor()
    {
        _actualTimer = 0;
        while (_actualTimer < incenseTimer)
        {
            _actualTimer += Time.deltaTime;
            yield return null;
        }

        incenseProtect = false;
    }

    public void IncenseActivate()
    {
        incenseProtect = true;
        StopCoroutine(IncenseActivateCor());
        StartCoroutine(IncenseActivateCor());
    }

    private void PlayHeartSound()
    {
        if (heartBeat.isPlaying) return;
        heartBeat.Play();
    }

    private void StopHeartSound()
    {
        if (!heartBeat.isPlaying) return;
        heartBeat.Stop();
    }
    
    private void StopHeartSound(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (!heartBeat.isPlaying) return;
        heartBeat.Stop();
    }
    
}
