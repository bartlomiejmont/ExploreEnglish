using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveTaskScriptMulti : NetworkBehaviour
{

    public bool isWon;
    public Material MaterialDefault;
    public Material MaterialActive;
    public string lastActiveScene = "";
    //public bool isMakeSentenceTaskFinished = false;

    private static int created = 0;

    void Awake()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
        if (created < 4)
        {
            DontDestroyOnLoad(gameObject);
            created ++;
        }
    }
    
    void Update()
    {
        IsTaskActive();
        SetMaterial();
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        if (!IsFirstLoadOfMainScene(next))
        {
            created = 4;
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

    void SetMaterial()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position , 2f, LayerMask.GetMask("Character"));

        bool currentPlayer = false;

        foreach (var c in colliders)
        {
            if(c.gameObject.GetComponent<NetworkIdentity>().hasAuthority) 
                currentPlayer = true;
        }

        if (colliders.Length > 0 && !isWon && currentPlayer)
        {
            GetComponent<SpriteRenderer>().material = MaterialActive;
        }
        else
        {
            GetComponent<SpriteRenderer>().material = MaterialDefault;
        }
    }

    void OnDestroy()
    {
        created = 0;
    }

    private void IsTaskActive()
    {
        if (isWon)
        {
            gameObject.layer = LayerMask.NameToLayer("FinishedTask");
        }
    }

}
