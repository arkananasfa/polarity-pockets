using System;
using UnityEngine;

public class PlayerBall : MonoBehaviour
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
    [SerializeField] private float sphereRadius = 0.4f; // Радиус сферы для SphereCast

    private Vector2 _force;
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private TrajectoryLine _tl;
    private bool _flag;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tl = GetComponent<TrajectoryLine>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            _tl.EndLine();
            predictionLine.positionCount = 0;
            _flag = false;
            return;
        }

        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
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

            Vector2[] trajectory = PlotWithCollisions(_rb, (Vector2)transform.position, velocity, 8, 0.01f, LayerMask.GetMask("Table", "Balls"));

            RenderPredictionLine(trajectory);
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
            _flag = false;
            ResourcesManager.Tries.Value--;
        }
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity *= friction;
    }

    private void RenderPredictionLine(Vector2[] trajectory)
    {
        predictionLine.positionCount = trajectory.Length;
        Vector3[] positions = new Vector3[trajectory.Length];
        for (int i = 0; i < trajectory.Length; i++)
        {
            positions[i] = new Vector3(trajectory[i].x, trajectory[i].y, 15f);
        }
        predictionLine.SetPositions(positions);
    }

    public Vector2[] PlotWithCollisions(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps, float friction, LayerMask collisionMask)
    {
        Vector2[] results = new Vector2[steps];
        float timestep = Time.fixedDeltaTime;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; i++)
        {
            RaycastHit2D hit = Physics2D.CircleCast(pos, sphereRadius, moveStep.normalized, moveStep.magnitude, collisionMask);
            if (hit.collider != null)
            {
                results[i] = hit.point;

                Vector2 normal = hit.normal;
                moveStep = Vector2.Reflect(moveStep, normal);

                moveStep *= (1f - friction * timestep);

                pos = hit.point + moveStep.normalized * sphereRadius; // Смещаем для предотвращения повторных столкновений
            }
            else
            {
                pos += moveStep;

                moveStep *= (1f - friction * timestep);

                if (moveStep.magnitude < 0.01f)
                    break;

                results[i] = pos;
            }
        }

        return results;
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, sphereRadius);
    }
}
