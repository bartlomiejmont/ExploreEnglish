using UnityEngine;

public class CrosshairMovement : MonoBehaviour
{
    private Vector3 target;
    public GameObject crosshair;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        target = transform.GetComponent<Camera>()
            .ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        crosshair.transform.position = new Vector2(target.x, target.y);
    }

}
