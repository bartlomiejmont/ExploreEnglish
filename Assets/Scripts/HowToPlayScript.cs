﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayScript : MonoBehaviour
{

    public GameObject Panel;

    
    public void ClosePanel()
    {

        if (Panel != null)
        {
            Panel.SetActive(false);
        }
    }
}
