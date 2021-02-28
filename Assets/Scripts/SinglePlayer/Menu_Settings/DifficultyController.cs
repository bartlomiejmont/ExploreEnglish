using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DifficultyController : MonoBehaviour
{
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    private GameObject currentSelected;

    void Start()
    {
        easyButton.Select();
    }

    void Update()
    {
        currentSelected = EventSystem.current.currentSelectedGameObject;
        if (easyButton.name == currentSelected.name)
        {
            ClearColor(mediumButton);
            ClearColor(hardButton);
            SetStartingColor(easyButton);
        }

        if (mediumButton.name == currentSelected.name)
        {
            ClearColor(easyButton);
            ClearColor(hardButton);
            SetStartingColor(mediumButton);
        }
           
        if(hardButton.name == currentSelected.name)
        {
            ClearColor(easyButton);
            ClearColor(mediumButton);
            SetStartingColor(hardButton);
        }
    }

    private void SetStartingColor(Button button)
    {
        Color color = new Color(121, 129, 159, 255);
        var colors = button.colors;
        colors.selectedColor = Color.blue;
        colors.normalColor = Color.blue;
        colors.pressedColor = Color.blue;
        button.colors = colors;
    }

    private void ClearColor(Button button)
    {
        var colors = button.colors;
        colors.selectedColor = Color.white;
        colors.normalColor = Color.white;
        colors.pressedColor = Color.white;
        button.colors = colors;
    }

    public void SetEasyDifficulty()
    {
        PlayerPrefs.SetString("Difficulty", "Easy");
    }

    public void SetMediumDifficulty()
    {
        PlayerPrefs.SetString("Difficulty", "Medium");
    }

    public void SetHardDifficulty()
    {
        PlayerPrefs.SetString("Difficulty", "Hard");
    }

}
