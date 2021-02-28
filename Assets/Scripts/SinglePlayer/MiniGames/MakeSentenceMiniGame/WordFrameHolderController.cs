using UnityEngine;

public class WordFrameHolderController : MonoBehaviour
{

    public int wordFrameHolderId;
    private static int counter = 0;

    void Start()
    {
        counter++;
        wordFrameHolderId = counter - 1;
    }

}
