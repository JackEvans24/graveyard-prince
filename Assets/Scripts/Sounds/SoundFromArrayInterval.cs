using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundFromArrayInterval : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private Vector2 waitBounds;

    private AudioSource audioSource;

    private float currentWaitTarget, waitElapsed;

    private void Awake()
    {
        this.audioSource = GetComponent<AudioSource>();

        this.ResetWaitValues();
    }

    void Update()
    {
        if (waitElapsed >= currentWaitTarget)
        {
            this.PlayAudio();
            this.ResetWaitValues();
        }
        else
        {
            waitElapsed += Time.deltaTime;
        }
    }

    private void ResetWaitValues()
    {
        waitElapsed = 0;
        currentWaitTarget = Random.Range(waitBounds.x, waitBounds.y);
    }

    private void PlayAudio()
    {
        var clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
    }
}
