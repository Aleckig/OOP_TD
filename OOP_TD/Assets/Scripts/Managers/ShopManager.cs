using System.Collections.Generic;
using UnityEngine;
public class ShopManager : MonoBehaviour
{
  public GameManager gameManager;
  [SerializeField] private GameObject UIButtonContainer;
  [SerializeField] private List<Tower> towerList;

  private TowerPlacement towerPlacement;
  public bool IsPlaced => towerPlacement.IsPlaced;

  private void Awake()
  {
    GetComponent<SetBuyButton>().SetButtons(towerList);
  }

  public void SetActiveTowerPlacement(TowerPlacement activeTowerPlacement)
  {
    towerPlacement = activeTowerPlacement;
  }

  public void BuyTower(Tower towerData)
  {
    GameObject tower;
    if (gameManager.ChangeMoneyValue(-1 * towerData.price))
    {
      tower = Instantiate(towerData.towerPrefab, towerPlacement.transform);
      tower.GetComponent<TowerSettings>().SetTower(towerData);
      towerPlacement.IsPlaced = true;
    }

  }

  public void DisplayUIButtonContainer(bool value)
  {
    UIButtonContainer.SetActive(value);
  }
}
