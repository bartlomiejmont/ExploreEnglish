using Cinemachine;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControllerMirror : NetworkBehaviour
{
    [SyncVar] 
    public string usernameString;
    public TextMesh username;
    public bool CanMove { get; set; }
   
    public float speed = 5.0f;
    public GameObject playerName;
    private Animator animator;
    private Rigidbody2D rigidBody2D;
    private Vector2 lookDirection;
    private float horizontal;
    private float vertical;

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
        CanMove = true;
        playerName.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("PlayerName"); //Setting the layer of nickname to be shown on top of objects in game
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lookDirection = new Vector2(1, 0);
        username.text = usernameString;
        GameManager._instance.LoadPlayerPosition();
    }

    [ClientCallback]
    void Update()
    {
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

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        Lobby.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Lobby.GamePlayers.Remove(this);
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
        if (("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity").Equals(gameScene))
        {
            CanMove = true;
            return;
        }
       
        Debug.Log("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity");
        Debug.Log(gameScene);
        CanMove = false;
    }

    private void UpdateTasks()
    {
        if ("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity" != gameScene) { return; }

        if (GameManager._instance.tasksReport.isShootingTaskActive)  // TO DO -> move this to another script
            GameObject.Find("ShootingTask").GetComponent<ActiveTaskScript>().isWon = true;
        if (GameManager._instance.tasksReport.isTranslateSentenceTaskActive)  // TO DO -> move this to another script
            GameObject.Find("MakeSentenceTaskMultiplayer").GetComponent<ActiveTaskScript>().isWon = true;
        if (GameManager._instance.tasksReport.isBubbleTaskActive)  // TO DO -> move this to another script
            GameObject.Find("BubblesTask").GetComponent<ActiveTaskScript>().isWon = true;
        if (GameManager._instance.tasksReport.isVerbsTaskActive)  // TO DO -> move this to another script
            GameObject.Find("PastTenseTask").GetComponent<ActiveTaskScript>().isWon = true;
        if (GameManager._instance.tasksReport.isTranslateSentenceTaskFinished)
        {
            GameManager._instance.UpdateProgressBar();
            GameManager._instance.tasksReport.isTranslateSentenceTaskFinished = false;
        }
        if (GameManager._instance.tasksReport.isBubbleTaskFinished)
        {
            GameManager._instance.UpdateProgressBar();
            GameManager._instance.tasksReport.isBubbleTaskFinished = false;
        }
        if (GameManager._instance.tasksReport.isVerbsTaskFinished)
        {
            GameManager._instance.UpdateProgressBar();
            GameManager._instance.tasksReport.isVerbsTaskFinished = false;
        }
        if (GameManager._instance.tasksReport.isShootingTaskFinished)
        {
            GameManager._instance.UpdateProgressBar();
            GameManager._instance.tasksReport.isShootingTaskFinished = false;
        }

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
        if (Input.GetKeyDown(KeyCode.X))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(rigidBody2D.position , 2.0f, LayerMask.GetMask("Task"));

            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider.name.Contains("Task"))
                    {
                        GameManager._instance.SavePlayerPosition();
                        SceneLoader.LoadTaskScene(collider.name + "Scene");
                        break;
                    }
                }
            }
        }
    }

}
