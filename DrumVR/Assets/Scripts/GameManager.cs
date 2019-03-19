using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using Valve.VR;

public class GameManager : MonoBehaviour
{


    [SerializeField] private Drumpart[] drumParts;
    [SerializeField] private Canvas[] menuCanvas;
    private List<Drumpart> randomSequence = new List<Drumpart>();

    public enum GameContext { MainMenu, FreeMode, MemoryMode, RythmMode, Menu };
    public GameContext gc;
    private SequenceManager sqManager;

    // Start is called before the first frame update
    void Start()
    {
        sqManager = FindObjectOfType<SequenceManager>();
        gc = GameContext.MainMenu;

        ShowParticlesOnMenuDrums(true);
        
        // Fade from black
        SteamVR_Fade.Start(Color.black, 0f);
        SteamVR_Fade.Start(Color.clear, 8f);

        // Clear congrats label
        // Show game title
        // Menu options

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method to dispatch to the action associated with the drum depending on the context
    public void CheckPartHit(Drumpart partHit)
    {
        Debug.Log("CheckPartHit in context " + gc);
        switch(gc)
        {
            case GameContext.MainMenu:
                //MainMenuManager.checkPartHit(partHit);
                CheckPartHitMainMenu(partHit);
                break;
            case GameContext.MemoryMode:
                sqManager.CheckPartHit(partHit);
                break;
            
        };
    }

    private void CheckPartHitMainMenu(Drumpart partHit)
    {
        string partName = partHit.transform.parent.name;
        switch(partName)
        {
            case "DKFYB_Snare_drum":
                Debug.Log("Rythm mode");
                // Rythm mode
                break;

            case "DKFYB_Hi-hat":
                Debug.Log("Free mode");
                // Free mode
                break;

            case "DKFYB_Crash":
                // Memory mode
                Debug.Log("Memory mode");
                foreach (Canvas cv in menuCanvas)
                {
                    cv.gameObject.SetActive(false);
                }
                ShowParticlesOnMenuDrums(false);
                gc = GameContext.MemoryMode;
                sqManager.enabled = true;
                break;

            case "DKFYB_Ride":
                // Settings
                break;

            case "DKFYB_Floor_tom":
                // Exit
                Application.Quit();
                break;
        }
    }

    private void ShowParticlesOnMenuDrums(bool show)
    {
        Debug.Log("Show particles on menu " + show);
        List<string> menuDrums = new List<string>() { "DKFYB_Snare_drum", "DKFYB_Hi-hat", "DKFYB_Crash", "DKFYB_Ride", "DKFYB_Floor_tom" };

        foreach (Drumpart dp in drumParts)
        {
            Debug.Log(dp.transform.parent.name);
            if (menuDrums.Contains(dp.transform.parent.name))
            {
                Debug.Log("YES, should light up " + dp.transform.parent.name);
                dp.targetIndicator.gameObject.SetActive(show);
            }

        }
    }

    private GameObject GetParticleSystem(Drumpart drumpart)
    {
        return drumParts[ArrayUtility.IndexOf(drumParts, drumpart)].targetIndicator.gameObject;
    }
}
