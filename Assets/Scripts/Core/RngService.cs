using System;
using System.Collections.Generic;

// Plain C# — no MonoBehaviour. It doesn't need a GameObject, and keeping it
// framework-agnostic makes it trivial to unit test in isolation.
public class RngService
{
    private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

    // Weighted pick: symbols with a higher `weight` land more often,
    // mirroring how real slot machines bias toward common low-value symbols.
    public SymbolData GetRandomSymbol(List<SymbolData> pool)
    {
        int totalWeight = 0;
        foreach (var s in pool) totalWeight += s.weight;

        int roll = _random.Next(0, totalWeight);
        int cumulative = 0;
        foreach (var s in pool)
        {
            cumulative += s.weight;
            if (roll < cumulative) return s;
        }
        return pool[pool.Count - 1]; // unreachable in practice, keeps the compiler happy
    }
}