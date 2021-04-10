using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    public Text progressBarText;
    public Image mask;
    public int maximum;
    public int current;
    private int taskCount = 4;
    private float waitTime = 1f;
    private float timer;

    void Start()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in players)
        {
            if (player.GetComponent<NetworkIdentity>().hasAuthority)
            {
                current = player.GetComponent<PlayerControllerMirror>().tasksDone;
            }
        }
        maximum = taskCount;
    }

    void Update()
    {
        GetCurrentFill();
        progressBarText.text = " " + current + " / " + taskCount;
    }
    
    private void GetCurrentFill()
    {
        if (current == 0)
            mask.fillAmount = 0f;
        else
        {
            mask.fillAmount = current / (float)taskCount; 
        }
    }
    
    

}
