using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacement : MonoBehaviour, IPointerClickHandler
{
    private bool isPlaced = false;
    public bool IsPlaced
    {
        get => isPlaced;
        set => isPlaced = value;
    }
    private ShopManager shopManager;

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