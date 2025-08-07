using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfectionHandler : MonoBehaviour
{
    public static InfectionHandler Instance { get; private set; }
    private Room _actualRoom;
    private float _actualInfection, _dangerMargen;
    [SerializeField] private float infectionTickRate, maxInfection, substractInfectionTickRate;
    private bool _coroutineActive;
    private bool _dead;
    public AudioSource dead;
    public delegate void UpdateInfection();

    public event UpdateInfection OnUpdateInfection;

    public float MaxInfection => maxInfection;
    public float ActualInfection => _actualInfection;
    public Room ActualRoom
    {
        set => _actualRoom = value;
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        _dangerMargen = maxInfection * 0.75f;
        Instance = this;
        OnUpdateInfection += CheckDanger;
        OnUpdateInfection += CheckLife;
        SceneManager.sceneLoaded += ResetVariables;
    }

    private void CheckLife()
    {
        if (_actualInfection < maxInfection) return;
        if (_dead) return;

        var itemInHand = Inventory.Instance.inventory[Inventory.Instance.countSelected];
        if (itemInHand != null)
        {
            if (itemInHand.itemName == "Rosary")
            {
                _actualInfection = 0;
                OnUpdateInfection?.Invoke();
                itemInHand.GetComponent<Rosary>().RosaryProtect();
                return;
            }
        }
        
        _dead = true;
        dead.Play();
        PlayerLifeHandlerNew.Instance.DamageTaked(1);
        Inventory.Instance.deleteItem = true;
        FadeOutHandler.Instance.FaceOut(1f);
        GameManagerNew.Instance.LoadSceneWithDelay("Hub", 1.5f);
    }

    private void OnDestroy()
    {
        OnUpdateInfection -= CheckDanger;
        OnUpdateInfection -= CheckLife;
        SceneManager.sceneLoaded -= ResetVariables;
    }

    private void Update()
    {
        CheckInfection();
    }

    void CheckInfection()
    {
        if (_actualRoom == null)
        {
            SubstractInfection();
            return;
        }
        
        if(_actualRoom.swarmActivate) AddInfection();
        else SubstractInfection();
    }

    void AddInfection()
    {
        _actualInfection += infectionTickRate * Time.deltaTime;
        _actualInfection = Mathf.Clamp(_actualInfection, 0, maxInfection);
        OnUpdateInfection?.Invoke();
    }

    private void CheckDanger()
    {
        if (_coroutineActive) return;
        if (!(_actualInfection >= _dangerMargen)) return;
        PlayerHandler.Instance.PlayerOnDanger();
        StartCoroutine(WaitDangerEnd());
    }
    
    IEnumerator WaitDangerEnd()
    {
        _coroutineActive = true;
        yield return new WaitUntil(() => _actualInfection < _dangerMargen);
        PlayerHandler.Instance.PlayerEndDanger();
        _coroutineActive = false;
    }

    void SubstractInfection()
    {
        if (_actualInfection <= 0) return;
        _actualInfection -= substractInfectionTickRate * Time.deltaTime;
        _actualInfection = Mathf.Clamp(_actualInfection, 0, maxInfection);
        OnUpdateInfection?.Invoke();
    }
    
    private void ResetVariables(Scene scene, LoadSceneMode loadSceneMode)
    {
        _dead = false;
        _coroutineActive = false;
        _actualInfection = 0;
        OnUpdateInfection?.Invoke();
    }
    
}
