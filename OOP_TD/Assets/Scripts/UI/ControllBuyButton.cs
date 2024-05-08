using Michsky.UI.Heat;
using UnityEngine;

public class ControllBuyButton : MonoBehaviour
{
    [HideInInspector]
    public BoxButtonManager boxButtonManager;
    public GameObject disableFilter;

    private void Start()
    {
        boxButtonManager = GetComponent<BoxButtonManager>();
        disableFilter.SetActive(false);
    }
    public void DisableButton(bool disable)
    {
        disableFilter.SetActive(disable);
        boxButtonManager.enabled = !disable;

        if (disable) boxButtonManager.UpdateUI();
    }
}