using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliotManager : MonoBehaviour
{
    [SerializeField] GameObject playerRef;
    Vector3 refVelocity = Vector3.zero;
    [SerializeField] float smoothTime;




    void Update()
    {
        Vector3 targetPosition = new Vector3(playerRef.transform.position.x, playerRef.transform.position.y, -4);
        gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, targetPosition, ref refVelocity, smoothTime);
    }
}
