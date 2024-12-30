using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuffPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject panel; // Панель с баффами
    [SerializeField] private CanvasGroup canvasGroup; // CanvasGroup для анимации
    [SerializeField] private Button toggleButton; // Кнопка для открытия/закрытия панели
    [SerializeField] private List<Button> buffButtons; // Кнопки баффов
    [SerializeField] private List<CocktailType> cocktailTypes; // Типы коктейлей в том же порядке, что и кнопки
    [SerializeField] private float animationDuration = 0.3f; // Длительность анимации
    [SerializeField] private Color activeColor = Color.white; // Цвет активной кнопки
    [SerializeField] private Color inactiveColor = Color.gray; // Цвет кнопки, если баффа нет

    private Coroutine animationCoroutine;
    private bool isPanelOpen = false; 
    
    private List<Cocktail> usedCocktails = new();

    private void OnEnable()
    {
        GlobalEventManager.OnBackToGameButtonClicked += UpdateBuffPanel;
        GlobalEventManager.OnGoToShopButtonClicked += EndEffects;
    }

    private void OnDisable()
    {
        GlobalEventManager.OnBackToGameButtonClicked -= UpdateBuffPanel;
        GlobalEventManager.OnGoToShopButtonClicked -= EndEffects;
    }

    private void Start()
    {
        toggleButton.onClick.AddListener(TogglePanel);
        UpdateBuffPanel();
        HidePanelInstantly();
        for (int i = 0; i < cocktailTypes.Count; i++)
        {
            int locali = i;
            buffButtons[locali].onClick.AddListener(() => UseCocktailBuff(cocktailTypes[locali]));
        }
    }

    private void Update()
    {
        // Закрываем панель, если клик был вне её области
        if (Input.GetMouseButtonDown(0) && isPanelOpen && !IsPointerOverUI())
        {
            ClosePanel();
        }
    }

    private void UseCocktailBuff(CocktailType cocktailType)
    {
        if (!CocktailsStorage.instance.cocktails.ContainsKey(cocktailType))
        {
            return;
        }
        if (CocktailsStorage.instance.cocktails[cocktailType] > 0)
        {
            Cocktail cocktail = CocktailFactory.GetCocktail(cocktailType);
            usedCocktails.Add(cocktail);
            cocktail.Use();
            CocktailsStorage.instance.cocktails[cocktailType]--;
            UpdateBuffPanel();
        }
    }

    private void EndEffects()
    {
        usedCocktails.ForEach(c => c.End());
        usedCocktails.Clear();
    }

    // Обновление UI панели
    public void UpdateBuffPanel()
    {
        var cocktails = CocktailsStorage.instance.cocktails;

        for (int i = 0; i < cocktailTypes.Count; i++)
        {
            CocktailType type = cocktailTypes[i];
            Button button = buffButtons[i];
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>(); // Текст на кнопке

            // Проверяем, есть ли коктейль в списке
            if (cocktails.ContainsKey(type) && cocktails[type] > 0)
            {
                button.image.color = activeColor; // Активный цвет
                buttonText.text = cocktails[type].ToString(); // Обновляем количество
            }
            else
            {
                button.image.color = inactiveColor; // Серый цвет
                buttonText.text = "0"; // Если баффов нет
            }
        }
    }

    // Открытие/закрытие панели по кнопке
    public void TogglePanel()
    {
        if (isPanelOpen)
        {
            ClosePanel();
        }
        else
        {
            OpenPanel();
        }
    }

    // Плавное открытие панели
    public void OpenPanel()
    {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(AnimatePanel(1, animationDuration));
        isPanelOpen = true;
        GameManager.IsBlocked = true;
    }

    // Плавное закрытие панели
    public void ClosePanel()
    {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(AnimatePanel(0, animationDuration));
        isPanelOpen = false;
        GameManager.IsBlocked = false;
    }

    // Мгновенно скрыть панель
    private void HidePanelInstantly()
    {
        panel.SetActive(false);
        canvasGroup.alpha = 0;
    }

    // Анимация плавного появления и исчезновения
    private IEnumerator AnimatePanel(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        if (targetAlpha > 0) panel.SetActive(true); // Включаем панель перед появлением

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration); // Плавное изменение прозрачности
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        if (targetAlpha == 0) panel.SetActive(false); // Выключаем панель после исчезновения
    }

    // Проверка, находится ли указатель над UI
    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
