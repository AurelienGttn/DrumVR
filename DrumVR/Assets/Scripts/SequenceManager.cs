using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using Valve.VR;

public class SequenceManager : MonoBehaviour
{
    public SteamVR_Action_Boolean grabAction;

    [SerializeField] private Drumpart[] drumParts;
    private TextMeshProUGUI congratsText;

    private List<Drumpart> randomSequence = new List<Drumpart>();

    public Drumpart nextPartToHit;
    private int currentIndex;
    private int sequenceLength = 1;
    private int mistakes;
    private bool sequenceEnded = false;


    private void Start()
    {
        congratsText = GameObject.Find("CongratsText").GetComponent<TextMeshProUGUI>();
        congratsText.gameObject.SetActive(false); 
    }
       

    // Only used for development
    private void Update()
    {
        if (grabAction.GetStateDown(SteamVR_Input_Sources.Any))
        {
            if (sequenceEnded)
                CreateRandomSequence(sequenceLength + 1);
            else
                CreateRandomSequence(sequenceLength);
            StartCoroutine(PlaySequence());
            
        }
        /*
        if (nextPartToHit != null){
            if (Input.GetAxis("Fire") > 0 || Input.GetAxis("Fire2") > 0)
            {
                CheckPartHit(nextPartToHit);
            }
        }*/
    }


    // Method to create a random sequence of parts to hit
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
        congratsText.gameObject.SetActive(false);
        sequenceEnded = false;

        sequenceLength = size;
        for (int i = 0; i < sequenceLength; i++)
        {
            int randomPart = Random.Range(0, drumParts.Length);
            randomSequence.Add(drumParts[randomPart]);
        }

        nextPartToHit = randomSequence[currentIndex];
    }


    // Method to check if the player hit the right drum part
    // and count the mistakes
    public void CheckPartHit(Drumpart partHit)
    {
        Debug.Log("checkPartHit");
        Debug.Log("part to hit = " + nextPartToHit.transform.parent.name);
        Debug.Log("part hit = " + partHit.transform.parent.name);
        if (partHit == nextPartToHit)
        {
            Debug.Log("current index = " + currentIndex);
            GetParticleSystem(partHit).SetActive(false);
            if(currentIndex < sequenceLength - 1) {
                nextPartToHit = randomSequence[currentIndex++];
            }
            else
            {
                sequenceEnded = true;
                switch (mistakes) {
                    case 0:
                        congratsText.text = "Well done!\nOnly " + mistakes + " mistakes";
                        break;
                    case 1:
                        congratsText.text = "Well done!\nOnly " + mistakes + " mistake";
                        break;
                    default:
                        congratsText.text = "Well done!\nOnly " + mistakes + " mistakes";
                        break;
                }
                congratsText.gameObject.SetActive(true);
                Debug.Log("Congratulations! Only " + mistakes + " mistakes.");
            }
        }
        else
        {
            Debug.Log("failed");
            mistakes++;
        }
    }


    private GameObject GetParticleSystem(Drumpart drumpart)
    {
        return drumParts[ArrayUtility.IndexOf(drumParts, drumpart)].targetIndicator.gameObject;   
    }


    // Method to play the full random sequence
    private IEnumerator PlaySequence()
    {
        GetParticleSystem(randomSequence[currentIndex]).SetActive(true);
        randomSequence[currentIndex].PlayDrumSound();

        yield return new WaitForSeconds(0.8f);

        GetParticleSystem(randomSequence[currentIndex]).SetActive(false);
        currentIndex++;

        yield return new WaitForSeconds(0.2f);

        if (currentIndex < randomSequence.Count)
        {
            StartCoroutine(PlaySequence());
        }
    }
}
