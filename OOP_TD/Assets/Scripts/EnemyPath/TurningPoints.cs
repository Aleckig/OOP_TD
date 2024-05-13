using System.Collections;
using System.Collections.Generic;
using Michsky.UI.Heat;
using UnityEngine;

public class TurningPoints : MonoBehaviour
{
    public GameObject[] allDisablePaths;
    public AbilityManager abilityManager;

    public void BlockPath() //Enables the PathBlocker script on decal projectors, so that player can click which path to disable
    {
        abilityManager.DecreasePathAbilityCount();
        for (int i = 0; i < allDisablePaths.Length; i++)
        {
            allDisablePaths[i].GetComponent<PathBlocker>().enabled = true;
        }
    }

    //public void DisableBlocking() //Disables the PathBlocker scripts so that the path disabling ability cannot be used more than once
    //{
    //    for (int i = 0; i < allDisablePaths.Length; i++)
    //    {
    //        allDisablePaths[i].GetComponent<PathBlocker>().enabled = false;
    //    }
    //}
}
