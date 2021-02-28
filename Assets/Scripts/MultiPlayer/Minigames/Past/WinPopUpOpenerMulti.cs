using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPopUpOpenerMulti : MonoBehaviour
{


    public GameObject EndGamePopUp;


    public void OpenEndGamePopUp()
    {
        if(EndGamePopUp != null)
        {
            bool isActive = EndGamePopUp.activeSelf;

            EndGamePopUp.SetActive(!isActive);
        }
    }
}
