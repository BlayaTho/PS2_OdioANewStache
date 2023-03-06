using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    public Camera camerazoom;
    public GameObject script;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        camerazoom.orthographicSize -= 4f;
        camerazoom.transform.position = new Vector2(0, 50);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        camerazoom.orthographicSize += 4f;
        script.gameObject.SetActive(false);

    }
}
