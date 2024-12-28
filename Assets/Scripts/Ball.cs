using System;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    private int CurrentPolarityIndex
    {
        get => _currentPolarityIndex;
        set
        {
            int realValue = value >= _polarities.Count ? 0 : value;
            _currentPolarityIndex = realValue;
            CurrentPolarity = _polarities[realValue];
        }
    }
    
    private Polarity CurrentPolarity
    {
        get => _currentPolarity;
        set
        {
            _currentPolarity = value;
            _currentPolarity.Init(this);
        }
    }
    
    [SerializeField] private  List<PolaritiyType> polarityTypes = new List<PolaritiyType>
        { PolaritiyType.Positive, PolaritiyType.Negative };
    
    private List<Polarity> _polarities = new List<Polarity>();
    private Polarity _currentPolarity;
    private int _currentPolarityIndex = 0;
    
    private SpriteRenderer _spriteRenderer;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        polarityTypes.ForEach(type => _polarities.Add(PolarityFactory.CreatePolarity(type)));
        if (_polarities.Count == 0)
            throw new Exception("No polarities found");
        CurrentPolarityIndex = 0;
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CurrentPolarity.InteractWithPocket();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoundController.Instance.PlayHitSound();
        if (collision.gameObject.CompareTag("Ball"))
        {
            CurrentPolarity.InteractWithBall();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            CurrentPolarity.InteractWithPlayer();
        }
        else
        {
            CurrentPolarity.InteractWithWall();
        }

        CurrentPolarityIndex++;
    }
}