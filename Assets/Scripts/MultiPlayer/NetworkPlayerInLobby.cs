using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkPlayerInLobby : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private Text[] playerNameTexts = new Text[8];
    [SerializeField] private Text[] playerReadyTexts = new Text[8];
    [SerializeField] private Button startGameButton = null;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";

    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    private bool isLeader;

    public bool IsLeader
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }

    private NetworkManagerLobby lobby;

    private NetworkManagerLobby Lobby
    {
        get
        {
            if (lobby != null) { return lobby; }

            return lobby = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput.DisplayName);
        lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Lobby.LobbyPlayers.Add(this);
        UpdateDisplay();
    }

    public override void OnStopClient()
    {
        Lobby.LobbyPlayers.Remove(this);
        UpdateDisplay();
        if(!isServer) 
            SceneManager.LoadScene("MainMenu");
    }


    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    private void UpdateDisplay()
    {

        if (!isLocalPlayer)
        {
            foreach (var player in Lobby.LobbyPlayers)
            {
                if (player.hasAuthority)
                {
                    player.UpdateDisplay();
                    player.gameObject.SetActive(false);
                    player.gameObject.SetActive(true);
                    break;
                }
            }
            return;
        }

        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting For player";
            playerReadyTexts[i].text = string.Empty;
        }

        for (int i = 0; i < Lobby.LobbyPlayers.Count; i++)
        {
            playerNameTexts[i].text = Lobby.LobbyPlayers[i].DisplayName;
            playerReadyTexts[i].text = Lobby.LobbyPlayers[i].IsReady
                ? "<color=green>Ready</color>"
                : "<color=red>Not Ready</color>";
        }
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if (!isLeader) { return; }
        startGameButton.interactable = readyToStart;
    }

    public void ExitLobby()
    {
        Lobby.LobbyPlayers.Remove(this);
        UpdateDisplay();
        SceneManager.LoadScene("MainMenu");
        GameObject manager = GameObject.FindGameObjectWithTag("NetworkManager");
        if (manager)
            Destroy(manager);

        if (isServer)
            Lobby.StopHost();
        else
            Lobby.StopClient();
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;
        Lobby.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Lobby.LobbyPlayers[0].connectionToClient != connectionToClient) { return; }
        Lobby.StartGame();
    }
}
