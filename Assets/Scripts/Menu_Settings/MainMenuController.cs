using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public AudioMixer audioMixer;
    public Text volumeValue;

    void Start()
    {
        volumeValue.text = "0";
    }

    public void StartGame()
    {
        SceneLoader.LoadTaskScene("UsernameScene");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        volumeValue.text = string.Format("{0:#.}", (volume + 20f));
    }
}
