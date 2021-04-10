using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : NetworkBehaviour
{

    [Header("UI")] 
    [SerializeField] private Text nameInputField = null;

    [SerializeField] private Button continueButton = null;
   
    public static string DisplayName { get; private set; }
    private const string PlayerPrefsName = "MultiplayerPlayerName";
    public SyncDictionary<string, int> usernamesList = new SyncDictionary<string, int>();
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
   

    [Command]
    public void GetValidatedNickname()
    {
        Debug.Log("nie zawiera" + usernamesList.Count);
        if (!usernamesList.ContainsKey(nameInputField.text))
        {
            usernamesList.Add(nameInputField.text, 1);
            DisplayName = PlayerNameInput.DisplayName;
            return;
        }

        if (usernamesList.TryGetValue(nameInputField.text, out int number))
        {
            DisplayName = nameInputField.text + number;
            usernamesList[nameInputField.text] = ++number;
        }
        Debug.Log("zawiera" + usernamesList.Count);
    }

}
