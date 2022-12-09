using System;
using System.Collections.Generic;

public abstract class StateMachine
{
    public event Action<State> StateChanged;

    public State CurrentState { get; private set; }
    public AnyState AnyState { get; private set; }

    protected StateMachineParameters Parameters { get; private set; } = new StateMachineParameters();

    private readonly List<State> _states = new();

    public StateMachine()
    {
        AnyState = new AnyState();
        AddState(AnyState);
    }

    public void Initialize(State startState)
    {
        CurrentState = startState;
        CurrentState?.Enter();
    }

    public void TryChangeState(State state)
    {
        if (CurrentState.CanExit == false)
            return;

        CurrentState?.Exit();

        CurrentState = state;
        CurrentState?.Enter();

        StateChanged?.Invoke(CurrentState);
    }

    public void OnEnable()
    {
        _states.ForEach(state => state.TransitionEnded += OnTransitionEnded);
    }

    public void OnDisable()
    {
        _states.ForEach(state => state.TransitionEnded -= OnTransitionEnded);
    }

    public virtual void Update(float deltaTime)
    {
        CurrentState.LogicUpdate();

        _states.ForEach(state => state.Update(deltaTime, CurrentState));
    }

    protected void AddState(State newState)
    {
        _states.Add(newState);
    }

    private void OnTransitionEnded(State nextState)
    {
        TryChangeState(nextState);
    }
}
