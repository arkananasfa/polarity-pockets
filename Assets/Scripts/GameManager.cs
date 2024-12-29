using UnityEngine;

public class GameManager : MonoBehaviour
{

    private void Awake()
    {
        ResourcesManager.Tries.OnValueChanged += CheckLose;
    }
    
    private void Start()
    {
        StartLevel();
    }

    private void StartLevel()
    {
        
    }

    private void CheckLose(int value)
    {
        if (value <= 0)
            Lose();
    }

    private void Lose()
    {
        
    }
    
}