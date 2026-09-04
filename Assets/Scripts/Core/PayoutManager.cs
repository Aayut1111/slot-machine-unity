public class PayoutManager
{
    public int CalculatePayout(SymbolData winningSymbol, int betAmount)
    {
        return betAmount * winningSymbol.payoutMultiplier;
    }
}