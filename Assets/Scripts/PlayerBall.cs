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

    private Vector2 _force;
    private Vector3 _startPoint;
    private Vector3 _endPoint;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startPoint = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            _startPoint.z = 15f;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _endPoint = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            _endPoint.z = 15f;

            var diff = _startPoint - _endPoint;
            
            _force = new Vector2(Mathf.Clamp(diff.x, minPower.x, maxPower.x),
                Mathf.Clamp(diff.y, minPower.y, maxPower.y));
            
            _rb.AddForce(_force * powerMultiplier, ForceMode2D.Impulse);
        }
    }
    
}