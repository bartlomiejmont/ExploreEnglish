using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleControllerMulti : MonoBehaviour
{
    private TextMesh mText;        
    private RaycastHit rayHit;
    Ray ray;
    private bool isActive;



    void Start()
    {
        isActive = true;
        mText = this.GetComponent<TextMesh>();
    }


    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        transform.localScale += new Vector3(0.05f, 0.05f, 0.05f) * Time.deltaTime;
        if (transform.localScale.x >= 0.3f)
        {
            BobblesGenerator.Bobbles.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (isActive)
        {
            BobblesGenerator.lastClickedLetter = mText.text;
            BobblesGenerator.OnBobbleClicked();
        }
        isActive = false;
        BobblesGenerator.Bobbles.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}

