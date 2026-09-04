using UnityEngine;

// A ScriptableObject means each symbol (Cherry, Bell, Seven…) becomes an
// asset you create and tune in the Inspector — no hardcoded enum of symbols.
[CreateAssetMenu(fileName = "NewSymbol", menuName = "SlotMachine/Symbol")]
public class SymbolData : ScriptableObject
{
    public string symbolId;
    public Sprite icon;

    [Tooltip("Relative weight in the RNG pool — higher appears more often")]
    public int weight = 10;

    [Tooltip("Bet multiplier paid out when all reels land this symbol")]
    public int payoutMultiplier = 2;
}
