using UnityEngine;

public class MusicaNivel : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); 
        audioSource = GetComponent<AudioSource>();
    }

    public void DetenerMusica()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
