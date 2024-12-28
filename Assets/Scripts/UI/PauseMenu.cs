using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;

    private bool _isPaused;

    private void Update()
    {
        // Открыть/закрыть меню паузы при нажатии на ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        // Продолжить игру
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Возвращаем время в нормальный ход
        _isPaused = false;
    }

    private void Pause()
    {
        // Поставить на паузу
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Останавливаем время
        _isPaused = true;
    }

    public void RestartScene()
    {
        // Перезапустить текущую сцену
        Time.timeScale = 1f; // Возвращаем время перед перезапуском
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        // Выйти из игры
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void OpenSettings()
    {
        // Здесь можно открыть меню настроек (добавьте свою реализацию)
        Debug.Log("Opening settings...");
    }
}