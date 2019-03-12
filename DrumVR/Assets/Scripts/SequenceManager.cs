using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{

    [SerializeField] private Drumpart[] drumParts;

    private List<Drumpart> randomSequence = new List<Drumpart>();
    private List<Drumpart> playedSequence = new List<Drumpart>();

    public Drumpart nextPartToHit;
    private int currentIndex = 0;

    private int mistakes = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)){
            CreateRandomSequence(1);
        }
    }

    private void CreateRandomSequence(int size)
    {
        randomSequence.Clear();
        for (int i = 0; i < size; i++)
        {
            int randomPart = Random.Range(0, drumParts.Length);
            randomSequence.Add(drumParts[randomPart]);
        }
        nextPartToHit = randomSequence[currentIndex];
        drumParts[ArrayUtility.IndexOf(drumParts, nextPartToHit)].targetIndicator.Play();
    }

    public void CheckPartHit(Drumpart partHit)
    {
        if (partHit == nextPartToHit)
        {
            playedSequence.Add(partHit);
            drumParts[ArrayUtility.IndexOf(drumParts, nextPartToHit)].targetIndicator.Stop();
            nextPartToHit = randomSequence[currentIndex++];
            drumParts[ArrayUtility.IndexOf(drumParts, nextPartToHit)].targetIndicator.Play();
        }
        else
        {
            mistakes++;
        }
    }
}
