using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reel : MonoBehaviour
{
    [SerializeField] private Image symbolDisplay;
    [SerializeField] private float spinDuration = 1.2f;
    [SerializeField] private AnimationCurve decelerationCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 5f);
    [SerializeField] private float flickerInterval = 0.06f;

    public SymbolData CurrentSymbol { get; private set; }
    public event Action OnReelStopped;
    public void PlayWinPunch()
{
    StopCoroutine(nameof(WinPunchRoutine));
    StartCoroutine(WinPunchRoutine());
}

private IEnumerator WinPunchRoutine()
{
    Vector3 originalScale = transform.localScale;
    Vector3 punchScale = originalScale * 1.2f;

    float duration = 0.15f;
    float t = 0f;
    while (t < duration)
    {
        transform.localScale = Vector3.Lerp(originalScale, punchScale, t / duration);
        t += Time.deltaTime;
        yield return null;
    }

    t = 0f;
    while (t < duration)
    {
        transform.localScale = Vector3.Lerp(punchScale, originalScale, t / duration);
        t += Time.deltaTime;
        yield return null;
    }

    transform.localScale = originalScale;
}

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
        symbolDisplay.sprite = blurPool[UnityEngine.Random.Range(0, blurPool.Count)].icon;

        float t = elapsed / spinDuration; // 0 at start, 1 near the end
        float currentInterval = flickerInterval * decelerationCurve.Evaluate(t);

        yield return new WaitForSeconds(currentInterval);
        elapsed += currentInterval;
    }

    CurrentSymbol = finalSymbol;
    symbolDisplay.sprite = finalSymbol.icon;
    OnReelStopped?.Invoke();

}

}