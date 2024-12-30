using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ball : MonoBehaviour, ICanBeExploded
{
    private int CurrentPolarityIndex
    {
        get => _currentPolarityIndex;
        set
        {
            CurrentPolarity?.End(this);
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
    
    private int NextIndex => CurrentPolarityIndex + 1 >= _polarities.Count ? 0 : CurrentPolarityIndex + 1;
    private Polarity NextPolarity => _polarities[NextIndex];

    [SerializeField] private List<PolaritiyType> polarityTypes = new List<PolaritiyType>
        { PolaritiyType.Positive, PolaritiyType.Negative };

    [SerializeField] private float friction = 0.99f;
    [SerializeField] private ParticleSystem collisionParticles;
    [SerializeField] private SpriteRenderer _nextPolaritySpriteRenderer;

    private List<Polarity> _polarities = new List<Polarity>();
    private Polarity _currentPolarity;
    private int _currentPolarityIndex = 0;
    private Rigidbody2D _rb;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        polarityTypes.ForEach(type => _polarities.Add(PolarityFactory.CreatePolarity(type)));
        if (_polarities.Count == 0)
            throw new Exception("No polarities found");
        CurrentPolarityIndex = 0;
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity *= friction;
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;

        if (collisionParticles != null)
        {
            var main = collisionParticles.main;
            main.startColor = color;
        }

        _nextPolaritySpriteRenderer.color = NextPolarity.Color;
    }

    public void SetPolarities(PolaritiyType[] polarities)
    {
        polarityTypes = polarities.ToList();
    }

    public float GetVelocity()
    {
        return _rb.linearVelocity.magnitude;
    }

    public void SetMass(float mass)
    {
        _rb.mass = mass;
    }

    public void AddExplosionForce(Vector2 from, float force)
    {
        _rb.AddForce(((Vector2)transform.position - from).normalized*force, ForceMode2D.Impulse);
    }

    public void ReversePolarity()
    {
        if (collisionParticles != null)
        {
            collisionParticles.transform.position = transform.position;
            collisionParticles.Play();
        }
        CurrentPolarityIndex++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CurrentPolarity.InteractWithPocket();
        GlobalEventManager.BallOnPocket();
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

        ReversePolarity();
    }
}
