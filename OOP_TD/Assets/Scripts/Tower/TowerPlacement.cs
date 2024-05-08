using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacement : MonoBehaviour, IPointerClickHandler
{

    private ShopManager shopManager;
    public GameObject towerObj;
    private bool isPlaced = false;
    public bool IsPlaced
    {
        get => isPlaced;
        set => isPlaced = value;
    }

    private void Awake()
    {
        shopManager = transform.parent.GetComponent<ShopManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        shopManager.SetActiveTowerPlacement(this);
        shopManager.DisplayUIButtonContainer(true);
    }
}