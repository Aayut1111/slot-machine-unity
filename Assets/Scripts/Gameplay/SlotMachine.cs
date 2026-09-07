using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private List<Reel> reels;
    [SerializeField] private List<SymbolData> symbolPool;
    [SerializeField] private int betAmount = 10;
    [SerializeField] private AudioSource winSfxSource;
[SerializeField] private AudioClip winClip;

    private readonly RngService _rng = new RngService();
    private readonly WinChecker _winChecker = new WinChecker();
    private readonly PayoutManager _payoutManager = new PayoutManager();

    private int _reelsStopped;

    public void Spin()
    {
        _reelsStopped = 0;

        for (int i = 0; i < reels.Count; i++)
        {
            SymbolData result = _rng.GetRandomSymbol(symbolPool);

            // staggered startDelay is what sells the "reels stop one by one" feel — see Phase 08
            reels[i].Spin(result, symbolPool, startDelay: i * 0.4f);
        }
    }

    // subscribe each reel's OnReelStopped to this in Awake()/OnEnable()
    private void HandleReelStopped()
    {
        _reelsStopped++;
        if (_reelsStopped < reels.Count) return;

        var finalSymbols = reels.Select(r => r.CurrentSymbol).ToList();
        if (_winChecker.IsWin(finalSymbols))
        {
            int payout = _payoutManager.CalculatePayout(finalSymbols[0], betAmount);
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
    }
} 