using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class WordFrameController : MonoBehaviour
{

    public Text word;
    public int wordFrameId;
    public bool isLocked = false;
    public static int counter;

    private static Dictionary<string, List<Tuple<string, int>>> words;
    private static GameObject[] holders;
    private static GameObject[] frames;
    private GameObject frameIAmAccessing;
    private Vector2 mousePosition;
    private Vector2 initialPosition;
    private float deltaX, deltaY;
    private bool isEmpty = false;

    void Start()
    {
        holders = GameObject.FindGameObjectsWithTag("WordObjectHolder");
        frames = GameObject.FindGameObjectsWithTag("WordFrame");
        initialPosition = transform.position;
        words = DictionarySentenceSetup.wordSet;
        word.text = SetCurrentWord();
        counter++;
        wordFrameId = counter - 1;
    }

    private void OnMouseDown()
    {
        if (isLocked)
            return;
        Debug.Log("click");
        deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
    }

    private void OnMouseDrag()
    {
        if (isLocked)
            return;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(mousePosition.x - deltaX, mousePosition.y - deltaY);
    }

    private void OnMouseUp()
    {
        if (isLocked)
            return;
        CheckAllHolders();
    }

    private string SetCurrentWord()
    {
        Random random = new Random();
        int index = random.Next(words.Count);
        string key = words.ElementAt(index).Value[counter].Item1;
        return key;
    }

    private void CheckAllHolders()
    {
        isEmpty = false;
        frameIAmAccessing = gameObject;

        foreach (var holder in holders)
        {
            // Debug.Log("FRAME POSITION -> x : " + transform.position.x + ", y : " + transform.position.y);
            // Debug.Log("HOLDER POSITION -> x : " + holder.transform.position.x + ", y : " + holder.transform.position.y);
            if (Mathf.Abs(transform.position.x - holder.transform.position.x) <= 30f &&
                Mathf.Abs(transform.position.y - holder.transform.position.y) <= 30f)
            {
                foreach (var frame in frames)
                {
                    if (frame == frameIAmAccessing)
                    {
                        continue;
                    }
                        
                    
                    if (Mathf.Abs(frame.transform.position.x - holder.transform.position.x) <= 30f &&
                        Mathf.Abs(frame.transform.position.y - holder.transform.position.y) <= 30f)
                    {
                        isEmpty = true;
                        break;
                    }
                }
                
                if(isEmpty)
                    continue;
                
                transform.position = new Vector2(holder.transform.position.x, holder.transform.position.y);
                return;
            }
        }
        
        transform.position = new Vector2(initialPosition.x, initialPosition.y);
    }

}
