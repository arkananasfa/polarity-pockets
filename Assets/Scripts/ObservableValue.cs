using System;
using UnityEngine;

public class ObservableValue<T>
{
    
    public event Action<T> OnValueChanged;

    public ObservableValue(T value)
    {
        Value = value;
    }
    
    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            OnValueChanged?.Invoke(value);
        }
    }

    private T _value;

}