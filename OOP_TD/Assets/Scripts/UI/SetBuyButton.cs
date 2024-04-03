using System.Collections;
using System.Collections.Generic;
using Michsky.UI.Heat;
using UnityEngine;

public class SetBuyButton : MonoBehaviour
{
    [SerializeField] private List<ControllBuyButton> ButtonsList;
    private ShopManager shopManager;
    private void Awake()
    {
        shopManager = GetComponent<ShopManager>();
    }

    public void SetButtons(List<Tower> towersList)
    {
        for (int i = 0; i < ButtonsList.Count; i++)
        {
            AssignBtnValues(ButtonsList[i], towersList[i]);
        }
    }
    private void AssignBtnValues(ControllBuyButton btn, Tower tower)
    {
        BoxButtonManager boxButtonManager = btn.GetComponent<BoxButtonManager>();
        boxButtonManager.buttonTitle = tower.price + " Money";
        boxButtonManager.buttonDescription = tower.name;
        boxButtonManager.UpdateUI();

        btn.towerData = tower;
        btn.gameManager = shopManager.gameManager;
        btn.shopManager = shopManager;
    }
}
