using System.Collections.Generic;
using UnityEngine;

public class DictionarySetup : MonoBehaviour
{
    public static Dictionary<string, string> wordSet;

    void Start()
    {
        wordSet = new Dictionary<string, string>
        {
            { "cat", "Animals" },
            { "dog", "Animals" },
            { "computer", "Devices" },
            { "mobile phone", "Devices" }
        };
    }

}
