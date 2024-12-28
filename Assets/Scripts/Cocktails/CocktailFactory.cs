using UnityEngine;

public static class CocktailFactory
{

    public static Cocktail GetCocktail(CocktailType type) => type switch
    {
        CocktailType.Teleport => new Teleport(),
        _ => new Teleport()
    };

}