using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drumstick : MonoBehaviour
{
    bool triggered = false;
    bool wrongDirection = false;
    private void OnTriggerEnter(Collider other)
    {
        // Make sure the stick exited the collision box
        if (!triggered)
        {
            // Check if the stick hit the top of the drum piece first
            if (other.name == "DirectionCheck")
            {
                Debug.Log("Hit bottom of: " + other.transform.parent.name);
                wrongDirection = true;
            }

            // Play the sound of the drum piece if it hit the top first
            else if (other.CompareTag("Drumset") && !wrongDirection)
            {
                Debug.Log("Play sound: " + other.transform.parent.name);
                other.GetComponent<AudioSource>().Play();
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
        if (other.CompareTag("Drumset"))
        {
            triggered = false;
        }
    }
}
