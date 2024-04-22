using System.Collections;
using Michsky.UI.Heat;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ControllBuyButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject disableFilter;
    public Tower towerData; //parameter for calling of the buying tower event
    public LevelManager gameManager;
    public ShopManager shopManager;
    private BoxButtonManager boxButtonManager;
    private void Awake()
    {
        disableFilter.SetActive(false);
        boxButtonManager = GetComponent<BoxButtonManager>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameManager.GetMoneyValue < towerData.price || shopManager.IsPlaced) return;
        //call of the buy event
        shopManager.BuyTower(towerData);
    }
    private void FixedUpdate()
    {
        Debug.Log(shopManager.IsPlaced);
        if (gameManager.GetMoneyValue < towerData.price || shopManager.IsPlaced)
        {
            disableFilter.SetActive(true);
            boxButtonManager.enabled = false;
            boxButtonManager.UpdateUI();
            return;
        }

        disableFilter.SetActive(false);
        boxButtonManager.enabled = true;
    }
}