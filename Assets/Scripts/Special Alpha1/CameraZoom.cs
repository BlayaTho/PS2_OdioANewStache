using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    public Camera camerazoom;
    public GameObject script;

    float CameraZoomValue = 8;
    float CameraDezoomValue = 18;
    bool dezooming = false;
    float lerpTime=0;
    float zoomTime = 0.5f;

    private void Start()
    {
    }
    private void Update()
    {

        Debug.Log(lerpTime);
        if (dezooming)
        {
            lerpTime = Mathf.Clamp(lerpTime + (Time.deltaTime * zoomTime), 0, 1);
            //camerazoom.transform.position = Vector3.Lerp(startingPosition, new Vector2(0, 50), Time.deltaTime);
        }
        else
        {
            lerpTime = Mathf.Clamp(lerpTime - (Time.deltaTime * zoomTime), 0, 1);
        }
        camerazoom.orthographicSize = Mathf.SmoothStep(CameraZoomValue, CameraDezoomValue, lerpTime);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //startingPosition = camerazoom.transform.position;
        if(dezooming) dezooming = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!dezooming) dezooming = true;
        //script.gameObject.SetActive(false);
    }
}
