using UnityEngine;

public class TowerSettings : MonoBehaviour
{
    [SerializeField] private Tower tower;

    public void SetTower(Tower _tower)
    {
        tower = _tower;
    }
    public Tower TransferClass()
    {
        return tower;
    }
}