using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drumset : MonoBehaviour
{

    private AudioSource source;
    private AudioClip sound;
    [SerializeField] private DirectionCheck directionCheck;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        sound = GetComponent<AudioSource>().clip;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (directionCheck == null || !directionCheck.GetComponent<DirectionCheck>().wrongDirection)
        {
            source.PlayOneShot(sound);
            Debug.Log(this.transform.parent.name);
        }
    }
}
