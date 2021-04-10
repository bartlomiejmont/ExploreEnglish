using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerMulti : MonoBehaviour
{
    //public SyncList<string> playersNames = new SyncList<string>();
    public GameObject[] players;
    private string lastActiveScene = "";
    
    void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    void Update()
    {
        //FindPlayers();
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        if (IsFirstLoadOfMainScene(next))
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        string currentScene = lastActiveScene;
        lastActiveScene = next.name;
        string mainSceneName = next.name;

        if (currentScene == null)
        {
            currentScene = "Replaced";
        }

    }

    private bool IsFirstLoadOfMainScene(Scene next)
    {
        return next.name == "GameSceneMultiplayer" && lastActiveScene == "";
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

  /*  [Server]
    public void FindPlayers()
    {
        foreach(var player in players)
        {
            if(player.GetComponent<PlayerControllerMirror>().tasksDone == 1)
            {
                playersNames.Add(player.GetComponent<PlayerControllerMirror>().usernameString);
            }
        }
        Debug.Log(playersNames.Count);
    }
  */
}
