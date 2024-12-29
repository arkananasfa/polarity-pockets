using System.Linq;
using UnityEngine;

public class ReversePolarity : Cocktail
{
    public override void Use()
    {
        var ballsGO = GameObject.FindGameObjectsWithTag("Ball");
        var balls = ballsGO.Select(b => b.GetComponent<Ball>()).ToList();
        balls.ForEach(b => b.ReversePolarity());
    }
}
