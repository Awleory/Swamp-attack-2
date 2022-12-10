using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private int _reward;
    [SerializeField] private int _damage;
    [SerializeField] private float _attackDelay = 1;

    public Player Target => _target;
    public int Damage => _damage;
    public float AttackDelay => _attackDelay;
    public bool IsAttacking => _attackCoroutine != null;

    private Player _target;
    private EnemyStateMachine _stateMachine;
    private bool _targetIsNear;
    private Coroutine _attackCoroutine;
    private float _attackSpeedRate = 0.2f;

    protected override void Awake()
    {
        base.Awake();
        _stateMachine = new EnemyStateMachine(this);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _stateMachine.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _stateMachine.OnDisable();
    }

    protected override void Start()
    {
        base.Start();
        _stateMachine.Initialize();    
    }

    protected override void Update()
    {
        base.Update();
        _stateMachine.Update(Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProcessTriggerCollision(collision, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ProcessTriggerCollision(collision, false);
    }

    public void Initialize(Player target)
    {
        _target = target;
    }

    public bool TryAttack()
    {
        if (_attackCoroutine == null)
        {
            _attackCoroutine = StartCoroutine(AttackCoroutine());
            return true;
        }

        return false;
    }

    protected override void OnDied()
    {
        base.OnDied();
        _stateMachine.SetParameterIsAlive(false);
        _target.OnEnemyDied(_reward);
    }

    private IEnumerator AttackCoroutine()
    {
        AnimationController.PlayAttack();
        yield return new WaitForSeconds(_attackSpeedRate);

        if (_targetIsNear)
            _target.TakeDamage(Damage);

        _attackCoroutine = null;
    }

    private void ProcessTriggerCollision(Collider2D collision, bool isEnter)
    {
        if (collision.TryGetComponent(out Player _))
        {
            _stateMachine.SetParameterTargetReached(isEnter);
            _targetIsNear = isEnter;
        }
    }
}
