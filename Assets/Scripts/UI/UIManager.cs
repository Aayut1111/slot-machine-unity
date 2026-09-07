using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private SlotMachine slotMachine;
    [SerializeField] private Button spinButton;
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private GameObject winBanner;
    [SerializeField] private TMP_Text winBannerText;

    private void OnEnable()
    {
        slotMachine.OnSpinStarted += HandleSpinStarted;
        slotMachine.OnBalanceChanged += HandleBalanceChanged;
        slotMachine.OnSpinResolved += HandleSpinResolved;
    }

    private void OnDisable()
    {
        slotMachine.OnSpinStarted -= HandleSpinStarted;
        slotMachine.OnBalanceChanged -= HandleBalanceChanged;
        slotMachine.OnSpinResolved -= HandleSpinResolved;
    }

    private void Start()
    {
        balanceText.text = $"Balance: {slotMachine.Balance}";
        winBanner.SetActive(false);
    }

    private void HandleSpinStarted()
    {
        spinButton.interactable = false;
        winBanner.SetActive(false);
    }

    private void HandleBalanceChanged(int newBalance)
    {
        balanceText.text = $"Balance: {newBalance}";
    }

    private void HandleSpinResolved(bool isWin, int payout)
    {
        spinButton.interactable = true;

        if (isWin)
        {
            winBannerText.text = $"YOU WIN {payout}!";
            winBanner.SetActive(true);
        }
    }
}