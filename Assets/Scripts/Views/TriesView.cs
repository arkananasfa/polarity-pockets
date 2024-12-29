using System;
using TMPro;
using UnityEngine;

public class TriesView : MonoBehaviour
{

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        ResourcesManager.Tries.OnValueChanged += SetValue;
    }
    
    private void Start()
    {
        SetValue(ResourcesManager.Tries.Value);
    }

    private void SetValue(int value)
    {
        _text.text = (value > 0 ? value : 0) + " tries";
    }
}