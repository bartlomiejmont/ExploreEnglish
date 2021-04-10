using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWonPanel : MonoBehaviour
{
    public GameObject spectatePanel;
    public Text textWin = null;
    
    private NetworkManagerLobby lobby;

    private NetworkManagerLobby Lobby
    {
        get
        {
            if (lobby != null) { return lobby; }

            return lobby = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public void ShowGameWonPopup(string pName)
    {

        var gameOverParent = GameObject.FindGameObjectWithTag("GameOverParent");
        var gameOverScript = gameOverParent.GetComponent<GameOverScript>();
        
        if (true)
        {
            if (gameOverScript.playersWon.Count == 0)
            {
                textWin.text = $"Congratulations {pName} you win !!!";
            }
            else
            {
                textWin.text = "Congratulations you get 2nd place !!!";
            }
            spectatePanel.SetActive(true);
        }
    }

    //TODO show ending scene
    public void CloseGameWonPopupAndGoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            player.GetComponent<PlayerControllerMirror>().ExitTheGame();
        }
    }

}
