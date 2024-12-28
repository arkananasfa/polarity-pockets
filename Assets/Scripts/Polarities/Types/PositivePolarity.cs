using UnityEngine;

public class PositivePolarity : Polarity
{
    protected override Color Color => Color.cyan;

    public override void InteractWithPocket()
    {
        ResourcesManager.Tries.Value += 2;
    }
}
