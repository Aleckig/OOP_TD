using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy.Inspector;
using Michsky.UI.Heat;
using UnityEngine.UI;

public class TowerDeckBuilder : MonoBehaviour
{
    // --> References
    [Title("References")]
    [InlineEditor]
    public GameData gameData;
    public GameObject towerDeckList;
    public GameObject towerDeckGrid;
    public GameObject towerDeckCardBlock;
    // <--

    // --> Prefabs
    [Title("Prefabs")]
    public List<GameObject> towersPrefabsList;
    public GameObject towerCardBtnPrefab;
    // <--

    // --> Default values for towers fields
    [Title("Settings")]
    public int pointsPerTower = 10;

    // This is starting values for all towers
    [BoxGroup("Default values for towers fields")]
    public int priceDefVal,
    damageDefVal;
    [BoxGroup("Default values for towers fields")]
    public float damageRangeDefVal,
    attackCooldownDefVal;
    // <--

    // --> Coefficient for calculation of value for future tower based on spent points
    [BoxGroup("Coefficient for calculation from points to value")]
    public int pricePointsCoef,
    damagePointsCoef;
    [BoxGroup("Coefficient for calculation from points to value")]
    public float damageRangePointsCoef,
    attackCooldownPointsCoef;
    // <--

    // --> Script variables
    private int activeCardId;
    public int ActiveCardId => activeCardId;
    // <--

    private void Start()
    {
        InstaniateCardButtons();
    }

    private void InstaniateCardButtons()
    {
        if (gameData.towerCardsList.Count < 4)
        {
            for (int i = 0; i < 4; i++)
            {
                string towerName = "Tower " + (i + 1);
                gameData.towerCardsList.Add(new TowerCard(i, towerName, priceDefVal, damageDefVal, damageRangeDefVal, attackCooldownDefVal));
            }
        }

        foreach (TowerCard towerCard in gameData.towerCardsList)
        {
            GameObject button = Instantiate(towerCardBtnPrefab, towerDeckGrid.transform);

            BoxButtonManager btnManager = button.GetComponent<BoxButtonManager>();
            btnManager.buttonTitle = towerCard.towerCardName;
            btnManager.onClick.AddListener(() => SetActiveCard(towerCard.TowerCardId));
            btnManager.onClick.AddListener(() => { towerDeckList.SetActive(false); towerDeckCardBlock.SetActive(true); });
            btnManager.UpdateUI();
        }
    }
    private void SetActiveCard(int towerCardId)
    {
        activeCardId = towerCardId;
    }

    public void CalculateTowersStats()
    {
        int _price = 0, _damage = 0;
        float _damageRange = 0, _attackCooldown = 0;

        for (int i = 0; i < 4; i++)
        {
            _price = gameData.towerCardsList[i].priceDefVal + (gameData.towerCardsList[i].pricePoints * pricePointsCoef);
            _damage = gameData.towerCardsList[i].damageDefVal + (gameData.towerCardsList[i].damagePoints * damagePointsCoef);
            _damageRange = gameData.towerCardsList[i].damageRangeDefVal + (gameData.towerCardsList[i].damageRangePoints * damageRangePointsCoef);
            _attackCooldown = gameData.towerCardsList[i].attackCooldownDefVal + (gameData.towerCardsList[i].attackCooldownPoints * attackCooldownPointsCoef);

            if (gameData.towersList.Count < 4)
            {
                for (int k = 0; k < 4; k++)
                {
                    gameData.towersList.Add(new Tower(towersPrefabsList[i], gameData.towerCardsList[i].towerCardName, _price, _damage, _damageRange, _attackCooldown));
                }
                return;
            }

            for (int k = 0; k < 4; k++)
            {
                gameData.towersList[i] = new Tower(towersPrefabsList[i], gameData.towerCardsList[i].towerCardName, _price, _damage, _damageRange, _attackCooldown);
            }
        }

    }
}
