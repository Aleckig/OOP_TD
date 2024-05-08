using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Michsky.UI.Heat;
using Sirenix.OdinInspector;
using UnityEngine;

public class TowersActionController : MonoBehaviour
{
    [Title("References")]
    [SerializeField] private GameData gameData;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private List<ControllBuyButton> buyButtons;
    [SerializeField] private GameObject morphWidnow;
    [SerializeField] private Transform morphDamageTypesBlock;
    [SerializeField] private Transform morphMethodsBlock;
    [Title("Prefabs")]
    [SerializeField] private GameObject damageTypeTextPrefab;
    [SerializeField] private GameObject damageTypeDropdownPrefab;
    [SerializeField] private GameObject methodTextPrefab;
    [SerializeField] private GameObject methodDropdownPrefab;
    [Title("Settings")]
    [SerializeField] private float intervalCheckBuyButton = 1f;
    [SerializeField] private int AddDamageTypePrice = 100;
    private IEnumerator CheckBuyButtonCoroutine;
    private Tower activeTower;
    private LineManager damageTypeObjRef;

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            buyButtons[i].boxButtonManager.buttonTitle = gameData.towersList[i].price + " Bits";
            buyButtons[i].boxButtonManager.buttonDescription = gameData.towersList[i].name;
            buyButtons[i].boxButtonManager.UpdateUI();

            Tower tower = new(gameData.towersList[i]);
            buyButtons[i].boxButtonManager.onClick.AddListener(() => { shopManager.BuyTower(tower); });
        }

        CheckBuyButtonCoroutine = CheckBuyButton();
        StartCoroutine(CheckBuyButtonCoroutine);

        morphWidnow.SetActive(false);
    }

    private void OnDestroy()
    {
        StopCoroutine(CheckBuyButtonCoroutine);
    }

    private IEnumerator CheckBuyButton()
    {
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                if (!buyButtons[i].gameObject.activeInHierarchy) continue;

                if (levelManager.GetMoneyValue < gameData.towersList[i].price || shopManager.IsPlaced)
                {
                    buyButtons[i].DisableButton(true);
                }
                else buyButtons[i].DisableButton(false);
            }

            yield return new WaitForSeconds(intervalCheckBuyButton);
        }
    }

    public void OpenMorphWindow()
    {
        if (!shopManager.IsPlaced) return;

        activeTower = shopManager.GetSelectedTower;

        InitializeMorphWindow();
        morphWidnow.SetActive(true);
    }

    public void CloseMorphWindow()
    {
        damageTypeObjRef = null;
        morphWidnow.SetActive(false);
    }

    //Display tower information
    public void InitializeMorphWindow()
    {
        //Clear data from previous tower
        foreach (Transform child in morphDamageTypesBlock)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in morphMethodsBlock)
        {
            Destroy(child.gameObject);
        }

        int dTAdded = activeTower.towerDamageTypeAdded.Count; //for shortening code
        //Instantiate damage type lines (Base type damage are always same and initialized automatically)
        for (int i = 1; i < dTAdded; i++)
        {
            GameObject line = Instantiate(damageTypeTextPrefab, morphDamageTypesBlock);
            line.GetComponent<LineManager>().TextSet(string.Join(",", activeTower.towerDamageTypeAdded.Take(i + 1)));
        }

        //Instantiate methods lines with parameters (For now only HARD CODED by DEV!)
        if (activeTower.specialMethods.Contains("TargetEnemy"))
        {
            GameObject line = Instantiate(methodDropdownPrefab, morphMethodsBlock);
            LineManager lineManager = line.GetComponent<LineManager>();
            lineManager.TextSet("TargetEnemy");
            lineManager.DropdownSetList(gameData.EnemyNamesList);
        }

        //Instantiate methods lines without parameters
        foreach (var item in activeTower.specialMethods)
        {
            if (item == "TargetEnemy") continue;

            GameObject line = Instantiate(methodDropdownPrefab, morphMethodsBlock);
            LineManager lineManager = line.GetComponent<LineManager>();
            lineManager.TextSet(item);
        }
    }

    public void AddDamageType()
    {
        if (levelManager.GetMoneyValue < AddDamageTypePrice) return;
        if (activeTower.towerDamageTypeAdded.Count >= Tower.towerDamageTypeAvailable.Count) return;

        CreateDamageTypeLine();
    }

    private void CreateDamageTypeLine()
    {
        int dTAdded = activeTower.towerDamageTypeAdded.Count; //for shortening code

        {
            GameObject line = Instantiate(damageTypeDropdownPrefab, morphDamageTypesBlock);
            LineManager lineManager = line.GetComponent<LineManager>();
            lineManager.DropdownSetList(Tower.towerDamageTypeAvailable.GetRange(dTAdded, 3 - dTAdded));

            lineManager.DropdownSetSelectedValue(0);
            damageTypeObjRef = lineManager;
        }

        {
            string selectedType = damageTypeObjRef.DropdownGetSelectedValue();
            List<string> temp = new() { selectedType };
            damageTypeObjRef.DropdownSetList(temp);

            string lastType = selectedType == "Fire" ? "Frost" : "Fire";
            GameObject line = Instantiate(damageTypeTextPrefab, morphDamageTypesBlock);
            line.GetComponent<LineManager>().TextSet("Electro, " + damageTypeObjRef.DropdownGetSelectedValue() + ", " + lastType);
        }



    }
}
