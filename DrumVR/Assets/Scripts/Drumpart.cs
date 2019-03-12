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
            // Use PlayOneShot to be able to allow the sound to overlap
            source.PlayOneShot(sound);

            // Check if this was the right part to hit
            sqManager.CheckPartHit(this);
        }
    }
}
