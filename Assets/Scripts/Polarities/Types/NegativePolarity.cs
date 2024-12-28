using UnityEngine;

public class NegativePolarity : Polarity
{
    protected override Color Color => Color.red;
    
    public override void InteractWithPocket()
    {
        ResourcesManager.Tries.Value -= 1;
    }
}
