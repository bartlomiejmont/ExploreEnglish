using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class DrawTaskCategory : MonoBehaviour
{
    public Text text;
    private static Text winText;
    private static string drawText;
    private static Dictionary<string, string> words;

    void Start()
    {
        winText = GameObject.FindGameObjectWithTag("WinText").GetComponent<Text>();
        words = WordsContainer.GetAllPairsForShootingGame();
        DrawRandomCategory();
        text.text = drawText;
    }

    private void DrawRandomCategory()
    {
        Random random = new Random();
        int index = random.Next(words.Count);
        drawText = words.Values.ElementAt(index);
    }

    public static string GetCurrentCategory()
    {
        return drawText;
    }

    public static void ShowWinText()
    {
        winText.text = "YOU WON";
        winText.gameObject.SetActive(true);
    }

    public void ChangeBackToMainScene()
    {
        SceneLoader.LoadMainScene();
    }
}
