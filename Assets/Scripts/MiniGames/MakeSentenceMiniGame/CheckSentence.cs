using System.Linq;
using UnityEngine;

public class CheckSentence : MonoBehaviour
{
    private static GameObject[] holders;
    private static GameObject[] frames;
    private int score = 0;
    public bool isLocked;
    private int errors = 0;

    public void ValidateSentence()
    {
        ValidationSetup();
        for (int i = 0; i < holders.Length; i++)
        {
            if (Mathf.Abs(GetWordFramePositionX(i) - GetHolderPositionX(i)) <= 30f &&
                Mathf.Abs(GetWordFramePositionY(i) - GetHolderPositionY(i)) <= 30f)
            {
                if (!GetCurrentWordFrame(i).gameObject.GetComponent<WordFrameController>().isLocked)
                    score++;
                GetCurrentWordHolder(i).gameObject.GetComponent<Interactable>().RemoveHighlight();
                GetCurrentWordHolder(i).gameObject.GetComponent<Interactable>().RemoveDefaultHighlight();
                GetCurrentWordHolder(i).gameObject.GetComponent<Interactable>().SetCorrectHighlight();
                GetCurrentWordFrame(i).gameObject.GetComponent<WordFrameController>().isLocked = true;
                GameOver();
            }
            else
            {
                GetCurrentWordHolder(i).gameObject.GetComponent<Interactable>().SetWrongHighlight();
                errors++;
            }
        }
    }

    private void SortHolderById()
    {
        holders.OrderBy(o => o.gameObject.GetComponent<WordFrameHolderController>().wordFrameHolderId);
    }

    private void SortWordFrameById()
    {
        frames.OrderBy(o => o.gameObject.GetComponent<WordFrameController>().wordFrameId);
    }

    private float GetHolderPositionX(int i)
    {
        return holders.ElementAt(i).gameObject.transform.position.x;
    }

    private float GetHolderPositionY(int i)
    {
        return holders.ElementAt(i).gameObject.transform.position.y;
    }

    private float GetWordFramePositionX(int i)
    {
        return frames.ElementAt(i).gameObject.transform.position.x;
    }

    private float GetWordFramePositionY(int i)
    {
        return frames.ElementAt(i).gameObject.transform.position.y;
    }

    private GameObject GetCurrentWordHolder(int i)
    {
        return holders.ElementAt(i);
    }

    private GameObject GetCurrentWordFrame(int i)
    {
        return frames.ElementAt(i);
    }

    private void ValidationSetup()
    {
        isLocked = false;
        gameObject.SetActive(true);
        holders = GameObject.FindGameObjectsWithTag("WordObjectHolder");
        frames = GameObject.FindGameObjectsWithTag("WordFrame");
        SortHolderById();
        SortWordFrameById();
    }

    private void GameOver()
    {
        if (score >= holders.Length)
        {
            CanvasController.SetWinText();
            SaveTaskData();
            GameManager._instance.MakeTaskInactive();
            GameManager._instance.UpdateProgressBar();
        }
    }

    private void SaveTaskData()
    {
        GameManager._instance.tasksReport.translateSentenceTaskTimer = Mathf.Round(Camera.main.GetComponent<MakeSentenceController>().taskTimer * 100f) / 100f;
        GameManager._instance.tasksReport.translateSentenceTaskErrors = errors;
        GameManager._instance.AssignTasks("TRANSLATE SENTENCE TASK");
        GameManager._instance.AssignTasks(GameManager._instance.tasksReport.translateSentenceTaskTimer.ToString());
        GameManager._instance.AssignTasks(GameManager._instance.tasksReport.translateSentenceTaskErrors.ToString());
        Debug.Log(GameManager._instance.tasksReport.shootingTaskTimer);
    }
}
