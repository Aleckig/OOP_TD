using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Michsky.UI.Heat;
using Sirenix.OdinInspector;

public class LineManager : SerializedMonoBehaviour
{
    [Title("Text")]
    [SerializeField] private bool EnablePlaceholder = false;
    [ShowIf("EnablePlaceholder")]
    [SerializeField] private TMP_Text textPlaceholder;
    [SerializeField] private bool EnableMultiplePlaceholder = false;
    [ShowIf("EnableMultiplePlaceholder")]
    [SerializeField] private Dictionary<string, TMP_Text> textPlaceholdersList = new();
    //
    [Title("Dropdown")]
    [SerializeField] private bool EnableDropdown = false;
    [ShowIf("EnableDropdown")]
    [SerializeField] private Dropdown dropdown;
    //
    [Title("Additional Func.")]
    [SerializeField] private bool EnableButton = false;
    [ShowIf("EnableButton")]
    [SerializeField] private ButtonManager buttonManager;
    [SerializeField] private bool EnableDestroyOnDisable = false;

    public string TextGet()
    {
        if (!EnablePlaceholder) return null;

        return textPlaceholder.text;
    }
    public string TextGet(string key)
    {
        if (!EnableMultiplePlaceholder) return null;

        if (!textPlaceholdersList.ContainsKey(key)) return "Wrong key";

        return textPlaceholdersList[key].text;
    }

    public void TextSet(string text)
    {
        if (!EnablePlaceholder) return;

        textPlaceholder.text = text;
    }
    public void TextSet(string key, string value)
    {
        if (!EnableMultiplePlaceholder) return;

        if (!textPlaceholdersList.ContainsKey(key)) return;

        textPlaceholdersList[key].text = value;
    }

    public string DropdownGetSelectedValue()
    {
        if (!EnableDropdown) return null;

        int index = dropdown.selectedItemIndex;
        return dropdown.items[index].itemName;
    }

    public bool DropdownSetSelectedValue(int index)
    {
        if (!EnableDropdown) return false;

        if (index >= dropdown.items.Count) return false;

        dropdown.selectedItemIndex = index;
        return true;
    }

    public void DropdownSetList(List<string> list)
    {
        if (!EnableDropdown) return;

        dropdown.items = new();
        dropdown.selectedItemIndex = 0;

        foreach (var text in list)
        {
            dropdown.CreateNewItem(text);
        }
    }

    public void AddEventOnClick(System.Action methodWithParameters)
    {
        if (!EnableButton) return;

        buttonManager.onClick.AddListener(() => { methodWithParameters(); });
    }

    private void OnDisable()
    {
        if (EnableDestroyOnDisable) Destroy(gameObject);
    }
}
