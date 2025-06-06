using System.Collections;
using System.Collections.Generic;
using FSM;
using Unity.VisualScripting;
using UnityEngine;
using IState = FSM.IState;

public class MorgueEnemy_Attacks : MonoBaseState
{
    [SerializeField] private MorgueEnemy owner;
    private bool _ray;
    private bool _headGrabbed;
    public List<Transform> _path;
    public Node startNode, goal;
    public float timeToLostPlayer;
    private float _actualTime;
    public float hipnosisTime;
    private float _actualGrabCD;
    public float grabCD;
    public int[] enemyAction = { 0, 1, 2 }; //0 - CurseRoom / 1 - Stun / 2 - Blind 
    private int _actualAction;
    private bool animationStarted;
    private float waitingTime;
    private bool _corroutine;
    [SerializeField] private float curseDuration;
    [SerializeField] private float stunDuration;
    public GameObject vomitBall;
    public Transform vomitBallStart;

    private IEnumerator nextAction = null;
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        print("Entre a Attacks");
        ChooseAttack();
    }

    public override void UpdateLoop()
    {
        if (owner.attackEnded) return;
        var playerPos = PlayerHandler.Instance.transform.position;
        playerPos.y = owner.transform.position.y;
        var dir = playerPos - transform.position;

        transform.LookAt(Vector3.SmoothDamp(transform.position,playerPos, ref owner.reference, owner.rotationSmoothTime), Vector3.up);

        if (owner.actualTimeToLost <= 0)
        {
            owner.attackEnded = true;
            return;
        }
        
        _ray = Physics.Raycast(transform.position, dir, dir.magnitude, owner.obstacles);
        
        if (_ray) return;
        
        owner.actualTimeToLost = timeToLostPlayer;
    }
    private void ChooseAttack()
    {
        //_actualAction = Random.Range(0, enemyAction.Length + 1);
        _actualAction = 2;
        print(enemyAction.Length + 1);
        print("Ataque Elegido: " + _actualAction);
        switch (_actualAction)
        {
            case 0:
                AttackCurseRoom();
                break;
            case 1:
                AttackSwarmStun();
                break;
            case 2:
                AttackVomit();
                break;
        }
    }
    
    private void Teleport()
    {
        if (_corroutine) return;
        if (waitingTime > 0)
        {
            waitingTime -= Time.deltaTime;
            return;
        }
        var playerPos = PlayerHandler.Instance.transform.position;
        playerPos.y = owner.transform.position.y;
        if (goal == null)
        {
            startNode = PathFindingManager.instance.CalculateDistance(owner.transform.position);
            goal = PathFindingManager.instance.CalculateFarthestNodeAndRoom(playerPos);
        }

        if (startNode == null)
        {
            owner.attackEnded = true;
            return;
        }

        if (startNode.room.roomBlocked)
        {
            owner.attackEnded = true;
            return;
        }

        StartCoroutine(TeleportCor());
    }
    
    IEnumerator TeleportCor()
    {
        _corroutine = true;
        //TriggerDissapear();
        while (owner.enemyVisibility > 0)
        {
            owner.enemyVisibility -= .5f;
            owner.enemyMaterial.SetFloat("_Power", owner.enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }

        if (goal != null)
        {
            //TriggerAppear();
            Vector3 target = goal.transform.position;
            target.y = owner.transform.position.y;
            owner.transform.position = target;
        }

        while (owner.enemyVisibility < 8)
        {
            owner.enemyVisibility += .5f;
            owner.enemyMaterial.SetFloat("_Power", owner.enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }

        waitingTime = 4f;
        goal = null;
        _corroutine = false;
        StartCoroutine(nextAction);
    }

    #region CurseRoom
    void AttackCurseRoom()
    {
        nextAction = CurseRoom();
        Teleport();
    }

    IEnumerator CurseRoom()
    {
        if (owner.actualRoom.swarmActivate || owner.crossUsed)
        {
            owner.attackEnded = true;
            StopCoroutine(CurseRoom());
        }
        owner.actualRoom.ActivateSwarm(curseDuration);
        owner.attackEnded = true;
        yield return null;
    }
    #endregion
    
    #region SwarmStun

    private void AttackSwarmStun()
    {
        var playerPos = PlayerHandler.Instance.transform.position;
        playerPos.y = owner.transform.position.y;
        var dir = playerPos - transform.position;
        _ray = Physics.Raycast(transform.position, dir, dir.magnitude, owner.obstacles);

        Teleport();
        nextAction = AttackSwarmStunCor();
        return;
        if (_ray)
        {
            Teleport();
            nextAction = AttackSwarmStunCor();
            return;
        }
        StartCoroutine(AttackSwarmStunCor());
    }

    IEnumerator AttackSwarmStunCor()
    {
        float actualTime = 0;
        PlayerHandler.Instance.particleStun.SetActive(true);
 
        while (actualTime < stunDuration)
        {
            actualTime += Time.deltaTime;
            PlayerHandler.Instance.UnPossesPlayer();
            var playerPos = PlayerHandler.Instance.transform.position;
            playerPos.y = owner.transform.position.y;
            var dir = playerPos - transform.position;
            _ray = Physics.Raycast(transform.position, dir, dir.magnitude, owner.obstacles);

            if (owner.crossUsed || _ray)
            {
                PlayerHandler.Instance.PossesPlayer();
                owner.attackEnded = true;
                break;
            }
            yield return null;
        }

        PlayerHandler.Instance.particleStun.SetActive(false);
        PlayerHandler.Instance.PossesPlayer();
        owner.attackEnded = true;
    }
    #endregion

    #region VomitBall

    private void AttackVomit()
    {
        var playerPos = PlayerHandler.Instance.transform.position;
        playerPos.y = owner.transform.position.y;
        var dir = playerPos - transform.position;
        _ray = Physics.Raycast(transform.position, dir, dir.magnitude, owner.obstacles);

        Teleport();
        nextAction = AttackVomitCor();
    }

    IEnumerator AttackVomitCor()
    {
        var ball = Instantiate(vomitBall, vomitBallStart.position, vomitBallStart.rotation);
        ball.transform.parent = vomitBallStart;
        float time = 0;
        Vector3 original = ball.transform.localScale;
        Vector3 finalScale = Vector3.one;
        ball.GetComponent<Rigidbody>().isKinematic = true;
        while (time < 1)
        {
            time += Time.deltaTime * 0.5f;
            ball.transform.localRotation = vomitBallStart.localRotation;
            ball.transform.localScale = Vector3.Lerp(original, finalScale, time);
            yield return null;
        }
        //ball.GetComponent<VomitBall>().ThrowBall();
        ball.GetComponent<Rigidbody>().isKinematic = false;
        ball.transform.forward = vomitBallStart.forward;
        ball.transform.parent = null;
        ball.GetComponent<VomitBall>().StartTrajectory();
        yield return new WaitForSeconds(.5f);
        owner.attackEnded = true;
    }

    #endregion
    public override Dictionary<string, object> Exit(IState to)
    {
        startNode = null;
        goal = null;
        _corroutine = false;
        nextAction = null;
        if (owner.crossUsed)
        {
            PlayerHandler.Instance.PossesPlayer();
            PlayerHandler.Instance.particleStun.SetActive(false);
        }
        StopAllCoroutines();
        print("Sali de attack");
        waitingTime = 0;

        owner.attackEnded = false;
        
        return base.Exit(to);
    }

    public override IState ProcessInput()
    {
        if (owner.ritualDone && Transitions.ContainsKey(StateTransitions.ToRitual))
            return Transitions[StateTransitions.ToRitual];

        if (owner.voodooActivate && Transitions.ContainsKey(StateTransitions.ToVoodoo))
            return Transitions[StateTransitions.ToVoodoo];
        
        if (owner.crossUsed && Transitions.ContainsKey(StateTransitions.ToPatrol))
        {
            return Transitions[StateTransitions.ToPatrol];
        }
        
        if (owner.attackEnded && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];
        
        if (!owner.enemyVisible && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];
        
        if (owner.bibleBurning && Transitions.ContainsKey(StateTransitions.ToSpecifyLocation))
            return Transitions[StateTransitions.ToSpecifyLocation];
        
        return this;
    }
}
