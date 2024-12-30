using UnityEngine;

public static class CocktailFactory
{

    public static Cocktail GetCocktail(CocktailType type) => type switch
    {
        CocktailType.Teleport => new Teleport(),
        CocktailType.ReversePolarity => new ReversePolarity(),
        CocktailType.PocketPlus => new PocketPlus(),
        CocktailType.ChaosBomb => new ChaosBomb(),
        _ => new Teleport()
    };

}