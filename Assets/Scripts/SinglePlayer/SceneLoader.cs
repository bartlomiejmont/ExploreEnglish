using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{

    public static void LoadMainScene()
    {
        SceneManager.LoadScene("GameScene");
    }


    public static void LoadTaskScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
