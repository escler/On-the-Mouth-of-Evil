using UnityEngine;

public class IllusionDuplication : EnemySteeringAgent
{
    private FiniteStateMachine _fsm;
    private Transform _characterPos;
    private IllusionBossDuplicationsAnim _anim;
    [SerializeField] private DecisionNode _decisionTree;


    public float speedWalk, speedRun;
    public bool canHit;
    public float rangeForAttack;
    public actionsEnemy lastAction;


    public DecisionNode DecisionTree => _decisionTree;
    public IllusionBossDuplicationsAnim Anim => _anim;
    public Transform CharacterPos => _characterPos;
    
    private void Awake()
    {
        _fsm = new FiniteStateMachine();
        _characterPos = Player.Instance.transform;
        _anim = GetComponentInChildren<IllusionBossDuplicationsAnim>();
        _fsm.AddState(States.Idle, new BossDuplication_Idle(this));
        _fsm.AddState(States.Moving, new BossDuplication_Move(this));
        _fsm.AddState(States.Attack, new BossDuplication_Attack(this));
        _fsm.AddState(States.SpecialAttack, new BossDuplication_Explode(this));
        
        _fsm.ChangeState(States.Idle);
    }
    
    public void ChangeToMove()
    {
        _fsm.ChangeState(States.Moving);
    }

    public void ChangeToAttack()
    {
        _fsm.ChangeState(States.Attack);
    }

    public void ChangeToExplode()
    {
        _fsm.ChangeState(States.SpecialAttack);
    }
}
