using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMultiplayer : MonoBehaviour
{

    [SerializeField] private NetworkManagerLobby networkManager = null;

    [Header("UI")] [SerializeField] private GameObject landingPagePanel = null;

    public void HostLobby()
    {
        networkManager.StartHost();
        landingPagePanel.SetActive(false);
    }

    public void GoToMainMenu()
    {
        SceneLoader.LoadTaskScene("MainMenu");
    }

}
