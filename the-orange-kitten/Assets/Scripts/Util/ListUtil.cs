using System.Collections.Generic;
using System.Linq;


public static class ListUtil
{
    public static T ElementAtOrElse<T>(this List<T> list, int index, T alternative = default)
    {
        return index < 0 || index >= list.Count ? alternative : list.ElementAt(index);
    }
}