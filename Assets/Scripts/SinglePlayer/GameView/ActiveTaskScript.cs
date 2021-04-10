using UnityEngine;

public class ActiveTaskScript : MonoBehaviour
{

    public bool isWon;
    public Material MaterialDefault;
    public Material MaterialActive;

    void Update()
    {
        SetMaterial();
        IsTaskActive();
        IsGameWon();
    }

    void SetMaterial()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position , 2f, LayerMask.GetMask("Character"));

        if (colliders.Length > 0 && !isWon)
        {
            GetComponent<SpriteRenderer>().material = MaterialActive;
        }
        else
        {
            GetComponent<SpriteRenderer>().material = MaterialDefault;
        }
    }

    private void IsTaskActive()
    {
        if (isWon)
            gameObject.layer = LayerMask.NameToLayer("FinishedTask");
    }

    private void IsGameWon()
    {
        if(GameManager._instance.tasksDone == GameManager._instance.tasksCount)
            SceneLoader.LoadTaskScene("ReportScene");
    }
}
