using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Michsky.UI.Heat;

public class AbilityManager : MonoBehaviour
{
    public TMP_Text shieldAbility;
    public TMP_Text shieldAbilityHighlighted;
    public int shieldsAmount;
    public ButtonManager shieldButton;
    public TMP_Text pathAbility;
    public TMP_Text pathAbilityHighlighted;
    public int pathAbilityAmount;
    public ButtonManager pathButton;
    //private bool pathAbilityUsed;
    private bool pathAbilityGained;
    public WaveSpawner waveSpawner;
    private bool enablePathBlock;

    void Start()
    {
        shieldsAmount = 0;
        pathAbilityAmount = 0;
        //pathAbilityUsed = false;
        pathAbilityGained = false;
        shieldAbility.SetText(shieldsAmount.ToString());
        shieldAbilityHighlighted.SetText(shieldsAmount.ToString());
        pathAbility.SetText(pathAbilityAmount.ToString());
        pathAbilityHighlighted.SetText(pathAbilityAmount.ToString());
        pathButton.enabled = false;
    }

    void Update()
    {
        if (waveSpawner.betweenWaves == true && pathAbilityAmount > 0)// && pathAbilityUsed == false)
        {
            pathButton.enabled = true;
        }
        else if (waveSpawner.betweenWaves == false)
        {
            pathButton.enabled = false;
        }
    }

    public void IncreaseShieldCount()
    {
        shieldButton.enabled = true;
        shieldsAmount++;
        shieldAbility.SetText(shieldsAmount.ToString());
        shieldAbilityHighlighted.SetText(shieldsAmount.ToString());

    }

    public void DecreaseShieldCount()
    {
        shieldsAmount--;
        if (shieldsAmount == 0)
        {
            shieldButton.enabled = false;
        }
        shieldAbility.SetText(shieldsAmount.ToString());
        shieldAbilityHighlighted.SetText(shieldsAmount.ToString());
    }

    public void IncreasePathAbilityCount()
    {
        if (pathAbilityGained == false)
        {
            //pathButton.enabled = true;
            pathAbilityAmount++;
            pathAbility.SetText(pathAbilityAmount.ToString());
            pathAbilityHighlighted.SetText(pathAbilityAmount.ToString());
            pathAbilityGained = true;
        }
    }

    public void DecreasePathAbilityCount()
    {
        pathAbilityAmount--;
        pathAbility.SetText(pathAbilityAmount.ToString());
        pathAbilityHighlighted.SetText(pathAbilityAmount.ToString());
        //pathAbilityUsed = true;
        pathButton.enabled = false;
    }
}
