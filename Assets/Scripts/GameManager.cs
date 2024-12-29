using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<Ball> _balls = new();
    
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
        
        LevelConfig config = currentRound.Value >= levelConfigs.Count 
            ? levelConfigs[^1]
            : levelConfigs[currentRound.Value-1];
        
        _ballsCount = 0;
        _balls.Clear();
        int count = 0;
        foreach (var sp in spawnPoints)
        {
            //if (count++ > 0)
                //break;
            _ballsCount++;
            var ball = SpawnBall(config);
            ball.transform.position = sp.position;
            _balls.Add(ball);
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
        {
            StartCoroutine(ChekingLose());
        }
    }
    
    private IEnumerator ChekingLose()
    {
        yield return new WaitForSeconds(0.3f);
        while (SomethingIsMoving() && ResourcesManager.Tries.Value <= 0 && _ballsCount > 0)
        {
            yield return new WaitForSeconds(0.2f);
        }

        if (ResourcesManager.Tries.Value <= 0 && _ballsCount > 0)
            Lose();
    }

    private bool SomethingIsMoving()
    {
        return playerBall.GetVelocity() > 0.5f || _balls.Any(b =>
        {
            if (b != null)
                return b.GetVelocity() > 0.5f;
            else return false;
        });
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