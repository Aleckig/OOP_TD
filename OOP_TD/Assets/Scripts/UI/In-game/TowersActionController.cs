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
    private bool addDTLocked = false;
    System.Action saveEnemyTarget;
    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            buyButtons[i].boxButtonManager.buttonTitle = gameData.towersList[i].price + " Bits";
            buyButtons[i].boxButtonManager.buttonDescription = gameData.towersList[i].towerName;
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
        if (CheckBuyButtonCoroutine != null) StopCoroutine(CheckBuyButtonCoroutine);
    }

    private IEnumerator CheckBuyButton()
    {
        while (true)
        {
            //Buy tower buttons check
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
        addDTLocked = false;

        saveEnemyTarget.Invoke();

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

            for (int i = 0; i < gameData.EnemyNamesList.Count; i++)
            {
                if (activeTower.targetEnemyName == gameData.EnemyNamesList[i])
                {
                    lineManager.DropdownSetSelectedValue(i);
                }
            }

            saveEnemyTarget = () => { SaveEnemyTarget(lineManager, activeTower); };
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
        if (addDTLocked) return;

        CreateDamageTypeLine();
        addDTLocked = true;
    }

    private void CreateDamageTypeLine()
    {
        int dTAdded = activeTower.towerDamageTypeAdded.Count; //for shortening code

        GameObject line = Instantiate(damageTypeDropdownPrefab, morphDamageTypesBlock);
        LineManager lineManager = line.GetComponent<LineManager>();

        lineManager.TextSet(string.Join(", ", activeTower.towerDamageTypeAdded.Take(activeTower.towerDamageTypeAdded.Count)) + ",");

        List<string> notAddedDTList = new();
        foreach (string dType in Tower.towerDamageTypeAvailable)
        {
            if (!activeTower.towerDamageTypeAdded.Contains(dType))
            {
                notAddedDTList.Add(dType);
            }
        }

        lineManager.DropdownSetList(notAddedDTList.GetRange(0, notAddedDTList.Count));
        lineManager.DropdownSetSelectedValue(0);

        lineManager.AddEventOnClick(() => { SaveDamageType(lineManager, activeTower); addDTLocked = false; levelManager.ChangeMoneyValue(-1 * AddDamageTypePrice); });
    }

    public void SaveDamageType(LineManager lineManager, Tower activeTowerParam)
    {
        activeTowerParam.towerDamageTypeAdded.Add(lineManager.DropdownGetSelectedValue());

        GameObject newLine = Instantiate(damageTypeTextPrefab, morphDamageTypesBlock);
        newLine.GetComponent<LineManager>().TextSet(string.Join(", ", activeTowerParam.towerDamageTypeAdded.Take(activeTowerParam.towerDamageTypeAdded.Count)));

        Destroy(lineManager.gameObject);
    }

    public void SaveEnemyTarget(LineManager lineManager, Tower activeTowerParam)
    {
        activeTowerParam.targetEnemyName = lineManager.DropdownGetSelectedValue();
    }
}
