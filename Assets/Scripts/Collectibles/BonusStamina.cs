using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BonusStamina : MonoBehaviour
{
    public Player player;
    public FlyAndDash flyDash;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] GameObject itself;
    private bool pass = false;


    void Update()
    {
        //correction des valeurs en cas de hauteur
        if(flyDash.timerStamina > 4 || flyDash.currentStamina > 4)
        {
            flyDash.timerStamina = 4;
            flyDash.currentStamina = 4;
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //si c'est la premiere fois qu'on le touche alors augmentation de la stamina
        if(pass == false)
        {
            flyDash.timerStamina += 3;
            flyDash.currentStamina += 3;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //booster en invisible
        sr.enabled = false;
        pass = true;
        
    }
}
