using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CheckWord : MonoBehaviour
{
    [SerializeField]
    private Text text1;

    [SerializeField]
    private Text text2;

    [SerializeField]
    private Text text3;

    [SerializeField]
    private Text text4;

    [SerializeField]
    private Text text5;

    [SerializeField]
    private Text text6;

    [SerializeField]
    private Text text7;

    [SerializeField]
    private Text text8;

    [SerializeField]
    private Text text9;

    [SerializeField]
    private Text text10;

    [SerializeField]
    private Text text11;

    private int index;

    private HashSet<int> usedIndexes;

    private int score = 0;

    private static Dictionary<string, string> diction1;

    public GameObject EndGamePopUp;

    private float _time;

    private void Start()
    {
        usedIndexes = new HashSet<int>();
        diction1 = WordsContainer.GetAllPairsForPresentPast();
        Debug.Log(diction1.Keys.ElementAt(0));
        text7.text = SetCurrentWord();
        text8.text = SetCurrentWord();
        text9.text = SetCurrentWord();
        text10.text = SetCurrentWord();
        text11.text = SetCurrentWord();
        _time = Time.time + 1;

    }


    private string SetCurrentWord()
    {
        string key = diction1.Keys.ElementAt(GetRandomIndex());

        return key;
    }


    private int GetRandomIndex()
    {
        var range = Enumerable.Range(0, diction1.Count).Where(i => !usedIndexes.Contains(i));
        var rand = new System.Random();
        int index = rand.Next(0, diction1.Count - usedIndexes.Count);
        index = range.ElementAt(index);
        usedIndexes.Add(index);

        return index;
    }

    private string GetDictionaryValue(string word)
    {
        string word1;
        diction1.TryGetValue(word, out word1);
        return word1;
    }

    public void checkWord()
    {

        int s = 0;

        string[] a = new string[5] { text1.text, text2.text, text3.text, text4.text, text5.text };
        string[] b = new string[5] { text7.text, text8.text, text9.text, text10.text, text11.text };
        Color[] c = new Color[5] { text1.color, text2.color, text3.color, text4.color, text5.color, };

        for (int i = 0; i < 5; i++)
        {
            if (a[i] == GetDictionaryValue(b[i]))
            {
                s++;
                c[i] = Color.green;
            };
        }
        score = s;
        text6.text = "Ilosc zdobytych punktow to: " + score + "/5.";


        GameOver();
        
    }

    public void PlayAgain()
    {
        if (EndGamePopUp != null)
        {
            EndGamePopUp.SetActive(false);
            score = 0;
        }
    }

    public static void MyDelay(int seconds)
    {
        TimeSpan ts = DateTime.Now.TimeOfDay + TimeSpan.FromSeconds(seconds);

        do { } while (DateTime.Now.TimeOfDay < ts);
    }

    public void GameOver()
    {

        if (score == 5)
        {
            SaveTaskData();
            SceneLoader.LoadMainScene();
        }

    }

    private void SaveTaskData()
    {
        GameManager._instance.tasksReport.verbsTaskTimer = (float)Math.Round(Time.time - _time,2);
        GameManager._instance.AssignTasks("Past Tense");

        GameManager._instance.AssignTasks(GameManager._instance.tasksReport.verbsTaskTimer.ToString());
        GameManager._instance.AssignTasks(GameManager._instance.tasksReport.verbsTaskErrors.ToString());

        GameManager._instance.tasksReport.isVerbsTaskActive = true;
        GameManager._instance.tasksReport.isVerbsTaskFinished = true;

    }

    public void CloseMiniGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}