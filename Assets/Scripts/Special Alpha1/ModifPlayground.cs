using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifPlayground : MonoBehaviour
{
    public GameObject murBlock;
    public GameObject murBlock2;
    public GameObject murBlock3;
    public GameObject murBlock4;


    public GameObject boostvit;
    public GameObject boostvit2;
    public GameObject boostvit3;
    public GameObject booststamina;
    public GameObject booststamina2;
    public GameObject booststamina3;
    public GameObject booststamina4;
    public GameObject booststamina5;

    public GameObject zoomCam;
    public GameObject aliot;
    public GameObject aliot2;
    public GameObject aliot3;
    public GameObject aliot4;
    public GameObject aliot5;
    public GameObject aliot6;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Destroy(murBlock);
        murBlock.gameObject.SetActive(false);
        murBlock2.gameObject.SetActive(false);
        murBlock3.gameObject.SetActive(false);
        murBlock4.gameObject.SetActive(false);
        zoomCam.gameObject.SetActive(false);
        

        boostvit.gameObject.SetActive(true);
        boostvit2.gameObject.SetActive(true);
        boostvit3.gameObject.SetActive(true);
        booststamina.gameObject.SetActive(true);
        booststamina2.gameObject.SetActive(true);
        booststamina3.gameObject.SetActive(true);
        booststamina4.gameObject.SetActive(true);
        booststamina5.gameObject.SetActive(true);
        aliot.gameObject.SetActive(true);
        aliot2.gameObject.SetActive(true);
        aliot3.gameObject.SetActive(true);  
        aliot4.gameObject.SetActive(true);
        aliot5.gameObject.SetActive(true);
        aliot6.gameObject.SetActive(true);
    }
}
