using System.Collections.Generic;
using System.Linq;

public class WinChecker
{
    public bool IsWin(List<SymbolData> finalSymbols)
    {
        if (finalSymbols == null || finalSymbols.Count == 0) return false;
        string firstId = finalSymbols[0].symbolId;
        return finalSymbols.All(s => s.symbolId == firstId);
    }
}