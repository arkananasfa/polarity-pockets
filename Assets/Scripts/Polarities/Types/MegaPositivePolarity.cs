using UnityEngine;

public class MegaPositivePolarity : Polarity
{
    protected override Color Color => Color.yellow;

    public override void InteractWithPocket()
    {
        ResourcesManager.Tries.Value += 5;
    }
}
