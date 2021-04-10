using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HidePanel : MonoBehaviour
{

    public GameObject gameObject;

    void Start()
    {
        StartCoroutine(RemoveAfterSeconds(5, gameObject));
    }

    IEnumerator RemoveAfterSeconds(int seconds, GameObject obj)
    {
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
    }

}
