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
        ray = Camera.current.ScreenPointToRay(Input.mousePosition);
        transform.localScale += new Vector3(0.05f, 0.05f, 0.05f) * Time.deltaTime;
        if (transform.localScale.x >= 0.3f)
        {
            BobblesGeneratorMulti.Bobbles.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (isActive)
        {
            BobblesGeneratorMulti.lastClickedLetter = mText.text;
            BobblesGeneratorMulti.OnBobbleClicked();
        }
        isActive = false;
        BobblesGeneratorMulti.Bobbles.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}

