using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy.Inspector;
using TMPro;

public class TowerDeckCardEdit : MonoBehaviour
{
    // --> References
    [Title("References")]
    [InlineEditor]
    public GameData gameData;
    public TowerDeckBuilder towerDeckBuilder;
    [HorizontalLine]
    public TMP_Text towerCardNameText;
    public TMP_Text pointsAmountText;
    public List<TMP_Text> towerStatsNameText;
    public List<TMP_Text> towerPointsText;
    public List<TMP_Text> towerPointInfoText;
    [HorizontalLine]
    // <--

    // --> Script variables
    [SerializeField] private List<string> statsNamesList;
    private TowerCard towerCard;
    // <--

    private void OnEnable()
    {
        towerCard = gameData.towerCardsList.Find(item => item.TowerCardId == towerDeckBuilder.ActiveCardId);
        if (towerCard == null) return;

        UpdateText();
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

    private void UpdateText()
    {
        towerCardNameText.text = towerCard.towerCardName;

        pointsAmountText.text = "Total points: " + towerDeckBuilder.pointsPerTower + '\n' + "Points available: " + (towerDeckBuilder.pointsPerTower - towerCard.usedPoints);

        towerStatsNameText[0].text = statsNamesList[0] + ": " + (towerCard.priceDefVal + (towerCard.pricePoints * towerDeckBuilder.pricePointsCoef));
        towerStatsNameText[1].text = statsNamesList[1] + ": " + (towerCard.damageDefVal + (towerCard.damagePoints * towerDeckBuilder.damagePointsCoef));
        towerStatsNameText[2].text = statsNamesList[2] + ": " + (towerCard.damageRangeDefVal + (towerCard.damageRangePoints * towerDeckBuilder.damageRangePointsCoef));
        towerStatsNameText[3].text = statsNamesList[3] + ": " + (towerCard.attackCooldownDefVal + (towerCard.attackCooldownPoints * towerDeckBuilder.attackCooldownPointsCoef));

        towerPointsText[0].text = "" + towerCard.pricePoints;
        towerPointsText[1].text = "" + towerCard.damagePoints;
        towerPointsText[2].text = "" + towerCard.damageRangePoints;
        towerPointsText[3].text = "" + towerCard.attackCooldownPoints;

        towerPointInfoText[0].text = "1 point = " + towerDeckBuilder.pricePointsCoef + " value";
        towerPointInfoText[1].text = "1 point = " + towerDeckBuilder.damagePointsCoef + " value";
        towerPointInfoText[2].text = "1 point = " + towerDeckBuilder.damageRangePointsCoef + " value";
        towerPointInfoText[3].text = "1 point = " + towerDeckBuilder.attackCooldownPointsCoef + " value";
    }
}
