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
    private VomitBall _actualBall;

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

        if (owner.Player.incenseProtect)
        {
            owner.actualTimeToLost = 0;
            return;
        }
        if (_ray) return;
        
        owner.actualTimeToLost = timeToLostPlayer;
    }

    private void TriggerAppear()
    {
        owner.anim.ChangeTrigger("Appear");
    }
    private void ChooseAttack()
    {
        _actualAction = Random.Range(0, 3);
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
        TriggerAppear();
        while (owner.enemyVisibility > 0)
        {
            owner.enemyVisibility -= 1f;
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
            owner.enemyVisibility += 1f;
            owner.enemyMaterial.SetFloat("_Power", owner.enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }

        waitingTime = 4f;

        yield return new WaitUntil(() => owner.anim.animator.GetCurrentAnimatorStateInfo(0).IsName("Appear"));
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
        owner.anim.ChangeTrigger("CurseRoom");
        yield return new WaitUntil(() => owner.anim.animator.GetCurrentAnimatorStateInfo(0).IsName("Cast"));
        owner.CurseRoomSound();
        if (owner.actualRoom.swarmActivate || owner.crossUsed)
        {
            yield return new WaitUntil(() => !owner.anim.animator.GetCurrentAnimatorStateInfo(0).IsName("Cast"));
            owner.attackEnded = true;
            StopCoroutine(CurseRoom());
        }
        owner.actualRoom.ActivateSwarm(curseDuration);
        yield return new WaitUntil(() => !owner.anim.animator.GetCurrentAnimatorStateInfo(0).IsName("Cast"));
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
        owner.anim.ChangeState("SwarmAttack", true);
        yield return new WaitUntil(() => owner.anim.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));
        owner.StunSound();
        float actualTime = 0;
        PlayerHandler.Instance.particleStun.SetActive(true);
 
        while (actualTime < stunDuration)
        {
            actualTime += Time.deltaTime;
            PlayerHandler.Instance.StunPlayer();
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
        owner.anim.animator.SetBool("Vomit", true);
        yield return new WaitUntil(() => owner.anim.animator.
            GetCurrentAnimatorStateInfo(0).IsName("Vomit"));
        
        owner.anim.animator.SetBool("Vomit", false);
        yield return new WaitUntil(() => !owner.anim.animator.
            GetCurrentAnimatorStateInfo(0).IsName("Vomit"));

        owner.attackEnded = true;
    }

    public void ThrowVomit()
    {
        owner.VomitSound();
        var ball = Instantiate(vomitBall, vomitBallStart.position, vomitBallStart.rotation);
        _actualBall = ball.GetComponent<VomitBall>();
        ball.transform.forward = vomitBallStart.forward;
        ball.GetComponent<VomitBall>().StartTrajectory();
    }

    #endregion
    public override Dictionary<string, object> Exit(IState to)
    {
        startNode = null;
        goal = null;
        _corroutine = false;
        nextAction = null;
        if(_actualBall != null) _actualBall.Explode();
        if (owner.crossUsed)
        {
            PlayerHandler.Instance.PossesPlayer();
            PlayerHandler.Instance.particleStun.SetActive(false);
        }
        StopAllCoroutines();
        print("Sali de attack");
        waitingTime = 0;
        
        owner.anim.ChangeState("SwarmAttack", false);
        owner.anim.ChangeState("Vomit", false);

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
