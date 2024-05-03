using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using Sirenix.Utilities;

public class TowerDeckEditCard : MonoBehaviour
{
    // --> References
    [Title("References")]
    [InlineEditor]
    public GameData gameData;
    [InlineEditor]
    public PlayerProgressData playerProgressData;
    public TowerDeckBuilder towerDeckBuilder;
    public RectTransform selfRT;
    public RectTransform dataBlockRT;
    public GameObject methodsListBlock;
    //
    public TMP_Text towerCardNameText;
    public TMP_Text pointsAmountText;
    [BoxGroup("UI text output")]
    public List<TMP_Text> towerStatsNameText;
    [BoxGroup("UI text output")]
    public List<TMP_Text> towerPointsText;
    [BoxGroup("UI text output")]
    public List<TMP_Text> towerPointInfoText;
    // <--

    // --> Prefabs
    [Title("Prefabs")]
    public GameObject methodLinePrefab;
    // <--

    // --> Script variables
    [Title("Script variables")]
    private TowerCard towerCard;
    [HorizontalGroup("Class field name in code and UI displayed equivalent")]
    [SerializeField] private List<string> classFieldNamesList, UIStatsTextList;
    // <--

    private void OnEnable()
    {
        towerCard = gameData.towerCardsList.Find(item => item.TowerCardId == towerDeckBuilder.ActiveCardId);
        if (towerCard == null) return;

        UpdateText();
        DisplayMethodList();
        UpdateBlockHeight();
    }

    public void AddPoint(int valueId)
    {
        if (towerCard.usedPoints >= towerDeckBuilder.pointsPerTower) return;

        switch (valueId)
        {
            case 0:
                towerCard.pricePoints += 1;
                break;
            case 1:
                towerCard.damagePoints += 1;
                break;
            case 2:
                towerCard.damageRangePoints += 1;
                break;
            case 3:
                towerCard.attackCooldownPoints += 1;
                break;
        }

        towerCard.usedPoints = towerCard.pricePoints + towerCard.damagePoints + towerCard.damageRangePoints + towerCard.attackCooldownPoints;
        UpdateText();
    }

    public void RemovePoint(int valueId)
    {
        switch (valueId)
        {
            case 0:
                if (towerCard.pricePoints == 0) break;
                towerCard.pricePoints -= 1;
                break;
            case 1:
                if (towerCard.damagePoints == 0) break;
                towerCard.damagePoints -= 1;
                break;
            case 2:
                if (towerCard.damageRangePoints == 0) break;
                towerCard.damageRangePoints -= 1;
                break;
            case 3:
                if (towerCard.attackCooldownPoints == 0) break;
                towerCard.attackCooldownPoints -= 1;
                break;
        }

        towerCard.usedPoints = towerCard.pricePoints + towerCard.damagePoints + towerCard.damageRangePoints + towerCard.attackCooldownPoints;
        UpdateText();
    }

    public void AddMethod(SpecialMethodItem item)
    {
        if (towerCard.usedSpecialPoints >= towerDeckBuilder.specialPointsPerTower) return;
        if (towerCard.specialMethods.Contains(item.methodNameCode)) return;

        towerCard.specialMethods.Add(item.methodNameCode);
        towerCard.usedSpecialPoints += item.methodPointsPrice;

        UpdateText();
    }
    public void RemoveMethod(SpecialMethodItem item)
    {
        if (!towerCard.specialMethods.Contains(item.methodNameCode)) return;

        towerCard.specialMethods.Remove(item.methodNameCode);
        towerCard.usedSpecialPoints -= item.methodPointsPrice;

        UpdateText();
    }

    private void UpdateText()
    {
        float heightValue = 100;

        towerCardNameText.text = towerCard.towerCardName;

        pointsAmountText.text = "Total points: " + towerDeckBuilder.pointsPerTower + " | Points available: " + (towerDeckBuilder.pointsPerTower - towerCard.usedPoints);
        pointsAmountText.text += "\nTotal special points : " + towerDeckBuilder.specialPointsPerTower + " | Special points available: " + (towerDeckBuilder.specialPointsPerTower - towerCard.usedSpecialPoints);

        for (int i = 0; i < classFieldNamesList.Count; i++)
        {
            string fieldBase = classFieldNamesList[i];
            string fieldDefVal = fieldBase + "DefVal";
            string fieldPoint = fieldBase + "Points";
            string fieldPointsCoef = fieldBase + "PointsCoef";

            towerStatsNameText[i].text = UIStatsTextList[i] + ": " + (towerCard.GetFieldValueFloat(fieldDefVal) + (towerCard.GetFieldValueFloat(fieldPoint) * towerDeckBuilder.GetFieldValueFloat(fieldPointsCoef)));

            towerPointsText[i].text = towerCard.GetFieldValueFloat(fieldPoint).ToString();

            towerPointInfoText[i].text = "1 point = " + towerDeckBuilder.GetFieldValueFloat(fieldPointsCoef) + " value";
            heightValue += 100;
        }

        dataBlockRT.sizeDelta = new(dataBlockRT.sizeDelta.x, heightValue);
    }
    private void DisplayMethodList()
    {
        RectTransform rectTransform = methodsListBlock.GetComponent<RectTransform>();
        int heightValue = 150;

        foreach (var item in playerProgressData.specialMethodsList)
        {
            heightValue += 100;
            rectTransform.sizeDelta = new(rectTransform.sizeDelta.x, heightValue);

            GameObject button = Instantiate(methodLinePrefab, methodsListBlock.transform);
            MethodLineController controller = button.GetComponent<MethodLineController>();
            controller.SetData(item, towerCard);
            controller.addMethodBtn.onClick.AddListener(() => { AddMethod(item); controller.MethodIsAdded(towerCard); });
            controller.removeMethodBtn.onClick.AddListener(() => { RemoveMethod(item); controller.MethodIsAdded(towerCard); });
        }

        float dataHeightVal = dataBlockRT.sizeDelta.y;
        dataBlockRT.sizeDelta = new(dataBlockRT.sizeDelta.x, dataHeightVal + heightValue);
    }

    private void UpdateBlockHeight()
    {
        float heightValue = 100;
        heightValue += dataBlockRT.sizeDelta.y;

        selfRT.sizeDelta = new(selfRT.sizeDelta.x, heightValue);
    }
}
