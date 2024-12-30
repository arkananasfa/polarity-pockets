using System.Linq;
using UnityEngine;

public class ChaosBomb : Cocktail
{
    public override void Use()
    {
        var ballsGO = GameObject.FindGameObjectsWithTag("Ball");
        var balls = ballsGO.Select(b => b.GetComponent<Ball>()).ToList();
        balls.ForEach(b =>
        {
            Vector2 from = new Vector2(Random.Range(-7.55f, 7.55f), Random.Range(-3.75f, 3.75f));
            b.AddExplosionForce(from, 10f);
        });
    }
}
