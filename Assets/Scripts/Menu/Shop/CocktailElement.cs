using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CocktailElement : MonoBehaviour
{
    
    [SerializeField] private Image iconImage;
    [SerializeField] private Image reflectionImage;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;

    private Button _button;
    private int _price;
    private CocktailType _type;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public CocktailElement Init(CocktailModel model)
    {
        gameObject.SetActive(true);
        iconImage.sprite = model.icon;
        reflectionImage.sprite = model.icon;
        descriptionText.text = model.description;
        costText.text = model.price.ToString() + "$";

        iconImage.preserveAspect = model.type == CocktailType.ReversePolarity || model.type == CocktailType.ChaosBomb;
        reflectionImage.preserveAspect = model.type == CocktailType.ReversePolarity || model.type == CocktailType.ChaosBomb;
        
        _price = model.price;
        _type = model.type;
        
        CheckIsAvailable(ResourcesManager.Money.Value);
        return this;
    }
    
    private void OnEnable()
    {
        ResourcesManager.Money.OnValueChanged += CheckIsAvailable;
        _button.onClick.AddListener(BuyCocktail);
    }

    private void OnDisable()
    {
        ResourcesManager.Money.OnValueChanged -= CheckIsAvailable;
        _button.onClick.RemoveListener(BuyCocktail);
    }

    private void CheckIsAvailable(int money)
    {
        _button.interactable = money >= _price;
    }

    private void BuyCocktail()
    {
        ResourcesManager.Money.Value-= _price;
        CocktailsStorage.instance.AddCocktail(_type);
        gameObject.SetActive(false);
    }

}