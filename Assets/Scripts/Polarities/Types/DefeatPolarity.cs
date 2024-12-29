using UnityEngine;

public class DefeatPolarity : Polarity
{
    public override Color Color => Color.black;
    
    public override void InteractWithPocket()
    {
        GlobalEventManager.Lose(GameManager.currentRound.Value);
    }
}
