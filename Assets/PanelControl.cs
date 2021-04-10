using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelControl : MonoBehaviour
{

    public void OpenOrClosePanel()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
