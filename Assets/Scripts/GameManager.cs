using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static bool IsBlocked = false;
    
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
        GlobalEventManager.OnBackToGameButtonClicked += StartNextLevel;
    }
    
    private void Start()
    {
        StartLevel();
    }

    private void StartNextLevel()
    {
        currentRound.Value++;
        StartLevel();
    }
    
    private void StartLevel()
        {
            playerBall.transform.position = playerStartPoint.position;
            
            if (currentRound.Value >= levelConfigs.Count)
                levelConfigs[^1].startTries--;
            LevelConfig config = currentRound.Value >= levelConfigs.Count 
                ? levelConfigs[^1]
                : levelConfigs[currentRound.Value-1];
            
            _ballsCount = 0;
            int count = 0;
            foreach (var sp in spawnPoints)
            {
                if (count++ > 0)
                    break;
                _ballsCount++;
                var ball = SpawnBall(config);
                ball.transform.position = sp.position;
            }
            ResourcesManager.Tries.Value = config.startTries;
            IsBlocked = false;
        }
    
    private Ball SpawnBall(LevelConfig config)
    {
        Ball ball = Instantiate(ballPrefab);
        PolaritiyType[] polarities = new PolaritiyType[2];
        polarities[0] = SelectPolarities(config);
        int tries = 0;
        do
        {
            polarities[1] = SelectPolarities(config);
            tries++;
        } while (polarities[1] == polarities[0] && tries < 30);
        ball.SetPolarities(polarities);
        ball.Init();
        return ball;
    }

    private PolaritiyType SelectPolarities(LevelConfig config)
    {
        int rand = Random.Range(1, 100);
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
        if (_ballsCount-1 == 0)
        {
            Win();
        }
        _ballsCount--;
    }
    
    private void CheckLose(int value)
    {
        if (value <= 0)
            Lose();
    }

    private void Win()
    {
        ResourcesManager.Money.Value += ResourcesManager.Tries.Value;
        GlobalEventManager.Win(currentRound.Value);
        IsBlocked = true;
    }

    private void Lose()
    {
        GlobalEventManager.Lose(currentRound.Value);
        IsBlocked = true;
    }
    
}