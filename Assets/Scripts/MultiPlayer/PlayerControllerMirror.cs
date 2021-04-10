using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControllerMirror : NetworkBehaviour
{
    [SyncVar] 
    public string usernameString;
    [SyncVar (hook = nameof(HandlePlayerWonGame))] 
    public int playersWon = 0;
    [SyncVar]
    public bool wasCameraShakingSabotageUsed = false;
    [SyncVar] 
    private bool wasDisableTaskSabotageUsed = false;
    [SyncVar] 
    private bool isDisableTaskSabotageBeingUsed = false;
    
    public event Action <int, PlayerControllerMirror> ClientOnGameWon;
    
    public TextMesh username;
    public string lastActiveScene = "";
    public bool CanMove { get; set; }
   
    public float speed = 5.0f;
    public int tasksDone = 0;
    public GameObject playerName;
    public GameObject[] tasksNames;
    public Dictionary<string, bool> miniGamesState = new Dictionary<string, bool>();
    private Animator animator;
    private Rigidbody2D rigidBody2D;
    private Vector2 lookDirection;
    private float horizontal;
    private float vertical;
    private float sabotageTimer = 10f;

    [Header("Scene")]
    [Scene]
    [SerializeField] private string gameScene = string.Empty;


    private NetworkManagerLobby lobby;

    private NetworkManagerLobby Lobby
    {
        get
        {
            if (lobby != null) { return lobby; }

            return lobby = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
        CanMove = true;
        playerName.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("PlayerName"); //Setting the layer of nickname to be shown on top of objects in game
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lookDirection = new Vector2(1, 0);
        username.text = usernameString;
    }

    [ClientCallback]
    void Update()
    {
       /* if (isDisableTaskSabotageBeingUsed)
        {
            sabotageTimer -= Time.deltaTime;
            if (sabotageTimer <= 0)
            {
                isDisableTaskSabotageBeingUsed = false;
            }
        }
       */

        if (!hasAuthority) { return; }
        
        OpenTask();
        HandleInput();
        UpdateTasks();
        HandleAnimation();
        DisableMovementWhileInTask(connectionToClient);

    }

    void FixedUpdate()
    {
        CharacterMovement();
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        if (IsFirstLoadOfMainScene(next))
        {
            tasksNames = GameObject.FindGameObjectsWithTag("Task");
            foreach (var t in tasksNames)
            {
                miniGamesState.Add(t.name + "Scene", false);
            }
        }

        string currentScene = lastActiveScene;
        lastActiveScene = next.name;
        string mainSceneName = next.name;

        if (currentScene == null)
        {
            currentScene = "Replaced";
        }

        foreach (var d in miniGamesState)
        {

            if (d.Key.Equals(currentScene) && mainSceneName == "GameSceneMultiplayer" && d.Value)
            {
                if (hasAuthority)
                {
                    GetNearestTask().GetComponent<ActiveTaskScriptMulti>().isWon = true;
                    tasksDone++;
                }
            }
        }

        if (tasksDone == 1)
        {
            FindObjectOfType<Camera>().GetComponent<GameWonPanel>().ShowGameWonPopup(usernameString);
            incrementPlayerWon();

            if (Lobby.GamePlayers.Count < 4)
            { 
                // GameOverForMax3Players();
            }
        }
    }

    private bool IsFirstLoadOfMainScene(Scene next)
    {
        return next.name == "GameSceneMultiplayer" && lastActiveScene == "";
    }

    public GameObject GetNearestTask()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Task");

        for (int i = 0; i < taggedObjects.Length; i++)
        {
            if (Vector3.Distance(gameObject.transform.position,
                taggedObjects[i].transform.position) <= 15f)
            {
                return taggedObjects[i];
            }
        }
        return null;
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        Lobby.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        if (hasAuthority)
        {
            Lobby.GamePlayers.Remove(this);
            SceneManager.LoadScene("MainMenu");
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerMirror(Clone)")
        {
            Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider>(), true);
        }
    }

    public void DisableMovementWhileInTask(NetworkConnection target)
    {
        if (("Assets/Scenes/Multi/" + SceneManager.GetActiveScene().name + ".unity").Equals(gameScene))
        {
            CanMove = true;
            return;
        }
        
        CanMove = false;
    }

    private void UpdateTasks()
    {
        if ("Assets/Scenes/Multi/" + SceneManager.GetActiveScene().name + ".unity" != gameScene) { return; }
    }

    private void CharacterMovement()
    {
        Vector3 movement = new Vector3(horizontal, vertical, 0.0f);
        movement = Vector3.ClampMagnitude(movement, 1);
        transform.position = transform.position + movement * (speed * Time.deltaTime);
    }

    private void HandleInput()
    {
        if(CanMove)
        { 
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
       
            if (!Mathf.Approximately(horizontal, 0.0f) || !Mathf.Approximately(vertical, 0.0f))
            {
                lookDirection.Set(horizontal, vertical);
                lookDirection.Normalize();
            }
        }
    }

    private void HandleAnimation()
    {
        animator.SetFloat("Horizontal", lookDirection.x);
        animator.SetFloat("Vertical", lookDirection.y);
        animator.SetFloat("Speed", new Vector2(horizontal, vertical).magnitude);
    }

    private void OpenTask()
    {
        if (Input.GetKeyDown(KeyCode.X) && !isDisableTaskSabotageBeingUsed)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(rigidBody2D.position , 2.0f, LayerMask.GetMask("Task"));

            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider.name.Contains("Task"))
                    {
                        SceneLoader.LoadTaskScene(collider.name + "Scene");
                        break;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Z) && miniGamesState[SabotageController.firstTaskSabotage + "Scene"]) 
        {
            UseShakingCameraSabotage();
        }

        /*if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("SABOTAGE USED");
            UseDisableTaskSabotage();
        }
        */
    }
    
    [ClientRpc]
    [Command]
    private void GameOverForMax3Players()
    {
        var gameOverModal = Resources
            .FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(g => g.CompareTag("GameOver"));
        
        Text gameOverModalText = Resources
            .FindObjectsOfTypeAll<Text>()
            .FirstOrDefault(g => g.CompareTag("WinText"));

        // gameOverModalText.text = username.text + " is a winner !!!";
        gameOverModal.SetActive(true);
    } 
    
    [ClientRpc]
    [Command]
    private void GameOverForMin4Players()
    {
        var gameOverModal = Resources
            .FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(g => g.CompareTag("GameOver"));
        
        Text gameOverModalText = Resources
            .FindObjectsOfTypeAll<Text>()
            .FirstOrDefault(g => g.CompareTag("WinText"));

        gameOverModalText.text = username.text + " is a winner !!!";
        gameOverModal.SetActive(true);
    }

    public void HandlePlayerWonGame(int oldWonNumber, int newWonNumber)
    {
        Debug.Log("Hook");
        ClientOnGameWon?.Invoke(newWonNumber,this);
    }

    [Command]
    private void incrementPlayerWon()
    {
        playersWon++;
    }

    public void ExitTheGame()
    {
        if (!hasAuthority) { return;}
        Lobby.GamePlayers.Remove(this);
        SceneManager.LoadScene("MainMenu");
        GameObject manager = GameObject.FindGameObjectWithTag("NetworkManager");

        if (manager)
            Destroy(manager);

        GameObject playerLeft = GameObject.FindGameObjectWithTag("PlayersLeft");

        if (playerLeft)
            Destroy(playerLeft);

        GameObject parent = GameObject.FindGameObjectWithTag("GameOverParent");

        if (parent)
            Destroy(parent);

        foreach (var o in GetAllDontDestroyOnLoadTasks())
        {
            Destroy(o);
        }

        if (isServer)
            Lobby.StopHost();
        else
            Lobby.StopClient();
    }

    private GameObject [] GetAllDontDestroyOnLoadTasks()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Task");

        return gameObjects;
    }

    [Command]
    [ClientRpc]
    public void UseShakingCameraSabotage()
    {
        Debug.Log("SABOTAGE1");
        if (hasAuthority || wasCameraShakingSabotageUsed) { return;}
        CameraShake.Instance.Shake(10f, 5f);
        wasCameraShakingSabotageUsed = true;
    }

    [Command]
    [ClientRpc]
    public void UseDisableTaskSabotage()
    {
        if (hasAuthority) { return; }
        Debug.Log("SABOTAŻ TASK");
        isDisableTaskSabotageBeingUsed = true;
        wasDisableTaskSabotageUsed = true;
    }
}
