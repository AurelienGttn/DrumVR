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
    private int currentIndex;
    private int sequenceLength;
    private int mistakes;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)){
            CreateRandomSequence(1);
        }
        if (nextPartToHit != null){
            if (Input.GetKeyDown(KeyCode.B))
            {
                CheckPartHit(nextPartToHit);
            }
        }
    }

    private void CreateRandomSequence(int size)
    {
        // Remove particles from former sequence's next drum part to hit
        if (nextPartToHit != null)
        {
            GetParticleSystem(nextPartToHit).SetActive(false);
            nextPartToHit = null;
        }
        // Clear sequence and reset index
        randomSequence.Clear();
        currentIndex = 0;
        mistakes = 0;

        sequenceLength = size;
        for (int i = 0; i < sequenceLength; i++)
        {
            int randomPart = Random.Range(0, drumParts.Length);
            randomSequence.Add(drumParts[randomPart]);
        }

        nextPartToHit = randomSequence[currentIndex];
        GetParticleSystem(nextPartToHit).SetActive(true);
    }

    public void CheckPartHit(Drumpart partHit)
    {
        if (partHit == nextPartToHit)
        {
            Debug.Log("well done");
            playedSequence.Add(partHit);
            GetParticleSystem(partHit).SetActive(false);
            Debug.Log("particle to stop = " + GetParticleSystem(partHit).name);
            if(currentIndex < sequenceLength - 1) {
                nextPartToHit = randomSequence[currentIndex++];
                GetParticleSystem(nextPartToHit).SetActive(true);
            }
            else
            {
                Debug.Log("Congratulations! Only " + mistakes + " mistakes.");
                CreateRandomSequence(sequenceLength + 1);
            }
        }
        else
        {
            mistakes++;
            Debug.Log("failed");
        }
    }

    private GameObject GetParticleSystem(Drumpart drumpart)
    {
        return drumParts[ArrayUtility.IndexOf(drumParts, drumpart)].targetIndicator.gameObject;   
    }
}
