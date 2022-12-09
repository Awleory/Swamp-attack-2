using System;
using System.Collections.Generic;

public class StateMachineParameters
{
    private Dictionary<string, bool> _boolParameters = new Dictionary<string, bool>();
    private Dictionary<string, float> _floatParameters = new Dictionary<string, float>();

    public void AddParameter(string name, bool value)
    {
        AddParameter(name, value, _boolParameters);
    }

    public void AddParameter(string name, float value)
    {
        AddParameter(name, value, _floatParameters);
    }

    public void SetParameter(string name, bool value)
    {
        SetParameter(name, value, _boolParameters);
    }

    public void SetParameter(string name, float value)
    {
        SetParameter(name, value, _floatParameters);
    }

    public bool TryGetParameter(string name, out bool value)
    {
        return TryGetValue(name, out value, _boolParameters);
    }

    public bool TryGetParameter(string name, out float value)
    {
        return TryGetValue(name, out value, _floatParameters);
    }

    private void AddParameter<TValue>(string name, TValue value, Dictionary<string, TValue> parameters)
    {
        if (parameters.ContainsKey(name))
            parameters[name] = value;
        else
            parameters.Add(name, value);
    }

    private void SetParameter<TValue>(string name, TValue value, Dictionary<string, TValue> parameters)
    {
        if (parameters.ContainsKey(name))
            parameters[name] = value;
        else
            throw new ArgumentOutOfRangeException(nameof(name));
    }

    private bool TryGetValue<TValue>(string name, out TValue value, Dictionary<string, TValue> parameters)
    {
        if (parameters.ContainsKey(name))
        {
            value = parameters[name];
            return true;
        }
        else
        {
            value = default(TValue);
            return false;
        }
    }
}
