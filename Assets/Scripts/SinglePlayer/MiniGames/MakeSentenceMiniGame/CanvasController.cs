using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public Text sentenceText;
    private static Text winText;

    void Start()
    {
        sentenceText.text = DictionarySentenceSetup.wordSet.ElementAt(0).Key;
        winText = GameObject.FindGameObjectWithTag("WinText").GetComponent<Text>();
    }

    public static void SetWinText()
    {
        winText.text = "YOU WON";
    }

    public void SwitchBackToMainScene()
    {
        WordFrameController.counter = 0;
        SceneLoader.LoadMainScene();
    }
}
