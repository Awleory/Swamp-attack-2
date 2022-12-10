using System;
using System.Diagnostics.Contracts;

public class EnemyStateMachine : StateMachine
{
    private AttackState _attackState;
    private ChaseState _chaseState;
    private IdleState _idleState;
    private DeathState _deathState;

    private readonly Predicate<bool> _truePredicate = new(value => value);
    private readonly Predicate<bool> _falsePredicate = new(value => value == false);

    private const string _targetReachedParameter = "TargetReached";
    private const string _targetIsAliveParameter = "TargetIsAlive";
    private const string _isAliveParameter = "IsAlive";

    public EnemyStateMachine(Enemy enemy)
    {
        _attackState = new AttackState(enemy);
        AddState(_attackState);

        _chaseState = new ChaseState(enemy);
        AddState(_chaseState);   

        _idleState = new IdleState();
        AddState(_idleState);

        _deathState = new DeathState();
        AddState(_deathState);

        Parameters.AddParameter(_targetReachedParameter, false);
        Parameters.AddParameter(_targetIsAliveParameter, true);
        Parameters.AddParameter(_isAliveParameter, true);
    }

    public void Initialize()
    {
        InitilizeAnyState();

        Initialize(_idleState);
    }

    public void SetParameterTargetReached(bool state)
    {
        Parameters.SetParameter(_targetReachedParameter, state);
    }

    public void SetParameterTargetIsAlive(bool state)
    {
        Parameters.SetParameter(_targetIsAliveParameter, state);
    }

    public void SetParameterIsAlive(bool state)
    {
        Parameters.SetParameter(_isAliveParameter, state);
    }

    private void InitilizeAnyState()
    {
        var transition = new StateTransition(_attackState, Parameters);
        transition.AddBoolPredicate(_targetReachedParameter, _truePredicate);
        transition.AddBoolPredicate(_targetIsAliveParameter, _truePredicate);
        AnyState.AddTransition(transition);

        transition = new StateTransition(_chaseState, Parameters);
        transition.AddBoolPredicate(_targetReachedParameter, _falsePredicate);
        transition.AddBoolPredicate(_targetIsAliveParameter, _truePredicate);
        AnyState.AddTransition(transition);

        transition = new StateTransition(_idleState, Parameters);
        transition.AddBoolPredicate(_targetIsAliveParameter, _falsePredicate);
        AnyState.AddTransition(transition);

        transition = new StateTransition(_deathState, Parameters);
        transition.AddBoolPredicate(_isAliveParameter, _falsePredicate);
        AnyState.AddTransition(transition);
    }
}