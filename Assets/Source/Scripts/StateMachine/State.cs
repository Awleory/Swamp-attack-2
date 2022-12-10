using System;
using System.Collections.Generic;

public abstract class State
{
    public event Action<State> TransitionEnded;

    public bool CanExit { get; protected set; } = true;

    private List<StateTransition> _transitions = new List<StateTransition>();
    protected bool _enable;

    public virtual void Enter()
    { 
        _enable = true;
    }

    public virtual void Exit() 
    {
        _enable = false;
    }

    public virtual void LogicUpdate() { }

    public void AddTransition(StateTransition transition)
    {
        _transitions.Add(transition);
    }

    public virtual void Update(float deltaTime, State currentActiveState)
    {
        if (_enable == false)
            return;

        foreach(var transition in _transitions)
        {
            if (transition.IsNeedTransit(currentActiveState))
            {
                TransitionEnded?.Invoke(transition.TargetState);
            }
        }
    }
}
