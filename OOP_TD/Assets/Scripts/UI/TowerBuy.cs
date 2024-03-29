using UnityEngine;

public class TowerBuy : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private GameObject towerPlacementPad;
    public void SetActiveTowerPlace(GameObject towerPlace)
    {
        towerPlacementPad = towerPlace;
    }
    public void BuyTower(GameObject towerPrefab)
    {
        TowerSettings towerSettings = towerPrefab.GetComponent<TowerSettings>();
        Tower towerStats = new(towerSettings.TransferClass());

        if (gameManager.ChangeMoneyValue(-1 * towerStats.price))
            Instantiate(towerPrefab, towerPlacementPad.transform);

    }
}
