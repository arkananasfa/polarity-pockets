using System;
using TMPro;
using UnityEngine;

public class MoneyView : MonoBehaviour
{

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        ResourcesManager.Money.OnValueChanged += SetValue;
    }

    private void Start()
    {
        SetValue(ResourcesManager.Money.Value);
    }

    private void SetValue(int value)
    {
        _text.text = value + "$";
    }
}