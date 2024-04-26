using System;

[Serializable]
public class TowerCard
{
    // --> Base values for towers fields which used for calculation of tower stats
    private int towerCardId;
    public string towerCardName;
    public int priceDefVal,
    damageDefVal;
    public float damageRangeDefVal,
    attackCooldownDefVal;
    // <--

    // --> How much points are spended for each parameter
    public int usedPoints = 0,
    pricePoints = 0,
    damagePoints = 0,
    damageRangePoints = 0,
    attackCooldownPoints = 0;
    // <--

    public TowerCard(int _towerId, string _towerCardName, int _priceDefVal, int _damageDefVal, float _damageRangeDefVal, float _attackCooldownDefVal)
    {
        towerCardId = _towerId;
        towerCardName = _towerCardName;
        priceDefVal = _priceDefVal;
        damageDefVal = _damageDefVal;
        damageRangeDefVal = _damageRangeDefVal;
        attackCooldownDefVal = _attackCooldownDefVal;
    }

    public int TowerCardId => towerCardId;
}
