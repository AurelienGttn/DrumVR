using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drumstick : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit trigger");
        if (other.CompareTag("Drumset")){
            other.GetComponent<AudioSource>().Play();
        }
    }
}
