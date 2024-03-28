using UnityEngine;

public class TowerSettings : MonoBehaviour
{
    [SerializeField] private Tower tower;

    public Tower TransferClass()
    {
        return tower;
    }
}