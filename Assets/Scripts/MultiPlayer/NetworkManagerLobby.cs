using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerLobby : NetworkManager
{
    [Header("Scene")]
    [Scene] 
    [SerializeField] private string menuScene = string.Empty;

    [Header("Lobby")] 
    [SerializeField] private NetworkPlayerInLobby lobbyPlayerPrefab = null;

    [Header("Game")]
    [SerializeField] private PlayerControllerMirror gamePlayerPrefab = null;

    [SerializeField] private int minPlayers = 2;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

    public List<NetworkPlayerInLobby> LobbyPlayers { get; } = new List<NetworkPlayerInLobby>();
    public List<PlayerControllerMirror> GamePlayers { get; } = new List<PlayerControllerMirror>();
    
    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();


    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");
        foreach (var prefab in spawnablePrefabs)
        {
            ClientScene.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect(NetworkConnection connection)
    {
        base.OnClientConnect(connection);
        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection connection)
    {
        base.OnClientDisconnect(connection);
        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnection connection)
    {
        if (numPlayers > maxConnections)
        {
            connection.Disconnect();
            return;
        }

        if (("Assets/Scenes/Multi/" + SceneManager.GetActiveScene().name + ".unity") != menuScene)
        {
            connection.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnection connection)
    {
        if (("Assets/Scenes/Multi/" + SceneManager.GetActiveScene().name + ".unity").Equals(menuScene))
        {
            Debug.Log("DODANO + " + LobbyPlayers.Count);
            bool isLeader = LobbyPlayers.Count == 0;
            NetworkPlayerInLobby lobbyPlayerInstance = Instantiate(lobbyPlayerPrefab);
            lobbyPlayerInstance.IsLeader = isLeader;
            NetworkServer.AddPlayerForConnection(connection, lobbyPlayerInstance.gameObject);
        }
    }

    public override void OnServerDisconnect(NetworkConnection connection)
    {
        if (connection.identity != null)
        {
            var player = connection.identity.GetComponent<NetworkPlayerInLobby>();
            LobbyPlayers.Remove(player);
            NotifyPlayersOfReadyState();
        }

        base.OnServerDisconnect(connection);
    }

    public override void OnStopServer()
    {
        LobbyPlayers.Clear();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in LobbyPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if(numPlayers < minPlayers) { return false; }

        foreach (var player in LobbyPlayers)
        {
            if (!player.IsReady) { return false; }
        }

        return true;
    }

    public void StartGame()
    {
        if (("Assets/Scenes/Multi/" + SceneManager.GetActiveScene().name + ".unity") == menuScene)
        {
            if (!IsReadyToStart()) { return; }
            ServerChangeScene("GameSceneMultiplayer");
        }
    }

    public override void ServerChangeScene(string sceneName)
    {
        float x = 0;
        float y = -12.5f;

        if (("Assets/Scenes/Multi/" + SceneManager.GetActiveScene().name + ".unity") == menuScene &&
            sceneName == "GameSceneMultiplayer")
        {
            for (int i = LobbyPlayers.Count - 1; i >= 0; i--)
            {
                Vector3 playerPosition = GetSpawnPosition(i, x, y);
                var connection = LobbyPlayers[i].connectionToClient;
                var playerInstance = Instantiate(gamePlayerPrefab , playerPosition, Quaternion.identity);
                playerInstance.usernameString = connection.identity.gameObject.GetComponent<NetworkPlayerInLobby>().DisplayName; // get current player in lobby object nickname

                NetworkServer.Destroy((connection.identity.gameObject));
                NetworkServer.ReplacePlayerForConnection(connection, playerInstance.gameObject, true);
            }
        }
        
        base.ServerChangeScene(sceneName);
    }

    public Vector3 GetSpawnPosition(int i, float x, float y)
    {
        if (i == 0 || i == 1)
        {
            x -= 2;
        }
        else if (i == 4 || i == 5)
        {
            x += 2;
        }
        else if (i == 2 || i == 3)
        {
            y -= 2;
        }
        else if (i == 6 || i == 7)
        {
            y += 2;
        }

        return new Vector3(x, y, 0);
    }
}
