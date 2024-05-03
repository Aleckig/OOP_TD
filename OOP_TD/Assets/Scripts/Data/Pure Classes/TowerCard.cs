using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class TowerCard
{
    // --> Base values for towers fields which used for calculation of tower stats
    [SerializeField, HideInInspector]
    private int towerCardId;
    public string towerCardName;
    //
    public float priceDefVal,
    damageDefVal,
    damageRangeDefVal,
    attackCooldownDefVal;
    // <--

    [ShowInInspector]
    public int TowerCardId => towerCardId;

    // --> How much points are spended for each parameter
    public int usedPoints = 0,
    usedSpecialPoints = 0,
    pricePoints = 0,
    damagePoints = 0,
    damageRangePoints = 0,
    attackCooldownPoints = 0;
    // <--

    // --> Polymorphism
    public List<string> specialMethods;
    // <--

    public TowerCard(int _towerId, string _towerCardName, float _priceDefVal, float _damageDefVal, float _damageRangeDefVal, float _attackCooldownDefVal)
    {
        towerCardId = _towerId;
        towerCardName = _towerCardName;
        priceDefVal = _priceDefVal;
        damageDefVal = _damageDefVal;
        damageRangeDefVal = _damageRangeDefVal;
        attackCooldownDefVal = _attackCooldownDefVal;
    }

    public float GetFieldValueFloat(string fieldName)
    {
        FieldInfo field = GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (field != null)
        {
            return Convert.ToSingle(field.GetValue(this));
        }

        throw new ArgumentException("Field not found - " + fieldName);
    }
}
