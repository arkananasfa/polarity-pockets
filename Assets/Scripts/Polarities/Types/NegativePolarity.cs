using UnityEngine;

public class NegativePolarity : Polarity
{
    public override Color Color => Color.red;
    
    public override void InteractWithPocket()
    {
        ResourcesManager.Tries.Value -= 1;
    }
}
