using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{

    [SerializeField] private CocktailElement cocktailElementPrefab;
    [SerializeField] private RectTransform container;

    private List<CocktailElement> _cocktailElements;
    
    public void Open()
    {
        var cocktails = GetThreeRandomCocktails();
        foreach (var cocktail in cocktails)
        {
            CocktailElement element = Instantiate(cocktailElementPrefab, container);
            element.Init(cocktail);
        }
    }

    private CocktailModel[] GetThreeRandomCocktails()
    {
        CocktailModel[] models = new CocktailModel[3];

        int cocktailTypesCount = CocktailsStorage.instance.cocktailModels.Count;
        
        int[] indices = new int[3];
        indices[0] = Random.Range(0, cocktailTypesCount);
        indices[1] = Random.Range(0, cocktailTypesCount);
        indices[2] = Random.Range(0, cocktailTypesCount);
        while (indices[1] == indices[0])
            indices[1] = Random.Range(0, cocktailTypesCount);
        while (indices[2] == indices[0] || indices[2] == indices[1])
            indices[2] = Random.Range(0, cocktailTypesCount);
        
        for (int i = 0;i<indices.Length;i++)
            models[i] = CocktailsStorage.instance.cocktailModels[indices[i]];

        return models;
    }  

}