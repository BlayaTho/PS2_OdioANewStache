using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public Player player;
    public FlyAndDash flyDash;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] GameObject itself;
    private bool pass = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pass == false && flyDash.is_flying == true)
        {
            flyDash.timerStamina += 2f;
            flyDash.currentStamina += 2;

            Boost();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
            sr.enabled = false;
            pass = true;
    }

    public void Boost()
    {
        //StopCoroutine(NoBoost());
        //timerstop = 1.8f;
        if(player.moveSpeed_horizontal < 1600)
        {
            player.moveSpeed_vertical = flyDash.speedVertFly + 500;
            player.moveSpeed_horizontal = flyDash.speedHoriFly + 500;
        }
    }

    
    /* public void NoBoost()
     {

         player.moveSpeed_vertical = flyDash.speedVertFly;
         player.moveSpeed_horizontal = flyDash.speedHoriFly;

     }*/
}
