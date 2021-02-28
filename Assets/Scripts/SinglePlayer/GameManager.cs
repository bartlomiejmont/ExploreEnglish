using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance { get; private set; }
    public TasksReport tasksReport;
    public List<string> reportInformation;
    public int tasksDone;
    public int tasksCount = 4;
    

    public Vector3 playerPosition = new Vector3(0, 0, 0);
    private GameObject player;

    private void Awake()
    {

        if (_instance == null)
        {
            _instance = this;
           DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        tasksDone = 0;
        player = GameObject.FindWithTag("Player");
        tasksReport = gameObject.AddComponent<TasksReport>();
    }

    public void SavePlayerPosition()
    {
        player = GameObject.FindWithTag("Player");
        GameManager._instance.playerPosition = GameManager._instance.player.transform.position;
    }

    public void LoadPlayerPosition()
    {
        player = GameObject.FindWithTag("Player");
        GameManager._instance.player.transform.position = GameManager._instance.playerPosition;
    }

    public void AssignTasks(string taskReport)
    {
        reportInformation.Add(taskReport);
    }

    public void MakeTaskInactive()
    {
        _instance.tasksReport.isTranslateSentenceTaskActive = true;
    }

    public void UpdateProgressBar()
    {
        _instance.tasksDone += 1;
    }

    public void ClearReport()
    {
        tasksReport.isVerbsTaskActive = false;
        tasksReport.isShootingTaskActive = false;
        tasksReport.isTranslateSentenceTaskActive = false;
        tasksReport.isBubbleTaskActive = false;
        reportInformation.Clear();
     
    }

}
