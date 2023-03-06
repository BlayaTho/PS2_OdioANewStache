using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeText : MonoBehaviour
{
    [SerializeField] TMP_Text text_XBOX;
    [SerializeField] TMP_Text text_PS4;
    public CheckController checkC;
    

    void Start()
    {
       
    }
    private void Update()
    {
        if (checkC.PS4_Controller == 1)
        {
            text_XBOX.enabled = false;
            text_PS4.enabled = true;
        }
        else
        {
            text_XBOX.enabled = true;
            text_PS4.enabled = false;
        }
    }

}   
