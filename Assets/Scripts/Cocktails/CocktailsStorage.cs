using System.Collections.Generic;
using UnityEngine;

public class CocktailsStorage : MonoBehaviour
{
    
    public List<CocktailModel> cocktailModels;
    public Dictionary<CocktailType, int> cocktails = new();

    #region singleton

    public static CocktailsStorage instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    #endregion

    public void AddCocktail(CocktailType type)
    {
        if (cocktails.ContainsKey(type))
            cocktails[type]++;
        else
            cocktails.Add(type, 1);
    }
    
}