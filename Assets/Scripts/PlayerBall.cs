using UnityEngine;
using UnityEngine.Serialization;

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
    [SerializeField] private Vector2 minPower;
    [SerializeField] private Vector2 maxPower;
    [SerializeField] private LineRenderer predictionLine;

    private Vector2 _force;
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private TrajectoryLine _tl;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tl = GetComponent<TrajectoryLine>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startPoint = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            _startPoint.z = 15f;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPoint = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            _startPoint.z = 15f;
            var endPos = currentPoint - _startPoint + transform.position;
            // _tl.RenderLine(_startPoint, currentPoint);
            var startPos = transform.position;
            endPos.z = 15f;
            startPos.z = 15f;
            _tl.RenderLine(startPos, endPos);
            
            var diff = _startPoint - currentPoint;
            Vector2 velocity = new Vector2(
                Mathf.Clamp(diff.x, minPower.x, maxPower.x),
                Mathf.Clamp(diff.y, minPower.y, maxPower.y)
            ) * powerMultiplier;

            // Предсказываем траекторию
            Vector2[] trajectory = Plot(_rb, (Vector2)transform.position, velocity, 4, 0.01f);
    
            // Рендерим траекторию с predictionLine
            RenderPredictionLine(trajectory);

            // Отображение траектории
            RenderPredictionLine(trajectory);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _endPoint = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            _endPoint.z = 15f;

            var diff = _startPoint - _endPoint;
            
            _force = new Vector2(Mathf.Clamp(diff.x, minPower.x, maxPower.x),
                Mathf.Clamp(diff.y, minPower.y, maxPower.y));
            
            _rb.AddForce(_force * powerMultiplier, ForceMode2D.Impulse);
            _tl.EndLine();
            predictionLine.positionCount = 0;
        }
    }
    
    private void RenderPredictionLine(Vector2[] trajectory)
    {
        predictionLine.positionCount = trajectory.Length;
        Vector3[] positions = new Vector3[trajectory.Length];
        for (int i = 0; i < trajectory.Length; i++)
        {
            positions[i] = new Vector3(trajectory[i].x, trajectory[i].y, 15f); // Z-координата для 2D
        }
        predictionLine.SetPositions(positions);
    }



     public Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps, float friction)
    {
        Vector2[] results = new Vector2[steps];
        float timestep = Time.fixedDeltaTime;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; i++)
        {
            // Добавляем шаг движения
            pos += moveStep;

            // Сохраняем текущую позицию
            results[i] = pos;

            // Учитываем трение (уменьшение скорости)
            moveStep *= (1f - friction * timestep);

            // Прекращаем симуляцию, если скорость становится слишком маленькой
            if (moveStep.magnitude < 0.01f)
                break;
        }

        return results;
    }

    
}