using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    [SerializeField] private GameObject panelsBack;
    
    [Header("Lose Menu")]
    [SerializeField] private RectTransform loseMenu;
    [SerializeField] private CanvasGroup loseMenuCanvasGroup;
    [SerializeField] private Button loseMenuButton;
    [SerializeField] private TextMeshProUGUI roundsBeatenText;
    
    [Header("Win Menu")]
    [SerializeField] private RectTransform winMenu;
    [SerializeField] private CanvasGroup winMenuCanvasGroup;
    [SerializeField] private Button winMenuButton;
    [SerializeField] private TextMeshProUGUI roundText;

    private void OnEnable()
    {
        GlobalEventManager.OnLose += ShowLoseMenu;
        GlobalEventManager.OnLose += ShowWinMenu;
        loseMenuButton.onClick.AddListener(RestartGame);
        winMenuButton.onClick.AddListener(GoToShop);
    }

    public void ShowWinMenu(int rounds)
    {
        ShowPanelsBack(true);
        
        winMenu.sizeDelta = Vector2.zero;
        winMenu.DOSizeDelta(new Vector2(400, 260), 0.6f);
        
        winMenuCanvasGroup.alpha = 0f;
        winMenuCanvasGroup.DOFade(1f, 0.6f);
        
        roundText.text = $"Rounds beaten: {rounds}";
    }

    public void ShowLoseMenu(int rounds)
    {
        ShowPanelsBack(true);
        
        loseMenu.sizeDelta = Vector2.zero;
        loseMenu.DOSizeDelta(new Vector2(400, 260), 0.6f);
        
        loseMenuCanvasGroup.alpha = 0f;
        loseMenuCanvasGroup.DOFade(1f, 0.6f);
        
        roundsBeatenText.text = $"Rounds beaten: {rounds}";
    }

    private void ShowPanelsBack(bool isActive)
    {
        panelsBack.SetActive(isActive);
    }

    private void GoToShop()
    {
        ShowPanelsBack(false);
        GlobalEventManager.GoToShopButton();
    }

    private void RestartGame()
    {
        ClearAllStaticData();
        SceneManager.LoadScene(0);
    }

    private void ClearAllStaticData()
    {
        GlobalEventManager.ClearAllSubscribers();
        GameManager.currentRound = new ObservableValue<int>(1);
        ResourcesManager.Tries = new ObservableValue<int>(10);
        ResourcesManager.Money = new ObservableValue<int>(0);
        //todo
    }
    
}