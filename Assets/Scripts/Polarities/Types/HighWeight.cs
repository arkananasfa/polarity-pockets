using UnityEngine;

public class HighMass : Polarity
{
    public override Color Color => Color.blue;

    public override void Init(Ball ball)
    {
        base.Init(ball);
        ball.SetMass(10f);
    }

    public override void End(Ball ball)
    {
        ball.SetMass(2f);
    }
    
    public override void InteractWithPocket()
    {
        ResourcesManager.Tries.Value += 2;
    }
}