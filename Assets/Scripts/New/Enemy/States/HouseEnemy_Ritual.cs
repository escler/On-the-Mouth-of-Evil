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
    private bool _pathCalculated, _pathFinish, bibleBurning, ritualReached, startRitualCor;
    
    
    public override void UpdateLoop()
    {
        if(owner.activateGoodExorcism) owner.EnemyAnimator.ChangeStateAnimation("Exorcism", true);
        
        if (!ritualReached) GoToNodeGoal();
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
        if(TarotCardPuzzle.Instance.BadPathTaked) StartCoroutine(RitualBadSequenceCor());
        else StartCoroutine(RitualGoodSequenceCor());
    }

    IEnumerator RitualBadSequenceCor()
    {
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
        yield return new WaitForSeconds(0.7f);
        float timer = 0;

        while (timer < 1)
        {
            Vector3 target = PlayerHandler.Instance.transform.position;
            target.y = owner.transform.position.y;
            Vector3 dir = target - owner.transform.position;
            owner.transform.rotation = Quaternion.LookRotation(dir);
            owner.transform.position += dir.normalized * owner.speed * Time.deltaTime;
            timer += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.3f);
        bool inNode = false;
        if (CorduraHandler.Instance.CorduraOn > 0) CorduraHandler.Instance.CorduraOn = 0;
        while (!inNode)
        {
            Vector3 target = RitualManager.Instance.ritualNode.transform.position;
            target.y = owner.transform.position.y;
            Vector3 dir = target - owner.transform.position;
            Vector3 targetPlayer = PlayerHandler.Instance.transform.position;
            targetPlayer.y = owner.transform.position.y;
            Vector3 dirPlayer = targetPlayer - owner.transform.position;
            inNode = Vector3.Distance(owner.transform.position, target) <= 0.1f;
            owner.transform.rotation = Quaternion.LookRotation(dirPlayer);
            owner.transform.position += dir.normalized * owner.speed / 3 * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }

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