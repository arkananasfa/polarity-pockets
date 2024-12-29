using UnityEngine;

public class HighMass : Polarity
{
    public override Color Color => Color.cyan;

    public virtual void Init(Ball ball)
    {
        base.Init(ball);
        ball.SetMass(5f);
    }

    public virtual void End(Ball ball)
    {
        ball.SetMass(2f);
    }
}