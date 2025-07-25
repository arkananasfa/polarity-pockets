using Unity.VisualScripting;
using UnityEngine;

public abstract class Polarity
{
    public abstract Color Color { get; }

    protected Ball ball;

    public virtual void Init(Ball ball)
    {
        this.ball = ball;
        ball.SetColor(Color);
    }

    public virtual void End(Ball ball) {}
    
    public virtual void InteractWithPlayer() {}
    public virtual void InteractWithBall() {}
    public virtual void InteractWithWall() {}
    public virtual void InteractWithPocket() {}

}