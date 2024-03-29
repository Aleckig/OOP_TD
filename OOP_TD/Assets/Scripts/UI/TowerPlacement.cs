using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TowerPlacement : MonoBehaviour, IPointerClickHandler
{
    //Event
    public UnityEvent onClick = new UnityEvent();

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }
}