using UnityEngine;

public class PocketPlus : Cocktail
{
    
    private GameObject pocket;
    
    public override void Use()
    {
        Vector2 location = new Vector2(Random.Range(-6.55f, 6.55f), Random.Range(-3f, 3f));
        pocket = Object.Instantiate(Resources.Load("Pocket"), location, Quaternion.identity) as GameObject;
    }

    public override void End()
    {
        Object.Destroy(pocket);
    }
}
