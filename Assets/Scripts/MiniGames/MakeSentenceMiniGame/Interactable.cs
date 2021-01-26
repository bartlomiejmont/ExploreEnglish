using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject highlight;
    private GameObject validator;


    void Start()
    {
        validator = GameObject.FindWithTag("Validation");
    }

    private void OnEnable()
    {
        highlight = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "WordFrame")
        {
            highlight = transform.GetChild(0).gameObject;
            highlight.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "WordFrame")
        {
            RemoveDefaultHighlight();
            RemoveHighlight();
        }
    }

    public void SetCorrectHighlight()
    {
        highlight = transform.GetChild(1).gameObject;
        highlight.SetActive(true);
        Debug.Log("GOOD LIGHT");
    }

    public void SetWrongHighlight()
    {
        highlight = transform.GetChild(2).gameObject;
        highlight.SetActive(true);
    }

    public void RemoveDefaultHighlight()
    {
        highlight = transform.GetChild(0).gameObject;
        highlight.SetActive(false);
    }

    public void RemoveHighlight()
    {
        highlight = transform.GetChild(2).gameObject;
        highlight.SetActive(false);
        highlight = transform.GetChild(1).gameObject;
        highlight.SetActive(false);
    }
}
