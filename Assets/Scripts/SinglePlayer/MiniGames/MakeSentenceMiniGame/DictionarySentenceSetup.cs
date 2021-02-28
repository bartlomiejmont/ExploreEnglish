using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DictionarySentenceSetup : MonoBehaviour
{
    public static Dictionary<string, List<Tuple<string, int>>> wordSet;
    private List<Tuple<string, int>> list;
    public static int listCount;

    void Start()
    {
        list = new List<Tuple<string, int>>
        {
            Tuple.Create("It's", 0),
            Tuple.Create("a", 1),
            Tuple.Create("sentence", 2),
            Tuple.Create("in", 3),
            Tuple.Create("Polish", 4)
        };
        
        wordSet = new Dictionary<string, List<Tuple<string, int>>>
        {
            { "To jest zdanie po polsku", list }
        };

        listCount = wordSet.Sum(x => x.Value.Count);
    }

}
