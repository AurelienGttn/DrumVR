using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;



public class ActionsTest : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // 1
    public SteamVR_Action_Boolean grabAction; // 3


    public bool GetGrab() // 2
    {
        return grabAction.GetState(handType);
    }

    private void Update()
    {
        if (GetGrab())
            Debug.Log("grab " + handType);
    }
}
