using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggeraliot : MonoBehaviour
{
    public GameObject aliot;
    public GameObject aliot2;
    public GameObject aliot3;
    public GameObject aliot4;
    public GameObject aliot5;
    public GameObject aliot6;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        aliot.gameObject.SetActive(false);
        aliot2.gameObject.SetActive(false);
        aliot3.gameObject.SetActive(false);
        aliot4.gameObject.SetActive(false);
        aliot5.gameObject.SetActive(false);
        aliot6.gameObject.SetActive(false);
    }
}
