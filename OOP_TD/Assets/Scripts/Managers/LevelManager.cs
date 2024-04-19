using TMPro;
using UnityEngine;

public class Levelmanager : MonoBehaviour
{
    [SerializeField] private TMP_Text UIMoneyText;
    [SerializeField] private int moneyValue = 200;
    public int GetMoneyValue => moneyValue;

    private void Awake()
    {
        UpdateMoneyUI();
    }

    /// <summary>
    /// Increasing or decreasing the amount of money.
    /// </summary>
    /// <param name="value">Positive or negative.</param>
    /// <returns>"False" if there not enough money for decresing, else "True".</returns>
    public bool ChangeMoneyValue(int value)
    {
        if ((moneyValue + value) < 0) return false;

        moneyValue += value;
        UpdateMoneyUI();
        return true;
    }

    private void UpdateMoneyUI()
    { UIMoneyText.text = "Money: " + moneyValue; }
}
