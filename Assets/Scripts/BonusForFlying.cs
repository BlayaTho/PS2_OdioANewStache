using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BonusForFlying : MonoBehaviour
{
    public Player player;
    public FlyAndDash flyDash;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] GameObject itself;
    public bool pass = false;


    void Update()
    {
        if(flyDash.timerStamina > 4 || flyDash.currentStamina > 4)
        {
            flyDash.timerStamina = 4;
            flyDash.currentStamina = 4;
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(pass == false)
        {
            flyDash.timerStamina += 3;
            flyDash.currentStamina += 3;
        }
        
        
        
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        sr.enabled = false;
        
    }
}
