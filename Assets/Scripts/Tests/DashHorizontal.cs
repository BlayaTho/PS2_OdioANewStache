using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashHorizontal : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;

    //DASH
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;
    [SerializeField] private TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    private void Update()
    {
        if (isDashing)
        {
            return;
        }
        // Fire 2 = a la touche B
        if (Input.GetButtonDown("Fire2") && canDash)
        {
            StartCoroutine(Dash());

        }
    }
    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

    }


    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if (sr.flipX == true)
        {
            rb.velocity = new Vector2(-transform.localScale.x * dashingPower, 0f);
        }
        else
        { rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f); }

        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
