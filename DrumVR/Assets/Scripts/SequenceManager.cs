using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using Valve.VR;

public class SequenceManager : MonoBehaviour
{
    // Action linked to the controllers trigger
    public SteamVR_Action_Boolean grabAction;

    // Keep track of all the parts we can hit
    [SerializeField] private Drumpart[] drumParts;

    // List to keep track of a randomly created sequence of drum parts
    private List<Drumpart> randomSequence = new List<Drumpart>();
    // List of the delays between each hit for rythm mode
    private List<float> delaySequence = new List<float>();
    public Drumpart nextPartToHit;
    private int currentIndex;
    // Time at which previous part was hit
    private float lastHit;
    // Tolerance interval for rythm mode
    [SerializeField] private float timeTolerance = 0.5f;
    // Length of the current random sequence
    public int sequenceLength = 2;
    // Count of the mistakes the player has made so far
    private int mistakes;
    private int totalMistakes;
    private int score;

    private TextMeshProUGUI congratsText;
    private TextMeshProUGUI scoreText;
    private bool sequenceEnded = false;

    private void Start()
    {
        congratsText    = GameObject.Find("CongratsText").GetComponent<TextMeshProUGUI>();
        scoreText       = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();

    }


    private void Update()
    {
        if (grabAction.GetStateDown(SteamVR_Input_Sources.Any))
        {
            if (sequenceEnded)
                CreateRandomSequence(sequenceLength + 1);
            else
                CreateRandomSequence(sequenceLength);
        }
    }

    public void StartPlaying()
    {
        score = 0;
        CreateRandomSequence(sequenceLength);
    }


    // Method to create a random sequence of parts to hit
    public void CreateRandomSequence(int size)
    {
        // Remove particles from former sequence's next drum part to hit
        if (nextPartToHit != null)
        {
            GetParticleSystem(nextPartToHit).SetActive(false);
            nextPartToHit = null;
        }
        // Clear sequence and reset index
        randomSequence.Clear();
        delaySequence.Clear();
        currentIndex = 0;
        mistakes = 0;
        congratsText.gameObject.SetActive(false);
        sequenceEnded = false;

        int randomPart = 0;

        sequenceLength = size;
        for (int i = 0; i < sequenceLength; i++)
        {
            if (GameManager.gc == GameManager.GameContext.MemoryMode)
                randomPart = Random.Range(0, drumParts.Length);
            else if (GameManager.gc == GameManager.GameContext.RythmMode)
                randomPart = 0;

            randomSequence.Add(drumParts[randomPart]);
        }

        nextPartToHit = randomSequence[currentIndex];

        if (GameManager.gc == GameManager.GameContext.MemoryMode)
            StartCoroutine(PlaySequence());
        else if (GameManager.gc == GameManager.GameContext.RythmMode)
            StartCoroutine(PlaySequenceDelay());
    }


    // Method to check if the player hit the right drum part
    // and count the mistakes
    public void CheckPartHit(Drumpart partHit)
    {
        bool rightMove = false;
        switch (GameManager.gc)
        {
            case GameManager.GameContext.MemoryMode:
                if (partHit == nextPartToHit)
                    rightMove = true;
                break;

            case GameManager.GameContext.RythmMode:
                // Check if the player hit the drum part in the tolerance interval
                Debug.Log("Delay : " + (Time.time - lastHit));
                Debug.Log(delaySequence[currentIndex]);
                if (currentIndex == 0 ||
                    (Time.time - lastHit > delaySequence[currentIndex] - timeTolerance)
                    && Time.time - lastHit < delaySequence[currentIndex] + timeTolerance)
                {
                    rightMove = true;
                    Debug.Log("Right move");
                }
                lastHit = Time.time;
                break;

            case GameManager.GameContext.FreeMode:
                score++;
                scoreText.text = "Score : " + score;
                break;
            default:
                break;
        };

        if (rightMove)
        {
            GetParticleSystem(partHit).SetActive(false);
            if (currentIndex < sequenceLength - 1)
            {
                nextPartToHit = randomSequence[++currentIndex];
                Debug.Log("Going to next index" + currentIndex);
            }
            else
            {
                sequenceEnded = true;
                switch (mistakes)
                {
                    case 0:
                        congratsText.text = "Well done!\nPerfect score!";
                        break;
                    case 1:
                        congratsText.text = "Well done!\nOnly " + mistakes + " mistake";
                        break;
                    default:
                        congratsText.text = "Well done!\nOnly " + mistakes + " mistakes";
                        break;
                }
                congratsText.gameObject.SetActive(true);

                if (sequenceLength * 2 - mistakes > 0) // We don't want to add negative scores
                    score += sequenceLength * 2 - mistakes;

                totalMistakes += mistakes;
                scoreText.text = "Score : " + score + " Mistakes : " + totalMistakes;
            }
        }
        else
        {
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
        else
        {
            currentIndex = 0;
        }
    }


    // Method to play the full random sequence
    // with a random delay
    private IEnumerator PlaySequenceDelay()
    {
        float delay = Random.Range(0.5f, 3f);
        GetParticleSystem(randomSequence[currentIndex]).SetActive(true);
        randomSequence[currentIndex].PlayDrumSound();
        if (currentIndex == 0)
        {
            delaySequence.Add(0);
            delaySequence.Add(delay);
        }
        else
            delaySequence.Add(delay);

        yield return new WaitForSeconds(delay);

        GetParticleSystem(randomSequence[currentIndex]).SetActive(false);
        currentIndex++;

        if (currentIndex < randomSequence.Count)
        {
            StartCoroutine(PlaySequenceDelay());
        }
        else
        {
            currentIndex = 0;
            foreach(float del in delaySequence)
            {
                Debug.Log(del);
            }
        }
    }
}
