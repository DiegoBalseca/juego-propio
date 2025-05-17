using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip coinSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayCoinSound();
            Destroy(gameObject); 
        }
    }

    private void PlayCoinSound()
    {
        if (coinSound != null)
        {
            GameObject tempGO = new GameObject("TempCoinSound");
            AudioSource audioSource = tempGO.AddComponent<AudioSource>();
            audioSource.clip = coinSound;
            audioSource.volume = 5f; 
            audioSource.Play();
            Destroy(tempGO, coinSound.length); 
        }
    }
}

