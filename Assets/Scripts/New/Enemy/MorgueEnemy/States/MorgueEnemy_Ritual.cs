using System.Collections;
using System.Collections.Generic;
using FSM;
using Unity.VisualScripting;
using UnityEngine;
using IState = FSM.IState;

public class MorgueEnemy_Ritual : MonoBaseState
{
    [SerializeField] private MorgueEnemy owner;
    private List<Transform> _path;
    private Node _startNode, _goalNode;
    private bool _pathCalculated, _pathFinish, bibleBurning, ritualReached, startRitualCor, corActivate;
    private IEnumerator _nextAction;
    
    
    public override void UpdateLoop()
    {
        if (!ritualReached) Startcort();
        //else StartRitualSequence();

        if (_pathFinish && owner.ritualDone)
        {
            ritualReached = true;
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
        corActivate = true;
        StartCoroutine(Startcor());
    }

    IEnumerator Startcor()
    {
        StartCoroutine(HideEnemy());
        while (owner.enemyVisibility > 0)
        {
            yield return new WaitForSeconds(0.01f);
        }
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

    }

    IEnumerator RitualGoodSequenceCor()
    {
        yield return null;
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
    
    IEnumerator HideEnemy()
    {
        while (owner.enemyVisibility > 0)
        {
            owner.enemyVisibility -= .3f;
            owner.enemyMaterial.SetFloat("_Power", owner.enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }
        owner.enemyVisible = false;
        
        GoToNodeGoal();
    }

    private void GoToNodeGoal()
    {
        if (_pathFinish) return;
        Vector3 target = _goalNode.transform.position;
        target.y = owner.transform.position.y;
        owner.transform.position = target;
        StartCoroutine(RitualBadSequenceCor());
        if (Vector3.Distance(target, owner.transform.position) <= 0.1f)
        {
            _pathFinish = true;
            owner.inRitualNode = true;
        }
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
