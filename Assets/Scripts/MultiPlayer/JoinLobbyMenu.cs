using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JoinLobbyMenu : MonoBehaviour
{

    private NetworkManagerLobby networkManager = null;

    [Header("UI")] 
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private InputField ipAddressInputField = null;
    [SerializeField] private Button joinButton = null;

 
    private void OnEnable()
    {
        NetworkManagerLobby.OnClientConnected += HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        NetworkManagerLobby.OnClientConnected += HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
    }

    public void JoinLobby()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManagerLobby>();
        Debug.Log(networkManager);
        string ipAddress = ipAddressInputField.text;
        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;
        //gameObject.SetActive(false); TO DO - Make it change it in another script?
        //landingPagePanel.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}
