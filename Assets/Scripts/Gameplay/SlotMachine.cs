using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private List<Reel> reels;
    [SerializeField] private List<SymbolData> symbolPool;
    [SerializeField] private int betAmount = 10;
    [SerializeField] private int startingBalance = 1000;
    [SerializeField] private AudioSource winSfxSource;
    [SerializeField] private AudioClip winClip;

    private readonly RngService _rng = new RngService();
    private readonly WinChecker _winChecker = new WinChecker();
    private readonly PayoutManager _payoutManager = new PayoutManager();

    private int _reelsStopped;

    public int Balance { get; private set; }

    public event Action OnSpinStarted;
    public event Action<int> OnBalanceChanged;
    public event Action<bool, int> OnSpinResolved;

    private void Awake()
    {
        Balance = startingBalance;

        foreach (var reel in reels)
        {
            reel.OnReelStopped += HandleReelStopped;
        }
    }

    public void Spin()
    {
        if (Balance < betAmount)
        {
            Debug.Log("Not enough balance to spin.");
            return;
        }

        Balance -= betAmount;
        OnBalanceChanged?.Invoke(Balance);
        OnSpinStarted?.Invoke();

        _reelsStopped = 0;
        for (int i = 0; i < reels.Count; i++)
        {
            SymbolData result = _rng.GetRandomSymbol(symbolPool);
            reels[i].Spin(result, symbolPool, startDelay: i * 0.4f);
        }
    }

    private void HandleReelStopped()
    {
        _reelsStopped++;
        if (_reelsStopped < reels.Count) return;

        var finalSymbols = reels.Select(r => r.CurrentSymbol).ToList();
        bool isWin = _winChecker.IsWin(finalSymbols);
        int payout = 0;

        if (isWin)
        {
            payout = _payoutManager.CalculatePayout(finalSymbols[0], betAmount);
            Balance += payout;
            OnBalanceChanged?.Invoke(Balance);

            Debug.Log($"WIN! Payout: {payout}");

            foreach (var reel in reels)
            {
                reel.PlayWinPunch();
            }

            if (winSfxSource != null && winClip != null)
            {
                winSfxSource.PlayOneShot(winClip);
            }
        }
        else
        {
            Debug.Log("No win this spin.");
        }

        OnSpinResolved?.Invoke(isWin, payout);
    }
}