using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBall : MonoBehaviour, ICanBeExploded
{
    private static Camera MainCamera
    {
        get
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;
            return _mainCamera;
        }
    }
    private static Camera _mainCamera;

    [SerializeField] private float powerMultiplier = 10f;
    [SerializeField] private float friction = 0.99f;
    [SerializeField] private Vector2 minPower;
    [SerializeField] private Vector2 maxPower;
    [SerializeField] private LineRenderer predictionLine;
    [SerializeField] private LineRenderer predictionLine2;
    [SerializeField] private float sphereRadius = 0.4f; // Радиус сферы для SphereCast
    [SerializeField] private LayerMask collisionMask;

    private Vector2 _force;
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private TrajectoryLine _tl;
    private bool _flag;
    private bool _isOutOfBoard;
    private Collider2D _collider;
    
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _tl = GetComponent<TrajectoryLine>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            _tl.EndLine();
            predictionLine.positionCount = 0;
            predictionLine2.positionCount = 0;
            _flag = false;
            return;
        }
        
        if (_isOutOfBoard)
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position = position - Vector3.forward * position.normalized;
            transform.position = position;
            
            bool isInRange = Mathf.Abs(position.x) < 7f && Mathf.Abs(position.y) < 3.2f;
            
            Color color = isInRange ? Color.green : Color.red;
            color = new Color(color.r, color.g, color.b, 0.8f);
            _spriteRenderer.color = color;
            
            if (Input.GetMouseButtonDown(0))
            {
                if (isInRange)
                {
                    _isOutOfBoard = false;
                    _collider.enabled = true;
                    GameManager.IsBlocked = false;
                    _spriteRenderer.color = Color.white;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && ResourcesManager.Tries.Value > 0 && Time.timeScale != 0 && !GameManager.IsBlocked)
        {
            _startPoint = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            _startPoint.z = 15f;
            _flag = true;
        }

        if (_flag)
        {
            Vector3 currentPoint = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            _startPoint.z = 15f;
            var endPos = currentPoint - _startPoint + transform.position;
            var startPos = transform.position;
            endPos.z = 15f;
            startPos.z = 15f;
            _tl.RenderLine(startPos, endPos);

            var diff = _startPoint - currentPoint;
            Vector2 velocity = new Vector2(
                Mathf.Clamp(diff.x, minPower.x, maxPower.x),
                Mathf.Clamp(diff.y, minPower.y, maxPower.y)
            ) * powerMultiplier;

            PlotWithCollisions(velocity);
        }

        if (Input.GetMouseButtonUp(0) && _flag)
        {
            _endPoint = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            _endPoint.z = 15f;

            var diff = _startPoint - _endPoint;

            _force = new Vector2(Mathf.Clamp(diff.x, minPower.x, maxPower.x),
                Mathf.Clamp(diff.y, minPower.y, maxPower.y));

            _rb.AddForce(_force * powerMultiplier, ForceMode2D.Impulse);
            _tl.EndLine();
            predictionLine.positionCount = 0;
            predictionLine2.positionCount = 0;
            _flag = false;
            if (_force.magnitude > 0.03f)
                ResourcesManager.Tries.Value--;
        }
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity *= friction;
    }

    public void PlayerLocate()
    {
        _rb.linearVelocity = Vector2.zero;
        _collider.enabled = false;
        GameManager.IsBlocked = true;
        _isOutOfBoard = true;
    }

    /*private void RenderPredictionLine(Vector2[] trajectory)
    {
        predictionLine.positionCount = trajectory.Length;
        Vector3[] positions = new Vector3[trajectory.Length];
        for (int i = 0; i < trajectory.Length; i++)
        {
            positions[i] = new Vector3(trajectory[i].x, trajectory[i].y, 15f);
        }
        predictionLine.SetPositions(positions);
    }*/

    public void PlotWithCollisions(Vector2 direction)
    {
        //1
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1000f, collisionMask);

        Vector3[] results = new Vector3[2];
        results[0] = transform.position;
        results[1] = hit.point;
        
        predictionLine.positionCount = 2;
        predictionLine.SetPositions(results);
        
        //2
        RaycastHit2D hit2 = Physics2D.CircleCast(transform.position, 0.272f, direction, 1000f, collisionMask);
        if (hit2.collider != null)
        {
            if (hit2.collider.gameObject.TryGetComponent(out Ball ball))
            {
                Vector2 direct = (Vector2)ball.transform.position - hit2.point;
                ball.gameObject.layer = LayerMask.NameToLayer("PB");
                RaycastHit2D hit3 = Physics2D.Raycast(ball.transform.position, direct, 1000f, collisionMask);
                ball.gameObject.layer = LayerMask.NameToLayer("Balls");
                
                Vector3[] results2 = new Vector3[2];
                results2[0] = ball.transform.position;
                results2[1] = hit3.point;
                
                predictionLine2.positionCount = 2;
                predictionLine2.SetPositions(results2);
            }
            else
            {
                predictionLine2.positionCount = 0;
            }
        }
    }

    /*[SerializeField] private float rad = 0.55f;
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, rad);
    }*/


    // public void ShowPredictionLine(Vector2 startPoint, Vector2 velocity)
    // {
    //     Vector2[] trajectory = PlotWithCollisions(_rb, startPoint, velocity, 8, collisionMask);
    //     RenderPredictionLine(trajectory, velocity);
    // }
    //
    // private void HandleOutOfBoardState()
    // {
    //     Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     position = position - Vector3.forward * position.normalized;
    //     transform.position = position;
    //
    //     bool isInRange = Mathf.Abs(position.x) < 7f && Mathf.Abs(position.y) < 3.2f;
    //
    //     Color color = isInRange ? Color.green : Color.red;
    //     color = new Color(color.r, color.g, color.b, 0.8f);
    //     _spriteRenderer.color = color;
    //
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         if (isInRange)
    //         {
    //             _isOutOfBoard = false;
    //             _collider.enabled = true;
    //             GameManager.IsBlocked = false;
    //             _spriteRenderer.color = Color.white;
    //         }
    //     }
    // }
    
    public void AddExplosionForce(Vector2 from, float force)
    {
        _rb.AddForce(((Vector2)transform.position - from).normalized*force, ForceMode2D.Impulse);
    }
    
    public float GetVelocity()
    {
        return _rb.linearVelocity.magnitude;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        ResourcesManager.Tries.Value--;
        PlayerLocate();
    }
}
