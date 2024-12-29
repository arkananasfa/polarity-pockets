using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopUI : MonoBehaviour
{

    #region Singleton

    public static ShopUI instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion
    
    [SerializeField] private CocktailElement cocktailElementPrefab;
    [SerializeField] private RectTransform container;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private CanvasGroup canvasGroup;

    private List<CocktailElement> _cocktailElements;

    private void OnEnable()
    {
        GlobalEventManager.OnGoToShopButtonClicked += Open;
    }

    public void Open()
    {
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1f, 0.6f);
        shopPanel.SetActive(true);
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