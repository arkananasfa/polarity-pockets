using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollisionSound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> collisionSounds; // Список звуков для воспроизведения
    [SerializeField] private float minVolume = 0.1f; // Минимальная громкость
    [SerializeField] private float maxVolume = 1.0f; // Максимальная громкость
    [SerializeField] private float minImpactForce = 0.1f; // Минимальная сила удара для звука
    [SerializeField] private float maxImpactForce = 10f; // Максимальная сила удара для громкости
    [SerializeField] private float pitchVariation = 0.3f; // Максимальное отклонение pitch от базового значения (например, ±0.2)
    [SerializeField] private bool onlyPlayOnHigherMass = true; // Играть звук только на объекте с большей массой

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Рассчитываем силу столкновения
        float impactForce = collision.relativeVelocity.magnitude;

        Debug.Log(impactForce);
        // Если сила меньше минимальной, звук не воспроизводится
        if (impactForce < minImpactForce) return;

        // Рассчитываем громкость на основе силы удара
        float volume = Mathf.Lerp(minVolume, maxVolume, impactForce / maxImpactForce);
        volume = Mathf.Clamp(volume, minVolume, maxVolume);

        // Выбираем случайный звук
        AudioClip randomClip = collisionSounds[Random.Range(0, collisionSounds.Count)];

        // Сохраняем оригинальный pitch
        float originalPitch = audioSource.pitch;

        // Устанавливаем случайный pitch в диапазоне ±pitchVariation
        audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);

        // Воспроизводим звук
        audioSource.PlayOneShot(randomClip, volume);

        // Возвращаем pitch к оригинальному значению
        audioSource.pitch = originalPitch;
    }
}
