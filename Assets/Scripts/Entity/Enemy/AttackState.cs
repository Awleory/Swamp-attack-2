public class AttackState : State
{
    private Enemy _enemy;
    private float _passedTime;
    private bool _attackIsReady = true;

    public AttackState(Enemy enemy)
    {
        _enemy = enemy;
    }
    public override void Enter()
    {
        base.Enter();
        _passedTime = 0;
    }

    public override void LogicUpdate()
    {
        if (_attackIsReady && _enemy.TryAttack())
            _attackIsReady = false;

        CanExit = _enemy.IsAttacking == false;
    }

    public override void Update(float deltaTime, State currentState = null)
    {
        base.Update(deltaTime, currentState);

        if (_attackIsReady == false)
        {
            _passedTime += deltaTime;

            if (_passedTime >= _enemy.AttackDelay)
            {
                _passedTime -= _enemy.AttackDelay;
                _attackIsReady = true;
            }
        }
    }
}
