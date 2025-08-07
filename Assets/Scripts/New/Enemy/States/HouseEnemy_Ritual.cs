using System.Collections;
using System.Collections.Generic;
using FSM;
using Unity.Mathematics;
using UnityEngine;

public class HouseEnemy_Ritual : MonoBaseState
{
    [SerializeField] private HouseEnemy owner;
    private List<Transform> _path;
    private Node _startNode, _goalNode;
    private bool _pathCalculated, _pathFinish, bibleBurning, ritualReached, startRitualCor, corActivate;
    
    
    public override void UpdateLoop()
    {
        //if(owner.activateGoodExorcism) owner.EnemyAnimator.ChangeStateAnimation("Exorcism", true);

        if (!ritualReached) Startcort();
        else StartRitualSequence();

        if (_pathFinish && owner.ritualDone)
        {
            ritualReached = true;
            owner.ShowEnemyRitual();
        }
    }

    void StartRitualSequence()
    {
        if (startRitualCor) return;
        startRitualCor = true;
        if(DecisionsHandler.Instance.badPath) StartCoroutine(RitualBadSequenceCor());
        else StartCoroutine(RitualGoodSequenceCor());
    }

    void Startcort()
    {
        if (corActivate) return;
        StartCoroutine(Startcor());
    }

    IEnumerator Startcor()
    {
        if(owner.enemyVisibility > 0)
        {
            owner.enemyVisibility = 0;
            owner.enemyMaterial.SetFloat("_Power", owner.enemyVisibility);
        }

        yield return null;

        GoToNodeGoal();
    }

    IEnumerator RitualBadSequenceCor()
    {
        yield return new WaitUntil(() => PlayerHandler.Instance.movement.inSpot);
        Vector3 target = PlayerHandler.Instance.transform.position;
        target.y = owner.transform.position.y;
        Vector3 dir = target - owner.transform.position;
        owner.transform.rotation = Quaternion.LookRotation(dir);
        yield return new WaitForSeconds(0.7f);
        
        if (CorduraHandler.Instance.CorduraOn > 0) CorduraHandler.Instance.CorduraOn = 0;
        yield return new WaitUntil(() => owner.activateBadExorcism);
        yield return new WaitUntil(() => owner.enemyVisibility <= 0);
    }

    IEnumerator RitualGoodSequenceCor()
    {
        yield return new WaitUntil(() => PlayerHandler.Instance.movement.inSpot);
        yield return new WaitUntil(() => owner.enemyVisibility <= 0);
        yield return new WaitForSeconds(0.7f);
        Vector3 target = PlayerHandler.Instance.transform.position;
        target.y = owner.transform.position.y;
        Vector3 dir = target - owner.transform.position;
        owner.transform.rotation = Quaternion.LookRotation(dir);
        owner.normalMesh.SetActive(false);
        owner.ritualMesh.SetActive(true);
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        print("Entre a ritual");
        _startNode = PathFindingManager.instance.CalculateDistance(owner.transform.position);
        _goalNode = owner.nodeRitual;
        _path = owner.pf.ThetaStar(_startNode, _goalNode, owner.obstacles);
        
        if (_path.Count > 0)
        {
            _path.Reverse();
        }
        _pathCalculated = true;
    }
    
    private void TravelPath()
    {
        Vector3 target = _path[0].position;
        target.y = owner.transform.position.y;
        Vector3 dir = target - owner.transform.position;
        owner.transform.rotation = Quaternion.LookRotation(dir);
        owner.transform.position += dir.normalized * (owner.speed * Time.deltaTime);
        
        if (Vector3.Distance(target, owner.transform.position) <= 0.1f) _path.RemoveAt(0);
    }

    private void GoToNodeGoal()
    {
        if (_pathFinish) return;
        Vector3 target = _goalNode.transform.position;
        target.y = owner.transform.position.y;
        owner.transform.position = target;
        if (Vector3.Distance(target, owner.transform.position) <= 0.1f) _pathFinish = true;
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        _path.Clear();
        _pathCalculated = false;
        _pathFinish = false;
        return base.Exit(to);
    }

    public override IState ProcessInput()
    {
        return this;
    }
}