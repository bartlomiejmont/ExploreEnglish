using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BobblesGeneratorMulti : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    public static List <GameObject> Bobbles;
    public Text textEnglish;
    public Text textPolish;
    private string toWrite;
    private float _time;
    private float timer;
    private int generationCounter;
    private KeyValuePair<string, string> randomWord;
    public static string lastClickedLetter;

    public delegate void BoobleClickAction();

    public static BoobleClickAction OnBobbleClicked;

    // Start is called before the first frame update
    void Start()
    {
        randomWord = wordGenarate();
        textEnglish.text = "";
        toWrite = randomWord.Value;
        textPolish.text = randomWord.Key;
        Bobbles = new List<GameObject>();
        _time = Time.time + 1;
        OnBobbleClicked += addLetter;
        generationCounter = 0;
        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(Time.time - _time) < 0.1)
        {
            _time = _time + Random.Range(0.5f, 2f);
            BobblesGenerate();
        } 
    }

    public void addLetter()
    {
        // Correct Letter
        if (char.Parse(lastClickedLetter) == toWrite[this.textEnglish.text.Length])
        {
            this.textEnglish.text += lastClickedLetter;
        }
        // LOSE 
        else
        {           
            GameManager._instance.tasksReport.bubbleTaskErrors ++;
            ResteGame();
        }
        // WIN 
        if (this.textEnglish.text.Length == toWrite.Length)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");

            foreach (var player in players)
            {
                if (player.GetComponent<NetworkIdentity>().hasAuthority)
                {
                    player.GetComponent<PlayerControllerMirror>().miniGamesState["BubblesTaskMultiScene"] = true;
                }
            }

            CloseMiniGame();
            SaveTaskData();
        }
    }

    private void ResteGame()
    {
        OnBobbleClicked = null;
        Start();
    }

    private void SaveTaskData()
    {
        GameManager._instance.tasksReport.bubbleTaskTimer = (float) Math.Round( Time.time - timer, 2);
        GameManager._instance.AssignTasks("Bubbles");
        GameManager._instance.AssignTasks(GameManager._instance.tasksReport.bubbleTaskTimer.ToString());
        GameManager._instance.AssignTasks(GameManager._instance.tasksReport.bubbleTaskErrors.ToString());
        
        GameManager._instance.tasksReport.isBubbleTaskActive = true;
        GameManager._instance.tasksReport.isBubbleTaskFinished = true;
    }

    void BobblesGenerate()
    {
        var bobble = Instantiate<GameObject>(prefab,new Vector3(Random.Range(-7.5f,7.5f),Random.Range(-4f,2.5f),0) ,Quaternion.identity);
        bobble = generateBobblePosition(bobble);
        TextMesh mText = bobble.GetComponent<TextMesh>();

        mText.text = generateRandomLetter();

        Bobbles.Add(bobble);
    }

    private string generateRandomLetter()
    {
        string chars = "abcdefghijklmnopqrstuvwxyz ";
        generationCounter++;
        if (generationCounter % Random.Range(2,6) == 0)
        {
            return toWrite[this.textEnglish.text.Length].ToString();
        }
        System.Random random = new System.Random();
        int num = random.Next(0, chars.Length -1);
        return chars[num].ToString();
    }

    private GameObject generateBobblePosition(GameObject bobble)
    {
        Bobbles.ForEach(b =>
        {
            float distance = Vector3.Distance(bobble.transform.position, b.transform.position);
            if (distance < 2)
            {
                bobble.transform.position = new Vector3(Random.Range(-7.5f,7.5f),Random.Range(-4f,2.5f),0);
                generateBobblePosition(bobble);
            }
        });
        return bobble;
    }

    private KeyValuePair<string, string> wordGenarate()
    {
        List<KeyValuePair<string, string>> words = WordsContainer.GetAllWordsWithTranslation();
        System.Random random = new System.Random();
        int index = random.Next(words.Count);
        return words[index];
    }

    public void CloseMiniGame()
    {
        OnBobbleClicked = null;
        SceneManager.LoadScene("GameSceneMultiplayer");
    }

}