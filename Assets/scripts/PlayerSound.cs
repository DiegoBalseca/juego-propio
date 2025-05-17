using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip footStepsAudio;

    private GroundsSensor groundsSensor;

    private ParticleSystem particleSystem;

    private Transform particlesTransform;

    private Vector3 particlesPosition;

    private bool alreadyPlaying = false;

    
    
    void Awake()
    {
        groundsSensor = GetComponentInChildren<GroundsSensor>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        particlesTransform = particleSystem.transform;
        particlesPosition = particlesTransform.localPosition;


    }

    
    void Start()
    {
        audioSource.loop = true;
        audioSource.clip = footStepsAudio;

    }

    void Update()
    {
        FootStepsSound();
    }

    void FootStepsSound()
    {
        if(groundsSensor.isGrounded && Input.GetAxisRaw("Horizontal") != 0 && !alreadyPlaying)
        {
            particlesTransform.SetParent(gameObject.transform);
            particlesTransform.localPosition = particlesPosition; 
            particlesTransform.rotation = transform.rotation;
            audioSource.Play();
            particleSystem.Play();
            alreadyPlaying = true;

        }
        else if(!groundsSensor.isGrounded | Input.GetAxisRaw("Horizontal") == 0)
        {
            particlesTransform.SetParent(null);
            audioSource.Stop();
            particleSystem.Stop();
            alreadyPlaying = false;
        }
    }
}
