using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Player")]
    [SerializeField] private PlayerBall playerBall;
    [SerializeField] private Transform playerStartPoint;
    
    [Header("Levels")]
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<LevelConfig> levelConfigs;
    [SerializeField] private Ball ballPrefab;

    public static ObservableValue<int> currentRound = new ObservableValue<int>(1);

    private int _ballsCount;
    
    private void Awake()
    {
        ResourcesManager.Tries.OnValueChanged += CheckLose;
        GlobalEventManager.OnBallOnPocket += CheckWin;
        GlobalEventManager.OnBackToGameButtonClicked += StartLevel;
    }
    
    private void Start()
    {
        StartLevel();
    }

    private void StartLevel()
    {
        playerBall.transform.position = playerStartPoint.position;
        
        _ballsCount = spawnPoints.Count;
        foreach (var sp in spawnPoints)
        {
            var ball = SpawnBall();
            ball.transform.position = sp.position;
        }
    }

    private Ball SpawnBall()
    {
        Ball ball = Instantiate(ballPrefab);
        ball
    }

    private PolaritiyType SelectPolarities()
    {
        LevelConfig config = levelConfigs[currentRound.Value-1];
        int rand = Random.Range(1, 101);
        foreach (var polaritySetts in config.levelConfig)
        {
            rand -= polaritySetts.percent;
            if (rand <= 0)
                return polaritySetts.polarityType;
        }

        return PolaritiyType.Positive;
    }

    private void CheckWin()
    {
        if (--_ballsCount == 0)
        {
            Win();
        }
    }
    
    private void CheckLose(int value)
    {
        if (value <= 0)
            Lose();
    }

    private void Win()
    {
        GlobalEventManager.Win(currentRound.Value);
        currentRound.Value++;
    }

    private void Lose()
    {
        GlobalEventManager.Lose(currentRound.Value);
    }
    
}