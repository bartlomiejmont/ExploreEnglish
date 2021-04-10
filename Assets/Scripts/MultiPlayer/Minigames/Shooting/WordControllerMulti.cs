using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Random = System.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class WordControllerMulti : MonoBehaviour
{
    public float lifeTime = 8f;
    public float speed = 5f;
    public Text word;
    public static int errors;
    private float currentLifeTime;
    private float waitTime = 1f;
    private float timer;
    private string currentCategory;
    private static Dictionary<string, string> words;
    private Vector3 screenCenter;
    private Vector3 movementVector = Vector3.zero;


    void Start()
    {
        Cursor.visible = false;
        screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        words = WordsContainer.GetAllPairsForShootingGame();
        currentCategory = DrawTaskCategoryMulti.GetCurrentCategory();
        word.text = SetCurrentWord();
        SetWordRotation();
        movementVector = (screenCenter - transform.position).normalized * speed;
        movementVector.z = 0;
        currentLifeTime = lifeTime;
        timer = waitTime;
    }

    void Update()
    {
        transform.position += movementVector * Time.deltaTime;
        CheckCollision();
        GameOver();

        if (currentLifeTime <= 0)
        {
            Destroy(gameObject);
            currentLifeTime = lifeTime;
        }

        currentLifeTime -= Time.deltaTime;
    }

    private void CheckCollision()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(gameObject.GetComponent<Rigidbody2D>().position,
            new Vector2(1.55f, 0.5f), 0, LayerMask.GetMask("Crosshair"));

        if (colliders.Length > 0 && Input.GetMouseButtonDown(0))
        {
            Destroy(gameObject);
            SetScore.wordCount++;

            if (!CheckIfDestroyedWordWasCorrect())
            {
                ReloadScene();
            }
        }
    }

    private void SetWordRotation()
    {
        Vector3 difference = screenCenter - transform.position;
        var rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if (rotationZ > 90)
        {
            rotationZ = -180 + rotationZ;
        }
        else if (rotationZ < -90)
        {
            rotationZ = 180 + rotationZ;
        }

        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }

    private bool CheckIfDestroyedWordWasCorrect()
    {
        string entry;
        if (words.TryGetValue(word.text, out entry))
        {
            Debug.Log("1: " + entry + " 2: " + currentCategory);
            if (entry == currentCategory)
                return true;
        }

        return false;
    }

    private string SetCurrentWord()
    {
        Random random = new Random();
        int index = random.Next(words.Count);
        string key = words.Keys.ElementAt(index);

        return key;
    }

    private void ReloadScene()
    {
        errors++;
        SetScore.wordCount = 0;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void GameOver()
    {
        if (SetScore.wordCount >= SetScore.maxScore)
        {
            GameObject[] gameObjects;
            gameObjects = GameObject.FindGameObjectsWithTag("WordObject");

            foreach (var word in gameObjects)
            {
                Destroy(word);
            }

            var players = GameObject.FindGameObjectsWithTag("Player");

            foreach (var player in players)
            {
                if (player.GetComponent<NetworkIdentity>().hasAuthority)
                {
                    player.GetComponent<PlayerControllerMirror>().miniGamesState["ShootingTaskMultiScene"] = true;
                }
            }

            DrawTaskCategoryMulti.ShowWinText();
            WordSpawnMulti game = Camera.main.GetComponent<WordSpawnMulti>();
            //SaveTaskData();
            //MakeTaskInactive();
            game.enabled = false;
            SetScore.wordCount = 0;
            Cursor.visible = true;
        }
    }

    private void SaveTaskData()
    {
        GameManager._instance.tasksReport.shootingTaskTimer = Mathf.Round(Camera.main.GetComponent<WordSpawnMulti>().taskTimer * 100f) / 100f;
        GameManager._instance.tasksReport.shootingTaskErrors = errors;
        GameManager._instance.AssignTasks("SHOOTING TASK");
        GameManager._instance.AssignTasks(GameManager._instance.tasksReport.shootingTaskTimer.ToString());
        GameManager._instance.AssignTasks(GameManager._instance.tasksReport.shootingTaskErrors.ToString());
        GameManager._instance.tasksReport.isShootingTaskFinished = true;
        Cursor.visible = true;
    }

    private void MakeTaskInactive()
    {
        GameManager._instance.tasksReport.isShootingTaskActive = true;
    }

}