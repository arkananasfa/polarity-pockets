using System;

public static class GlobalEventManager
{
    public static event Action<int> OnWin;
    public static event Action<int> OnLose;
    public static event Action OnBallOnPocket;
    public static event Action OnGoToShopButtonClicked;
    public static event Action OnBackToGameButtonClicked;

    public static void Lose(int round)
    {
        OnLose?.Invoke(round);
    }

    public static void Win(int round)
    {
        OnWin?.Invoke(round);
    }

    public static void BallOnPocket()
    {
        OnBallOnPocket?.Invoke();
    }

    public static void GoToShopButton()
    {
        OnGoToShopButtonClicked?.Invoke();
    }

    public static void BackToGame()
    {
        OnBackToGameButtonClicked?.Invoke();
    }

    public static void ClearAllSubscribers()
    {
        OnLose = null;
        OnWin = null;
    }
}