using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WING : MonoBehaviour
{
    public FlyAndDash flyDash;
    public Image imag;

    
    void Update()
    {
        
        if(flyDash.canDash == true )
        {
            imag.enabled = true;
        }
        else  imag.enabled = false;
    }
}
