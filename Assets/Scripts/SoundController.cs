using UnityEngine;

public class SoundController : MonoBehaviour
{

    public static SoundController Instance;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayHitSound()
    {
        //todo
    }
    
}