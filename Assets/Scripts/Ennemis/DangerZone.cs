using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    public Player player;
    public FlyAndDash flyDash;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        player.TakeDamage(1);
        
        if(flyDash.is_flying == true)
        {
            flyDash.ChuteVol();
        }
    }

    #region ANCIENNE METHODE FOIREUSE
    /*  private void Update()
     {
    public float horizontalinverse;
    public float verticalinverse;
         if (hurt && flyDash.is_flying)
          {
              horizontalinverse = horizontalinverse * -1f;
              verticalinverse = verticalinverse * -1f;
              player.rb.AddForce(new Vector2(horizontalinverse * 70, verticalinverse * 70), ForceMode2D.Impulse);
          }

          /*if (hurt)
          {
              if (player.rb.velocity.y < 0 )
              {
                  player.rb.AddForce(new Vector2(0, 70), ForceMode2D.Impulse);
              }
              else if (player.rb.velocity.y > 0)
              {
                  player.rb.AddForce(new Vector2(0, -70), ForceMode2D.Impulse);
              }
              else
              {

              }
          }
      }*/
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        horizontalinverse = player.horizontal_value;
        verticalinverse = player.vertical_value;
        player.TakeDamage(1);
        hurt = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        hurt = false;
    }*/
    #endregion
}
