using System;

[Serializable]
public class TowerCard
{
    // --> Base values for towers fields which used for calculation of tower stats
    public int priceDefVal,
    damageDefVal;
    public float damageRangeDefVal,
    attackCooldownDefVal;
    // <--

    // --> How much points are spended for each parameter
    public string TowerCardName;
    public int pricePoints,
    damagePoints,
    damageRangePoints,
    attackCooldownPoints;
    // <--
}
