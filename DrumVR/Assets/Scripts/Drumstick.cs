using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Drumstick : MonoBehaviour
{
    [SerializeField] private GameObject rightHand, leftHand;
    private Vector3 velocityRH, velocityLH;

    [SerializeField] private AudioClip clip;

    private bool right = false, left = false;
    private bool triggered = false;
    private bool wrongDirection = false;

    private void Start()
    {
        if (name == "Right Drumstick")
            right = true;
        else
            left = true;
    }

    private void Update()
    {
        velocityLH = leftHand.GetComponent<VelocityEstimator>().GetAngularVelocityEstimate();
        velocityRH = rightHand.GetComponent<VelocityEstimator>().GetAngularVelocityEstimate();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Drumpart")) {
        //    if ((right && velocityRH.y <= 0) || 
        //        (left && velocityLH.y <= 0))
        //    {
        //        other.GetComponent<AudioSource>().Play();
        //    }
        //}

        // Make sure the stick exited the collision box
        if (!triggered && !wrongDirection)
        {
            // Check if the stick hit the bottom of the drum piece first
            if (other.name == "DirectionCheck")
            {
                wrongDirection = true;
            }

            // Play the sound of the drum piece if it hit the top first
            else if (other.CompareTag("Drumpart"))
            {
                other.GetComponent<AudioSource>().PlayOneShot(clip);
                //other.GetComponent<AudioSource>().Play();
                triggered = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset the direction check on exit
        if (other.name == "DirectionCheck")
        {
            wrongDirection = false;
        }

        // Allow the stick to trigger another drum piece
        if (other.CompareTag("Drumpart"))
        {
            triggered = false;
        }
    }
}
