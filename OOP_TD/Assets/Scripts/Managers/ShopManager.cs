using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
public class ShopManager : MonoBehaviour
{
  [SerializeField] private GameData gameData;
  [SerializeField] private LevelManager levelManager;
  [SerializeField] private GameObject UIBuyTowerButtonsContainer;
  private TowerPlacement selectedTowerPlacement;
  [SerializeField] private string returnMoneyMethodName;
  [SerializeField] private float returnMoneyCoef;
  [HideInInspector]
  public List<GameObject> saveCreatedTowersDataList = new();
  public bool IsPlaced => selectedTowerPlacement.IsPlaced;
  public Tower GetSelectedTower => selectedTowerPlacement.towerObj.GetComponent<TowerSettings>().GetTower();

  private void Awake()
  {
    UIBuyTowerButtonsContainer.SetActive(false);
  }
  public void SetActiveTowerPlacement(TowerPlacement activeTowerPlacement)
  {
    selectedTowerPlacement = activeTowerPlacement;
  }

  public void BuyTower(Tower towerData)
  {
    GameObject tower;
    if (levelManager.ChangeMoneyValue(-1 * towerData.price))
    {
      tower = Instantiate(towerData.towerPrefab, selectedTowerPlacement.transform);
      tower.GetComponent<TowerSettings>().SetTower(towerData);
      selectedTowerPlacement.IsPlaced = true;
      selectedTowerPlacement.towerObj = tower;

      //list for saving data
      saveCreatedTowersDataList.Add(tower);
    }
  }

  public void DestroyTower()
  {
    if (selectedTowerPlacement.towerObj == null) return;

    TowerSettings towerSettings = selectedTowerPlacement.towerObj.GetComponent<TowerSettings>();
    Tower towerData = towerSettings.GetTower();

    if (towerSettings.GetMethodStatus(returnMoneyMethodName))
      levelManager.ChangeMoneyValue(towerData.price);
    else levelManager.ChangeMoneyValue((int)(towerData.price * returnMoneyCoef));

    selectedTowerPlacement.IsPlaced = false;

    Destroy(selectedTowerPlacement.towerObj);

    //list for saving data
    saveCreatedTowersDataList.Remove(selectedTowerPlacement.towerObj);
  }

  public void DisplayUIButtonContainer(bool value)
  {
    UIBuyTowerButtonsContainer.SetActive(value);
  }
}
