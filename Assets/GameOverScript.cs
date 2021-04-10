using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{

    public Text winText = null;
    public Canvas gameOver = null;
    public List<PlayerControllerMirror> playersWon;
    private static int created;
    
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
        DontDestroyOnLoad(gameObject);
        created++;

        if (created != 1)
        {
            Destroy(gameObject);
        }

        playersWon = new List<PlayerControllerMirror>();
        winText.text = "";
        foreach (var playerControllerMirror in Lobby.GamePlayers)
        {
            playerControllerMirror.ClientOnGameWon += HandlePlayerWonGame;
        }
    }

    private void OnDestroy()
    {
        foreach (var playerControllerMirror in Lobby.GamePlayers)
        {
            playerControllerMirror.ClientOnGameWon -= HandlePlayerWonGame;
        }

        created = 0;
    }

    void HandlePlayerWonGame(int playerWonNumber, PlayerControllerMirror player)
    {
        playersWon.Add(player);
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

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            player.GetComponent<PlayerControllerMirror>().ExitTheGame();
        }
    }
}
