﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drumpart : MonoBehaviour
{
    private SequenceManager sqManager;

    private AudioSource source;
    private AudioClip sound;

    // Checks if the player hits the part in the right direction
    [SerializeField] private DirectionCheck directionCheck;

    // FX to show which part to hit
    public ParticleSystem targetIndicator;

    void Start()
    {
        sqManager = FindObjectOfType<SequenceManager>();

        source = GetComponent<AudioSource>();
        sound = GetComponent<AudioSource>().clip;

        // Don't show the FX at first
        targetIndicator.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (directionCheck == null || !directionCheck.GetComponent<DirectionCheck>().wrongDirection)
        {
            PlayDrumSound();

            // Check if this was the right part to hit
            sqManager.CheckPartHit(this);
        }
    }

    // Method to play the drum sound needs to be public to be called
    // from the Sequence Manager
    public void PlayDrumSound()
    {
        // Use PlayOneShot to allow the sounds to overlap
        source.PlayOneShot(sound);
    }
}
