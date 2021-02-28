using Cinemachine;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed = 5.0f;
    public Text username;

    private Animator animator;
    private Rigidbody2D rigidBody2D;
    private Vector2 lookDirection;
    private float horizontal;
    private float vertical;
    private CinemachineVirtualCamera vcam;

    void Awake()
    {
        
    }

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lookDirection = new Vector2(1, 0);
        username.text = PlayerPrefs.GetString("Username");
        GameManager._instance.LoadPlayerPosition();
        UpdateTasks();
    }

    void Update()
    {
        OpenTask();
        HandleInput();
        HandleAnimation();

        if (Input.GetKeyDown(KeyCode.Z))
            SceneLoader.LoadTaskScene("ReportScene");
    }

    void FixedUpdate()
    {
        CharacterMovement();
    }

    private void UpdateTasks()
    {
        if (GameManager._instance.tasksReport.isShootingTaskActive)  // TO DO -> move this to another script
            GameObject.Find("ShootingTask").GetComponent<ActiveTaskScript>().isWon = true;
        if (GameManager._instance.tasksReport.isTranslateSentenceTaskActive)  // TO DO -> move this to another script
            GameObject.Find("MakeSentenceTask").GetComponent<ActiveTaskScript>().isWon = true;
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
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
       
        if (!Mathf.Approximately(horizontal, 0.0f) || !Mathf.Approximately(vertical, 0.0f))
        {
            lookDirection.Set(horizontal, vertical);
            lookDirection.Normalize();
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
