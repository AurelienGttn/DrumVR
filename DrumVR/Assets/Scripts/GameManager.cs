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
    [SerializeField] private GameObject PauseMenuCanvas;

    // Action linked to the controllers menu button
    public SteamVR_Action_Boolean openMenuAction;
    public enum GameContext { MainMenu, FreeMode, MemoryMode, RhythmMode, Menu };
    public static GameContext gc;
    public static GameContext previousContext; // Used to resume game after menu was opened
    private SequenceManager sqManager;
    private TextMeshProUGUI modeText;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI scoreText;


    void Start()
    {
        modeText = GameObject.Find("ModeText").GetComponent<TextMeshProUGUI>();
        titleText = GameObject.Find("CongratsText").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        sqManager = FindObjectOfType<SequenceManager>();
        sqManager.enabled = false;
        gc = GameContext.MainMenu;
        openMainMenu();
        
        // Fade from black
        SteamVR_Fade.Start(Color.black, 0f);
        SteamVR_Fade.Start(Color.clear, 8f);

        // Clear congrats label
        // Show game title
        // Menu options

    }

    private void Update()
    {
        if (openMenuAction.GetStateDown(SteamVR_Input_Sources.Any))
        {
            if (gc == GameContext.MainMenu) return;
            if (gc == GameContext.Menu)
            {
                // Resume game
                gc = previousContext;
                closeMenu();
            } else
            {
                previousContext = gc;
                gc = GameContext.Menu;
                openMenu(); // Doesn't work from "free mode"
            }
        }
    }

    private void openMainMenu()
    {


        modeText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        titleText.gameObject.SetActive(true);
        titleText.text = "Drums VR";

        PauseMenuCanvas.SetActive(false);
        foreach (Canvas cv in menuCanvas)
        {
            cv.gameObject.SetActive(true);
        }

        ShowParticlesOnMenuDrums(true);
    }

    private void openMenu()
    {
        PauseMenuCanvas.SetActive(true);
        ShowParticlesOnMenuDrums(true);

    }

    // Method to dispatch to the action associated with the drum depending on the context
    public void CheckPartHit(Drumpart partHit)
    {
        switch(gc)
        {
            case GameContext.MainMenu:
                CheckPartHitMainMenu(partHit);
                break;
            case GameContext.Menu:
                CheckPartHitPauseMenu(partHit);
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
                // Rhythm mode
                gc = GameContext.RhythmMode;
                sqManager.Init();
                closeMainMenu();
                break;

            case "DKFYB_Hi-hat":
                // Free mode
                gc = GameContext.FreeMode;
                closeMainMenu();
                break;

            case "DKFYB_Crash":
                // Memory mode
                gc = GameContext.MemoryMode;
                closeMainMenu();
                sqManager.Init();
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

    private void closeMainMenu()
    {
        foreach (Canvas cv in menuCanvas)
        {
            cv.gameObject.SetActive(false);
        }
        ShowParticlesOnMenuDrums(false);
        sqManager.enabled = true;

        switch (GameManager.gc)
        {
            case GameManager.GameContext.FreeMode:
                modeText.text = "Free mode";
                break;
            case GameManager.GameContext.MemoryMode:
                modeText.text = "Memory mode";
                break;
            case GameManager.GameContext.RhythmMode:
                modeText.text = "Rhythm mode";
                break;
            case GameManager.GameContext.MainMenu:
                modeText.text = "Main menu";
                break;
            case GameManager.GameContext.Menu:
                modeText.text = "Menu";
                break;

        }
        scoreText.gameObject.SetActive(true);
        modeText.gameObject.SetActive(true);
    }

    private void CheckPartHitPauseMenu(Drumpart partHit)
    {
        string partName = partHit.transform.parent.name;
        switch (partName)
        {
            case "DKFYB_Snare_drum":
                // New Sequence
                sqManager.CreateRandomSequence(sqManager.sequenceLength); // Fix : This resets the score
                closeMenu();
                break;

            case "DKFYB_Hi-hat":
                // Resume
                gc = previousContext;
                closeMenu();
                break;

            case "DKFYB_Crash":
                // Main menu
                gc = GameContext.MainMenu;
                closeMenu();
                openMainMenu();
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

    private void closeMenu()
    {

        ShowParticlesOnMenuDrums(false);
        PauseMenuCanvas.SetActive(false);
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
