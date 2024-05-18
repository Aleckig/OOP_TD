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
    public int baseFixAmount;
    public ButtonManager pathButton;
    public ButtonManager baseFixButton;
    public TMP_Text baseFix;
    public TMP_Text baseFixHighlighted;
    //private bool pathAbilityUsed;
    private bool pathAbilityGained;
    public WaveSpawner waveSpawner;
    private bool enablePathBlock;
    public Dictionary<string, int> abilityDictionary;

    void Start()
    {
        shieldsAmount = 0;
        pathAbilityAmount = 0;
        baseFixAmount = 0;
        //pathAbilityUsed = false;
        pathAbilityGained = false;
        shieldAbility.SetText(shieldsAmount.ToString());
        shieldAbilityHighlighted.SetText(shieldsAmount.ToString());
        pathAbility.SetText(pathAbilityAmount.ToString());
        pathAbilityHighlighted.SetText(pathAbilityAmount.ToString());
        baseFix.SetText(baseFixAmount.ToString());
        baseFixHighlighted.SetText(baseFixAmount.ToString());
        shieldButton.enabled = false;
        pathButton.enabled = false;
        baseFixButton.enabled = false;
        abilityDictionary = new() { };
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

    public void IncreaseBaseFixCount()
    {
        baseFixButton.enabled = true;
        baseFixAmount++;
        baseFix.SetText(baseFixAmount.ToString());
        baseFixHighlighted.SetText(baseFixAmount.ToString());
    }

    public void DecreaseBaseFixCount()
    {
        baseFixAmount--;
        if (baseFixAmount == 0)
        {
            baseFixButton.enabled = false;
        }
        baseFix.SetText(baseFixAmount.ToString());
        baseFixHighlighted.SetText(baseFixAmount.ToString());
    }
}
