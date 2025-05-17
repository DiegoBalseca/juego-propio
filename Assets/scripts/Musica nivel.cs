using UnityEngine;

public class MusicaNivel : MonoBehaviour
{
    void Start()
    {
        GetComponent<AudioSource>().Play();
    }

    public void StopMusic()
    {
        GetComponent<AudioSource>().Stop();
    }
}
