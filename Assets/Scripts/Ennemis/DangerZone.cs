using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    public Player player;
    public bool hurt = false;
    public float horizontalinverse;
    public float verticalinverse;

    private void Update()
    {
        

        if (hurt)
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
                horizontalinverse = horizontalinverse * -1f;
                verticalinverse = verticalinverse * -1f;
                player.rb.AddForce(new Vector2(horizontalinverse * 70, verticalinverse * 70), ForceMode2D.Impulse);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        horizontalinverse = player.horizontal_value;
        verticalinverse = player.vertical_value;
        player.TakeDamage(1);
        hurt = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hurt = false;
    }
}
