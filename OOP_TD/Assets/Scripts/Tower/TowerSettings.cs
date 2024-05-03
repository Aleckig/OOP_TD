using Sirenix.OdinInspector;
using UnityEngine;

public class TowerSettings : MonoBehaviour
{
    [SerializeField] private Tower tower;
    private SphereCollider sphereCollider;
    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }
    public void SetTower(Tower _tower)
    {
        tower = _tower;
        sphereCollider.radius = tower.damageRange;
    }
    public Tower GetTower()
    {
        return tower;
    }
}