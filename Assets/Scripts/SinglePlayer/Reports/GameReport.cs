using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameReport : MonoBehaviour
{
    public Text reportText;

    void Start()
    {
        DisplayReport();
    }

    void Update()
    {
        
    }

    private void DisplayReport()
    {
        for(int i = 0; i < GameManager._instance.reportInformation.Count; i++)
        {
            
            if (i % 3 == 0)
            {
                reportText.text += GameManager._instance.reportInformation[i].ToUpper();
                reportText.fontSize = 50;
                reportText.text += "\n";
                reportText.text += "\n";
            }
            else if(i % 3 == 1)
            {
                Debug.Log(i);
                reportText.text += "Task time : " + GameManager._instance.reportInformation[i] + "s";
                reportText.fontSize = 20;
                reportText.text += "\n";
                reportText.text += "\n";
            }
            else if(i % 3 == 2 )
            { 
                reportText.text += "Errors : " + GameManager._instance.reportInformation[i];
                reportText.fontSize = 20;
                reportText.text += "\n";
                reportText.text += "\n";
            }
                   
        }
    }

    public void PlayAgain()
    {
        GameManager._instance.ClearReport();
        GameManager._instance.tasksDone = 0;
        GameManager._instance.playerPosition = new Vector3(0,0,0);
        SceneLoader.LoadTaskScene("UsernameScene");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
