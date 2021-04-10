using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayersLeftScript : MonoBehaviour
{

    public List<Sprite> places = new List<Sprite>();
    public Image currentPlace = null;
    public int playersWonCount;
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
       
        playersWonCount = 0;
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
        playersWonCount++;
        if (playersWonCount < places.Count)
        {
            currentPlace.sprite = places[playersWonCount];
        }
    }
    
}
