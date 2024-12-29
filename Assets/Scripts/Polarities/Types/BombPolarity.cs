using UnityEngine;

public class BombPolarity : Polarity
{
    public override Color Color => new Color(1f, 0.5f, 0f, 1f);

    public override void InteractWithPocket()
    {
        ResourcesManager.Tries.Value -= 1;
    }

    public override void InteractWithPlayer()
    {
        
    }

    public override void InteractWithBall()
    {
        
    }
}