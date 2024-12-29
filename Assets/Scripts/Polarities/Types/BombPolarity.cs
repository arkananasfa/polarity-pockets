using UnityEngine;

public class BombPolarity : Polarity
{
    private LayerMask mask = LayerMask.GetMask("Balls"); 
    
    public override Color Color => new Color(1f, 0.5f, 0f, 1f);

    
    public override void InteractWithPocket()
    {
        ResourcesManager.Tries.Value -= 1;
    }

    public override void End(Ball ball)
    {
        var colliders = Physics2D.OverlapCircleAll(ball.transform.position, 3f, mask);
        foreach (var collider in colliders)
        {
            var obj = collider.GetComponent<ICanBeExploded>();
            if (obj != ball)
                obj.AddExplosionForce(ball.transform.position, 10f);
        }
        Object.Destroy(Object.Instantiate(Resources.Load("Explosion"), ball.transform.position, Quaternion.identity), 3f);
    }
}