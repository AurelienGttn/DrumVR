using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drumstick : MonoBehaviour
{
    bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            if (other.CompareTag("Drumset"))
            {
                other.GetComponent<AudioSource>().Play();
                triggered = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        triggered = false;
    }
}
