using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class LevelManager : MonoBehaviour
{
    [Title("References")]
    [SerializeField] private GameData gameData;
    [SerializeField] private PlayerProgressData playerProgressData;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private TMP_Text UIMoneyText;
    [SerializeField] private AbilityManager abilityManager;
    [Title("Settings")]
    [SerializeField] private int moneyValue = 200;
    public int GetMoneyValue => moneyValue;
    [Title("Level Data")]
    [SerializeField] private string levelDifficulty;
    [SerializeField] private LevelDataSaveItem levelDataSave;
    private void Awake()
    {
        UpdateMoneyUI();
    }

    /// <summary>
    /// Increasing or decreasing the amount of money.
    /// </summary>
    /// <param name="value">Positive or negative.</param>
    /// <returns>"False" if there not enough money for decresing, else "True".</returns>
    public bool ChangeMoneyValue(int value)
    {
        if ((moneyValue + value) < 0) return false;

        moneyValue += value;
        UpdateMoneyUI();
        return true;
    }

    private void UpdateMoneyUI()
    {
        UIMoneyText.text = moneyValue.ToString();
    }

    //Call when base is destroyed save all other data before that
    public void OnLevelEnd(bool levelStatus)
    {
        levelDataSave.completionStatus = levelStatus;

        levelDataSave.towerCardsList.Clear();
        levelDataSave.towerCardsList = gameData.towerCardsList;

        levelDataSave.towersList.Clear();
        foreach (var towerObj in shopManager.saveCreatedTowersDataList)
        {
            levelDataSave.towersList.Add(towerObj.GetComponent<TowerSettings>().GetTower());
        }

        // levelDataSave.powerupUsedCountDict = abilityManager.SOMEDICTIOMARY;
        levelDataSave.powerupUsedCountDict = abilityManager.abilityDictionary; //Added by Kimi

        playerProgressData.levelDataSaveManager.SaveData(gameData.activeLevelId, levelDifficulty, levelDataSave);
    }

    //Copy that methods into needed script to send data
    public void SaveEnemyDamage(float totalDamage, DictionaryStrFloat enemiesThatDealtDamageDict)
    {
        levelDataSave.baseDamageReceived = totalDamage;
        foreach (var item in enemiesThatDealtDamageDict)
        {
            levelDataSave.enemiesThatDealtDamageDict.Clear();
            levelDataSave.enemiesThatDealtDamageDict.Add(item.Key, item.Value);
        }
    }

    public void SavePowerupsCount(DictionaryStrInt powerupUsedCountDict)
    {
        foreach (var item in powerupUsedCountDict)
        {
            levelDataSave.powerupUsedCountDict.Clear();
            levelDataSave.powerupUsedCountDict.Add(item.Key, item.Value);
        }
    }
}
