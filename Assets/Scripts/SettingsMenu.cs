using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider musicVolumeSlider;
    public Slider collisionVolumeSlider;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer; // Один микшер для всех групп

    private const string MusicVolumeKey = "Volume (of Music)";
    private const string CollisionVolumeKey = "Volume (of CollisionSounds)";

    void Start()
    {
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        float savedCollisionVolume = PlayerPrefs.GetFloat(CollisionVolumeKey, 1f);

        // Установка значений ползунков
        musicVolumeSlider.value = savedMusicVolume;
        collisionVolumeSlider.value = savedCollisionVolume;

        // Применение громкости
        SetMusicVolume(savedMusicVolume);
        SetCollisionVolume(savedCollisionVolume);

        // Подписка на изменения ползунков
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        collisionVolumeSlider.onValueChanged.AddListener(SetCollisionVolume);
    }

    public void SetMusicVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
        audioMixer.SetFloat("Volume (of Music)", dB); // Устанавливаем громкость для группы "Music"
        PlayerPrefs.SetFloat(MusicVolumeKey, volume); // Сохраняем настройки
    }

    public void SetCollisionVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
        audioMixer.SetFloat("Volume (of CollisionSounds)", dB); // Устанавливаем громкость для группы "CollisionSound"
        PlayerPrefs.SetFloat(CollisionVolumeKey, volume); // Сохраняем настройки
    }

    private void OnDestroy()
    {
        // Отписка от событий, чтобы избежать утечек
        musicVolumeSlider.onValueChanged.RemoveListener(SetMusicVolume);
        collisionVolumeSlider.onValueChanged.RemoveListener(SetCollisionVolume);
    }
}