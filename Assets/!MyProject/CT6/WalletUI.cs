using UnityEngine;
using TMPro;

public class WalletUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private GameObject walletGameObject;

    private int currentBalance = 0;

    private void Start()
    {
        if (walletGameObject != null)
        {
            walletGameObject.SetActive(false);
        }

        UpdateBalanceText();
    }

    public void ShowWallet()
    {
        if (walletGameObject != null)
        {
            walletGameObject.SetActive(true);
        }
    }

    public void AddCoins(int amount)
    {
        currentBalance += amount;
        UpdateBalanceText();
        Debug.Log($"Баланс: {currentBalance}");
    }

    private void UpdateBalanceText()
    {
        if (balanceText != null)
        {
            balanceText.text = $"Монеты: {currentBalance}";
        }
    }
}