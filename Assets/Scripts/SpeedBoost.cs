using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public Player player;
    public FlyAndDash flyDash;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] GameObject itself;
    public bool speedActiv = false;
    public bool pass = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pass == false)
        {
            
            flyDash.timerStamina += 2f;
            flyDash.currentStamina += 2;

            Boost();

            //Invoke("StopBoost", 1.8f);
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
            sr.enabled = false;
        pass = true;
    }

    public void Boost()
    {
        player.moveSpeed_vertical = flyDash.speedVertFly + 500;
        player.moveSpeed_horizontal = flyDash.speedHoriFly + 500;
        speedActiv = true;
    }
    public void StopBoost()
    {
        player.moveSpeed_vertical = flyDash.speedVertFly;
        player.moveSpeed_horizontal = flyDash.speedHoriFly;
        speedActiv = false;
    }
}
