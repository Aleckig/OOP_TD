using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy.Inspector;

public class TowerDeckBuilder : MonoBehaviour
{
    // --> References
    [Title("References")]
    [InlineEditor]
    public GameData gameData;
    public GameObject towerDeckGrid;
    // <--

    // --> Prefabs
    [Title("Prefabs")]
    public GameObject towerCardBtnPrefab;
    // <--

    [Title("Settings")]
    public int pointsPerTower = 10;

    // --> Default values for towers fields
    // This is starting values for all towers
    [BoxGroup("Default values for towers fields")]
    public int priceDefVal,
    damageDefVal;
    [BoxGroup("Default values for towers fields")]
    public float damageRangeDefVal,
    attackCooldownDefVal;
    // <--

    // --> Limit for points 
    /*
    [BoxGroup("How much points can be spent")]
    public int pricePointsLimit;
    [BoxGroup("How much points can be spent")]
    public int damagePointsLimit,
    damageRangePointsLimit,
    attackCooldownPointsLimit;
    */
    // <--

    // --> Coefficient for calculation of value for future tower based on spent points
    [BoxGroup("Coefficient for calculation from points to value")]
    public int pricePointsCoef;
    [BoxGroup("Coefficient for calculation from points to value")]
    public float damagePointsCoef,
    damageRangePointsCoef,
    attackCooldownPointsCoef;
    // <--

    // --> Script variables
    private int activeCardIndex;
    // <--



    private void InstaniateCardButtons()
    {
        if (gameData.towerCardsList.Count != 4)
        {
            for (int i = 0; i < 4; i++)
            {
                gameData.towerCardsList.Add(new TowerCard());
                GameObject button = Instantiate(towerCardBtnPrefab, towerDeckGrid.transform);

                // button.GetComponent<>
            }
        }
    }
    public void CalculateTowerStats()
    {

    }
}
