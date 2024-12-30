using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip firstTrack; // Первый трек
    [SerializeField] private AudioClip loopingTrack; // Второй трек, который будет зацикливаться
    [SerializeField] private AudioClip shopMusic; // Музыка магазина

    private AudioSource audioSource;
    private bool isInShop = false; // Флаг для проверки, в магазине ли игрок
    private float globalVolume = 1f; // Глобальная громкость музыки

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false; // Первоначально отключаем зацикливание

        // Загрузка глобальной громкости музыки
        globalVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);

        // Подписка на глобальные события
        GlobalEventManager.OnGoToShopButtonClicked += ChangeShopMusic;
        GlobalEventManager.OnBackToGameButtonClicked += PlayFirstTrack;

        PlayFirstTrack();
    }

    private void Update()
    {
        // Проверяем, если трек закончился, и мы не в магазине, запускаем следующий трек
        if (!isInShop && !audioSource.isPlaying && audioSource.clip == firstTrack)
        {
            PlayLoopingTrack();
        }

        // Применяем глобальную громкость в реальном времени
        ApplyGlobalVolume();
    }

    private void ApplyGlobalVolume()
    {
        audioSource.volume = globalVolume * (isInShop ? 0.4f : 0.2f); // Глобальная громкость * локальная
    }

    private void ChangeShopMusic()
    {
        isInShop = true; // Устанавливаем флаг
        audioSource.Stop(); // Останавливаем текущую музыку
        audioSource.clip = shopMusic; // Меняем трек на магазинный
        audioSource.loop = true; // Зацикливаем
        audioSource.Play(); // Воспроизводим
    }

    private void PlayFirstTrack()
    {
        isInShop = false; // Сбрасываем флаг
        audioSource.Stop(); // Останавливаем текущую музыку
        if (firstTrack != null)
        {
            audioSource.clip = firstTrack;
            audioSource.loop = false; // Первоначально без зацикливания
            audioSource.Play();
        }
        else
        {
        }
    }

    private void PlayLoopingTrack()
    {
        if (loopingTrack != null)
        {
            audioSource.clip = loopingTrack;
            audioSource.loop = true; // Включаем зацикливание
            audioSource.Play();
        }
        else
        {
        }
    }

    public void SetGlobalVolume(float volume)
    {
        globalVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume); // Сохраняем глобальную громкость
    }

    private void OnDestroy()
    {
        // Отписываемся от событий, чтобы избежать ошибок
        GlobalEventManager.OnGoToShopButtonClicked -= ChangeShopMusic;
        GlobalEventManager.OnBackToGameButtonClicked -= PlayFirstTrack;
    }
}
