using UnityEngine;
using UnityEngine.UI;

public class SavePlayerUsername : MonoBehaviour
{
    private string playerName;
    public Text text;
    public InputField inputField;

    void Start()
    {
        PlayerPrefs.SetString("Difficulty", "Easy");
        inputField.text = PlayerPrefs.GetString("Username");
    }

    public void Save()
    {
        playerName = text.text;
        PlayerPrefs.SetString("Username", playerName);
        SceneLoader.LoadMainScene();
    }
}
