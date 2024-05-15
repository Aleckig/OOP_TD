using UnityEngine;
using UnityEngine.EventSystems;

public class PathBlocker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject[] pathToBlock;
    public GameObject[] additionalObjectsToBlock; // Add the chosen game objects here
    private GameObject turningPoints;
    public GameObject blockedVFX;
    [SerializeField] private AbilityManager abilityManager;

    void Start()
    {
        turningPoints = GameObject.FindWithTag("TurningPoints");
    }

    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData) //Disables the path, the decal and the ability to disable any more paths
    {
        foreach (GameObject path in pathToBlock)
        {
            path.SetActive(false);
        }

        foreach (GameObject obj in additionalObjectsToBlock)
        {
            obj.SetActive(false); // Disable the chosen game objects
        }

        blockedVFX.SetActive(true);
        if (!abilityManager.abilityDictionary.ContainsKey("PathBlock"))
        {
            abilityManager.abilityDictionary.Add("PathBlock", 1);
        }
        else
        {
            abilityManager.abilityDictionary["PathBlock"]++;
        }
        //turningPoints.GetComponent<TurningPoints>().DisableBlocking();
        //this.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) //While using the ability, hovering mouse over certain path makes it visually transparent to indicate that it can be disabled
    {
        //this.GetComponent<DecalProjector>().fadeFactor = 0.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //this.GetComponent<DecalProjector>().fadeFactor = 1f;
    }
}
