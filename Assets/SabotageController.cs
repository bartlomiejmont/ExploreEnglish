using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = System.Random;

public class SabotageController : MonoBehaviour
{
    public static string firstTaskSabotage;
    public GameObject[] tasksNames;
    private static int created;
    public bool isSabotageBeingUsed = false;

    private NetworkManagerLobby lobby;

    private NetworkManagerLobby Lobby
    {
        get
        {
            if (lobby != null) { return lobby; }

            return lobby = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    void Awake()
    {
        Random rand = new Random();
        if (created == 1) { return; }
        DontDestroyOnLoad(gameObject);
        tasksNames = GameObject.FindGameObjectsWithTag("Task");
        firstTaskSabotage = tasksNames[rand.Next(0, tasksNames.Length)].name;
        Debug.Log(firstTaskSabotage);
        created++;
    }

    /*void HandlePlayerWonGame(int playerWonNumber, PlayerControllerMirror player)
    {
        isSabotageBeingUsed = true;
        winText.text += player.username.text + $" has earned {playersWon.Count} place !!! \n";

        if (Lobby.GamePlayers.Count < 4)
        {
            winText.text = player.username.text + " is a winner !!!";
            gameOver.gameObject.SetActive(true);
        }
        else if (playersWon.Count >= 3)
        {
            gameOver.gameObject.SetActive(true);
        }
    }
    */
}
