using System.Collections.Generic;
using UnityEngine;

public class DictionaryPrototype : MonoBehaviour
{
    public static Dictionary<string, string> wordSet;

    void Start()
    {
        wordSet = new Dictionary<string, string>
        {
            {"Make", "Made"},
            {"Speak", "Spoke"},
            {"Bring", "Brought"},
            {"Cost", "Cost" },
            {"Leave", "Left" }
        };
    }

}
