using System;
using TMPro;
using UnityEngine;

public class RoundView : MonoBehaviour
{

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        GameManager.currentRound.OnValueChanged += SetValue;
    }

    private void Start()
    {
        SetValue(GameManager.currentRound.Value);
    }

    private void SetValue(int value)
    {
        _text.text = value + " round";
    }
}