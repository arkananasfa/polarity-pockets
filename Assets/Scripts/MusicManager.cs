using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip firstTrack; // Первый трек
    [SerializeField] private AudioClip loopingTrack; // Второй трек, который будет зацикливаться
    [SerializeField] private AudioClip shopMusic; // Музыка магазина

    private AudioSource audioSource;
    private bool isInShop = false; // Флаг для проверки, в магазине ли игрок

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false; // Первоначально отключаем зацикливание
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
    }

    private void ChangeShopMusic()
    {
        isInShop = true; // Устанавливаем флаг
        audioSource.Stop(); // Останавливаем текущую музыку
        audioSource.clip = shopMusic; // Меняем трек на магазинный
        audioSource.loop = true; // Зацикливаем
        audioSource.volume = 0.4f; // Устанавливаем громкость
        audioSource.Play(); // Воспроизводим
    }

    private void PlayFirstTrack()
    {
        isInShop = false; // Сбрасываем флаг
        audioSource.Stop(); // Останавливаем текущую музыку
        audioSource.volume = 0.2f; // Устанавливаем громкость
        if (firstTrack != null)
        {
            audioSource.clip = firstTrack;
            audioSource.loop = false; // Первоначально без зацикливания
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("First track is not assigned!");
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
            Debug.LogWarning("Looping track is not assigned!");
        }
    }

    private void OnDestroy()
    {
        // Отписываемся от событий, чтобы избежать ошибок
        GlobalEventManager.OnGoToShopButtonClicked -= ChangeShopMusic;
        GlobalEventManager.OnBackToGameButtonClicked -= PlayFirstTrack;
    }
}
