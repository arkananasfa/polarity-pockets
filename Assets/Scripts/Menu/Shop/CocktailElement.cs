using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CocktailElement : MonoBehaviour
{
    
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;

    private Button _button;
    private int _price;
    private CocktailType _type;
    
    public CocktailElement Init(CocktailModel model)
    {
        iconImage.sprite = model.icon;
        nameText.text = model.name;
        descriptionText.text = model.description;
        costText.text = model.price.ToString();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(BuyCocktail);
        
        _price = model.price;
        _type = model.type;
        
        CheckIsAvailable(ResourcesManager.Money.Value);
        return this;
    }
    
    private void OnEnable()
    {
        ResourcesManager.Money.OnValueChanged += CheckIsAvailable;
    }

    private void OnDisable()
    {
        ResourcesManager.Money.OnValueChanged -= CheckIsAvailable;
    }

    private void CheckIsAvailable(int money)
    {
        _button.interactable = money < _price;
    }

    private void BuyCocktail()
    {
        CocktailsStorage.instance.AddCocktail(_type);
        Destroy(gameObject);
    }

}