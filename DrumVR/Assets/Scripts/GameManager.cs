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

    public enum GameContext { MainMenu, FreeMode, MemoryMode, RythmMode, Menu };
    public static GameContext gc;
    private SequenceManager sqManager;

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

    // Method to dispatch to the action associated with the drum depending on the context
    public void CheckPartHit(Drumpart partHit)
    {
        switch(gc)
        {
            case GameContext.MainMenu:
                //MainMenuManager.checkPartHit(partHit);
                CheckPartHitMainMenu(partHit);
                break;
            default:
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
                // Rythm mode
                break;

            case "DKFYB_Hi-hat":
                // Free mode
                break;

            case "DKFYB_Crash":
                // Memory mode
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
        List<string> menuDrums = new List<string>() { "DKFYB_Snare_drum", "DKFYB_Hi-hat", "DKFYB_Crash", "DKFYB_Ride", "DKFYB_Floor_tom" };

        foreach (Drumpart dp in drumParts)
        {
            if (menuDrums.Contains(dp.transform.parent.name))
            {
                dp.targetIndicator.gameObject.SetActive(show);
            }

        }
    }

    private GameObject GetParticleSystem(Drumpart drumpart)
    {
        return drumParts[ArrayUtility.IndexOf(drumParts, drumpart)].targetIndicator.gameObject;
    }
}
