using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using Michsky.UI.Heat;

public class MethodLineController : MonoBehaviour
{
    [Title("References")]
    [SerializeField]
    private TMP_Text methodName;
    [SerializeField]
    private TMP_Text methodParamInfo;
    [SerializeField]
    private TMP_Text methodPointsPrice;
    public ButtonManager addMethodBtn;
    public ButtonManager removeMethodBtn;
    [SerializeField]
    private GameObject unlockFilter;
    private bool methodAdded = false;
    private string itemMethodName;

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    public void SetData(SpecialMethodItem item, TowerCard towerCard)
    {
        itemMethodName = item.methodNameCode;
        methodName.text = item.methodNameDisplay;
        methodParamInfo.text = item.paramListDisplay != "" ? "Param: " + item.paramListDisplay : "";
        methodPointsPrice.text = "" + item.methodPointsPrice + " special point(s)";

        unlockFilter.SetActive(!item.unlockedStatus);

        addMethodBtn.enabled = item.unlockedStatus;
        removeMethodBtn.enabled = item.unlockedStatus;

        MethodIsAdded(towerCard);
    }

    public void MethodIsAdded(TowerCard towerCard)
    {
        methodAdded = towerCard.specialMethods.Contains(itemMethodName);

        methodName.color = methodAdded ? Color.green : Color.white;
    }
}
