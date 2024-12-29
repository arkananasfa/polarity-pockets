using UnityEngine;

public class PositivePolarity : Polarity
{
    public override Color Color => Color.cyan;

    public override void InteractWithPocket()
    {
        ResourcesManager.Tries.Value += 2;
    }
}
