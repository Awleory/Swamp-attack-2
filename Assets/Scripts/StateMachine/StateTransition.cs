using System.Collections.Generic;
using System;

public class StateTransition
{
    public State TargetState { get; private set; }

    private Dictionary<string, Predicate<bool>> _boolPredicates = new Dictionary<string, Predicate<bool>>();
    private Dictionary<string, Predicate<float>> _floatPredicates = new Dictionary<string, Predicate<float>>();
    private StateMachineParameters _parameters;

    public StateTransition(State targetState, StateMachineParameters parameters)
    {
        TargetState = targetState;
        _parameters = parameters;
    }

    public void AddBoolPredicate(string name, Predicate<bool> predicate)
    {
        _boolPredicates.Add(name, predicate);
    }

    public void AddFloatPredicate(string name, Predicate<float> predicate)
    {
        _floatPredicates.Add(name, predicate);
    }

    public bool IsNeedTransit(State currentState)
    {
        if (currentState == TargetState)
            return false;

        foreach(var predicate in _boolPredicates)
        {
            if (_parameters.TryGetParameter(predicate.Key, out bool parameterValue) == false)
                return false;

            if (predicate.Value(parameterValue) == false)
                return false;
        }

        foreach (var predicate in _floatPredicates)
        {
            if (_parameters.TryGetParameter(predicate.Key, out float parameterValue) == false)
                return false;

            if (predicate.Value(parameterValue) == false)
                return false;
        }

        return true;
    }
}

