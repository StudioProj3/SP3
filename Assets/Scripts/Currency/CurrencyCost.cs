using System;
using System.Collections.Generic;

[Serializable]
public struct CurrencyCost 
{
    public List<Pair<ItemBase, int>> costs;
}