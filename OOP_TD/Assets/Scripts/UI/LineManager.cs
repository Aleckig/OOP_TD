using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Michsky.UI.Heat;
using Sirenix.OdinInspector;

public class LineManager : MonoBehaviour
{
    [Title("Text")]
    [SerializeField] private bool EnabledPlaceholder = false;
    [ShowIf("EnabledPlaceholder")]
    [SerializeField] private TMP_Text textPlaceholder;
    [Title("Dropdown")]
    [SerializeField] private bool EnabledDropdown = false;
    [ShowIf("EnabledDropdown")]
    [SerializeField] private Dropdown dropdown;


    public string TextGet()
    {
        return textPlaceholder.text;
    }

    public void TextSet(string text)
    {
        textPlaceholder.text = text;
    }

    public string DropdownGetSelectedValue()
    {
        int index = dropdown.selectedItemIndex;
        return dropdown.items[index].itemName;
    }

    public bool DropdownSetSelectedValue(int index)
    {
        if (index >= dropdown.items.Count) return false;

        dropdown.selectedItemIndex = index;
        return true;
    }

    public void DropdownSetList(List<string> list)
    {
        dropdown.items = new();
        dropdown.selectedItemIndex = 0;

        foreach (var text in list)
        {
            dropdown.CreateNewItem(text);
        }
    }
}
