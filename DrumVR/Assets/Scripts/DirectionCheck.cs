using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionCheck : MonoBehaviour
{

    [HideInInspector] public bool wrongDirection;

    void Start()
    {
        wrongDirection = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        wrongDirection = true;
    }

    private void OnTriggerExit(Collider other)
    {
        wrongDirection = false;
    }

}
