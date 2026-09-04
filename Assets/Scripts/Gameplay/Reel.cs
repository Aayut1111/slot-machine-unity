using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reel : MonoBehaviour
{
    [SerializeField] private Image symbolDisplay;
    [SerializeField] private float spinDuration = 1.2f;
    [SerializeField] private float flickerInterval = 0.06f;

    public SymbolData CurrentSymbol { get; private set; }
    public event Action OnReelStopped;

    // finalSymbol was already decided by RngService before this call —
    // Spin() is purely presentation, it never influences the outcome.
    public void Spin(SymbolData finalSymbol, List<SymbolData> blurPool, float startDelay)
    {
        StartCoroutine(SpinRoutine(finalSymbol, blurPool, startDelay));
    }

    private IEnumerator SpinRoutine(SymbolData finalSymbol, List<SymbolData> blurPool, float startDelay)
    {
        yield return new WaitForSeconds(startDelay);

        float elapsed = 0f;
        while (elapsed < spinDuration)
        {
            elapsed += flickerInterval;
            // visual-only randomness — never the real outcome
            symbolDisplay.sprite = blurPool[UnityEngine.Random.Range(0, blurPool.Count)].icon;
            yield return new WaitForSeconds(flickerInterval);
        }

        CurrentSymbol = finalSymbol;
        symbolDisplay.sprite = finalSymbol.icon;
        OnReelStopped?.Invoke();
    }
}