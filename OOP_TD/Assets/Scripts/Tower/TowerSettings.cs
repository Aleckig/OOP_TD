using System.Collections.Generic;
using System.ComponentModel;
using Sirenix.OdinInspector;
using UnityEngine;

public class TowerSettings : SerializedMonoBehaviour
{
    [SerializeField] private PlayerProgressData playerProgressData;
    [SerializeField] private Tower tower;
    [Title("Special Methods check list")]
    public Dictionary<string, bool> specialMethods;
    private SphereCollider sphereCollider;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        foreach (var item in playerProgressData.specialMethodsList)
        {
            specialMethods.Add(item.methodNameCode, false);
        }
    }

    private void Start()
    {
        foreach (var key in tower.specialMethods)
        {
            if (specialMethods.ContainsKey(key))
            {
                specialMethods[key] = true;
            }
        }
    }

    public void SetTower(Tower _tower)
    {
        tower = new(_tower);
        sphereCollider.radius = tower.damageRange;
    }

    public Tower GetTower()
    {
        return tower;
    }

    public bool GetMethodStatus(string key)
    {
        if (specialMethods.TryGetValue(key, out bool value))
            return value;
        return false;
    }
}