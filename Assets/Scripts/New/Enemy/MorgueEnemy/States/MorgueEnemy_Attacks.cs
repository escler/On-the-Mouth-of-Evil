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
    public int[] enemyAction = { 0}; //0 - CurseRoom / 1 - Stun / 2 - Blind 
    private int _actualAction;
    private bool animationStarted;
    private float waitingTime;
    private bool _corroutine;
    [SerializeField] private float curseDuration;
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
        CurseRoom();
        //if (_actualGrabCD <= 0) StartCoroutine(Hipnosis());
        //else 
    }

    private void ChooseAttack()
    {
        switch (_actualAction)
        {
            case 0:
                AttackCurseRoom();
                break;
        }
    }

    void AttackCurseRoom()
    {
        Teleport();
    }

    private void CurseRoom()
    {
        if (owner.actualRoom.swarmActivate || owner.crossUsed)
        {
            owner.attackEnded = true;
            return;
        }
        owner.actualRoom.ActivateSwarm(curseDuration);
        owner.attackEnded = true;
    }
    
    public override Dictionary<string, object> Exit(IState to)
    {
        
        startNode = null;
        goal = null;
        _corroutine = false;
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
