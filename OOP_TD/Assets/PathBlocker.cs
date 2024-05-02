using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class PathBlocker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject pathToBlock;
    private GameObject turningPoints;

    void Start()
    {
        turningPoints = GameObject.FindWithTag("TurningPoints");
    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData) //Disables the path, the decal and the ability to disable any more paths
    {
        pathToBlock.gameObject.SetActive(false);
        turningPoints.GetComponent<TurningPoints>().DisableBlocking();
        this.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) //While using the ability, hovering mouse over certain path makes it visually transparent to indicate that it can be disabled
    {
        this.GetComponent<DecalProjector>().fadeFactor = 0.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.GetComponent<DecalProjector>().fadeFactor = 1f;
    }
}
