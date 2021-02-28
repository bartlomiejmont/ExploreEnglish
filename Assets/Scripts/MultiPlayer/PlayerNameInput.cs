using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{

    [Header("UI")] 
    [SerializeField] private Text nameInputField = null;

    [SerializeField] private Button continueButton = null;
   
    public static string DisplayName { get; private set; }
    private const string PlayerPrefsName = "MultiplayerPlayerName";
    private void Start() => SetUpInputField();

    public void SetUpInputField()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsName)) { return; }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsName);
        nameInputField.text = defaultName;
        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        //continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;
        PlayerPrefs.SetString(PlayerPrefsName, DisplayName);
    }
    

}
