using UnityEngine;

public class MegaPositivePolarity : Polarity
{
    public override Color Color => Color.yellow;

    public override void InteractWithPocket()
    {
        ResourcesManager.Tries.Value += 5;
    }
}
