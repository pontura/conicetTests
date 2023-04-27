using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Events
{
    public static System.Action<string> Log = delegate { };
    public static System.Action<int> ChangeCharacter = delegate { };
    public static System.Action<int, List<string>, List<string>> OnStatsGameDone = delegate { };

}
   
