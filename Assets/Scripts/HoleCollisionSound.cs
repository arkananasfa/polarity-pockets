using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HoleCollisionSound : MonoBehaviour
{
    [SerializeField] private AudioClip collisionSound; // Звук для воспроизведения
    [SerializeField] private float volume = 1.0f; // Громкость звука

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collisionSound != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(collisionSound, volume);
        }
    }
}