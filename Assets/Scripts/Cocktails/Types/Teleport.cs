using UnityEngine;

public class Teleport : Cocktail
{
    public override void Use()
    {
        GameObject.FindAnyObjectByType<PlayerBall>().PlayerLocate();
    }
}
