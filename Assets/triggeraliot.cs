using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggeraliot : MonoBehaviour
{
    public GameObject aliot;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        aliot.gameObject.SetActive(true);
    }
}
