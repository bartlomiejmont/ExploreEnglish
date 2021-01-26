using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetScore : MonoBehaviour
{
    public Text scoreText;
    public static int wordCount;
    public static int maxScore = 5;


    void Update()
    {
        scoreText.text = wordCount + " / " + maxScore;
    }

}
