using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckController : MonoBehaviour
{
    

    public int Xbox_One_Controller = 0;
    public int PS4_Controller = 0;

    public string inputDash = "";
    public string inputJump = "";
    public string inputVol = "";
    void Update()
    {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            print(names[x].Length);
            if (names[x].Length == 19)
            {
                print("PS4 CONTROLLER IS CONNECTED");
                PS4_Controller = 1;
                Xbox_One_Controller = 0;
            }
            if (names[x].Length == 33)
            {
                print("XBOX ONE CONTROLLER IS CONNECTED");
                //set a controller bool to true
                PS4_Controller = 0;
                Xbox_One_Controller = 1;

            }
        }


        if (Xbox_One_Controller == 1)
        {
            inputDash = "Dash_XBOX";
            inputJump = "Jump_XBOX";
            inputVol = "Vol_XBOX";
        }
        else if (PS4_Controller == 1)
        {
            inputDash = "Dash_PS4";
            inputJump = "Jump_PS4";
            inputVol = "Vol_PS4";
        }
        else
        {
            // there is no controllers
        }
    }
}
