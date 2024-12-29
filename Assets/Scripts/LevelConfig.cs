using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Scriptable Objects/LevelConfig")]
public class LevelConfig : ScriptableObject
{

    public List<BallTypeConfig> levelConfig;
    public int startTries = 10;

}

[Serializable]
public class BallTypeConfig
{
    public PolaritiyType polarityType;
    public int percent;
}