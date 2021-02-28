using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MakeSentenceControllerMirror : MonoBehaviour
{

    public GameObject word;
    public GameObject wordHolder;
    private int xPosition, yPosition;
    private HashSet<int> usedIndexes;
    public float taskTimer;

    void Start()
    {
        usedIndexes = new HashSet<int>();
        SpawnWordHolders();
        SpawnWords();
    }

    void Update()
    {
        taskTimer += Time.deltaTime;
    }

    private void SpawnWords()
    {
        for (int i = 0; i < DictionarySentenceSetup.listCount; i++)
        {
            GetRandomIndex();
            Instantiate(word, new Vector2(-600 + xPosition * 300, -136), Quaternion.identity);
        }
    }

    private void SpawnWordHolders()
    {
        for (int i = 0; i < DictionarySentenceSetup.listCount; i++)
        {
            Instantiate(wordHolder, new Vector2(-600 + i * 300, 177), Quaternion.identity);
        }
    }

    private void GetRandomIndex()
    {
        var range = Enumerable.Range(0, DictionarySentenceSetup.listCount).Where(i => !usedIndexes.Contains(i));
        var rand = new System.Random();
        int index = rand.Next(0, DictionarySentenceSetup.listCount - usedIndexes.Count);
        xPosition = range.ElementAt(index);
        usedIndexes.Add(xPosition);
    }
}
