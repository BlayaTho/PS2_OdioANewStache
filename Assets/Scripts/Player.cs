using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animController;
     float horizontal_value;
     float vertical_value;
    
    Vector2 ref_velocity = Vector2.zero;

    [Header("MOVEMENT")]
    [SerializeField] float jumpForce ;
    [SerializeField] float moveSpeed_horizontal ;
    [SerializeField] float moveSpeed_vertical = 0f;
    [SerializeField] bool duringJump = false;
    [SerializeField] bool can_jump = false;
    [Range(0, 1)][SerializeField] float smooth_time ;
    [SerializeField] float maxFallSpeed;
    private bool isFacingRight = true;
    bool is_jumping = false;
    private bool releasejump = false;
    

    [Header("DASH")]
    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingCooldown;
    [SerializeField] private float dashingBeforeFlyCooldown;
    [SerializeField] private TrailRenderer tr;
    private Vector2 dashingdirection;
    private bool canDash = true;
    private bool isDashing;
    


    [Header("VOL")]
    [SerializeField] bool is_flying = false;
    [SerializeField] bool aerial = false;
    [SerializeField] bool MadeAFly = false;
    [SerializeField] float speedverti;
    [SerializeField] float speedhori;
    [Range(0, 15)][SerializeField] float gravityfall;
    [SerializeField] float Timerseconde;
    private float smoothdash = 0.06f;
    bool Govol = false;
    bool flyactivated = false;


    [Header("DOUBLE JUMP")]
    [SerializeField] float jumpForceAerial;
    [SerializeField] int CountJump;
    private int LastPressedJumpTime = 0;
    private int LastOnGroundTime = 0;
    private bool IsGrounded = false;
    


    
    




    /*[Header("test wall slide")]
    [SerializeField] private float wallSlidingSpeed ;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    bool isWallSliding;*/

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animController = GetComponent<Animator>();
        
    }

    void Update()
    {
        horizontal_value = Input.GetAxis("Horizontal");
        vertical_value = Input.GetAxis("Vertical");

     

        Flip();
       // WallSlide();
       
        
        animController.SetFloat("Speed", Mathf.Abs(horizontal_value));
   
    
        //Jump = à la touche A et Y
        if (Input.GetButton("Jump") && can_jump)
        {
            
            is_jumping = true;
            animController.SetBool("Jumping", true);
           
        }
        if(Input.GetButtonUp("Jump"))
        {
            
            
            releasejump = true;
        }
        

        if (Input.GetButtonDown("Jump") && aerial && CountJump > 0)
        {
            releasejump = false;
            PhysicDoubleJump();

        }

        
      
        // Fire1 = à la touche X
        if (Input.GetButton("Fire1") && is_flying == false && aerial)
        {
            
            if (MadeAFly) { }
            else
            {
                flyactivated = true;
                isDashing = true;
                canDash = false;
                tr.emitting = true;
                dashingdirection = new Vector2(horizontal_value, vertical_value);
               
                if (dashingdirection == Vector2.zero)
                {
                    dashingdirection = new Vector2(transform.localScale.x, y: 0);
                }
                releasejump = false;
                
                Invoke("Vol", 0.25f);
                duringJump = false;
                Invoke("ChuteVol", Timerseconde);
                StartCoroutine(StopDashing());

      
            }
        }
        
        

        // Fire 2 = a la touche B
        if (Input.GetButtonDown("Fire2") && canDash)
        {
            
            isDashing = true;
            canDash = false;
            tr.emitting = true;
            dashingdirection = new Vector2(horizontal_value, vertical_value);
            if(dashingdirection == Vector2.zero)
            {
                dashingdirection = new Vector2(transform.localScale.x, y:0);
            }
            StartCoroutine(StopDashing());
            
        }

        if(isDashing)
        {
            rb.velocity = dashingdirection.normalized * dashingPower;
            return;
        }
        
       
    }

  

    void FixedUpdate()
    {
        if (is_jumping && can_jump)
        {           
            is_jumping = false;
            duringJump = true;
            aerial = true;
            
            
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            
            can_jump = false;
        }
        
        
       
        if (releasejump && duringJump && isDashing == false && is_flying == false)
        {
            
            rb.AddForce(new Vector2(0, -50), ForceMode2D.Force);
        }

      
      
        if(rb.velocity.y < 0 && is_flying == false)
        {
            
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, maxFallSpeed));
        }


        
        // flip flop pour couper le rb velocity du premier vector qui bloque les deplacements sur le y lors du vol
        if (is_flying == false)
        {
            Vector2 target_velocity = new Vector2(horizontal_value * moveSpeed_horizontal * Time.fixedDeltaTime, rb.velocity.y);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocity, ref ref_velocity, smooth_time);
        }
        else if (is_flying)
        {
            Vector2 target_velocitydash = new Vector2(horizontal_value * moveSpeed_horizontal * Time.fixedDeltaTime, vertical_value * moveSpeed_vertical * Time.fixedDeltaTime);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocitydash, ref ref_velocity, smoothdash);
            
        }
        //BIG GROS
       /* if(horizontal_value != 0 && is_flying && vertical_value ==0)
        {
            rb.AddForce(new Vector2(15, 50f));
            
            
        }*/

        //Stopper le vol si le player stop les mouvements pour eviter de flotter immobile
         if ((horizontal_value == 0 && vertical_value ==0) && is_flying)
         {
            
             ChuteVol();
         }
          
    } //FIN DU FIXUPDATE


    // Lorsque le personnage touche le sol
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        MadeAFly = false;
        releasejump = false;
        can_jump = true;
        aerial = false;
        canDash = true;
        rb.gravityScale = 4f;
        moveSpeed_horizontal = 720f;
        animController.SetBool("Jumping", false);
        CountJump = 1; //reset double saut quand on touche le sol
        
        
    }

    #region ENUM VOID

    private IEnumerator StopDashing()
    {
        if (flyactivated)
        {
            yield return new WaitForSeconds(dashingBeforeFlyCooldown);
            
            isDashing = false;
            Govol = true;
        }
        else
        {
            yield return new WaitForSeconds(dashingCooldown);
            if (is_flying == false)
            {
                tr.emitting = false;
            }
            isDashing = false;
            Govol = true;
        }
    }


    private void Vol()
    {
        MadeAFly = true;
        is_flying = true;
        animController.SetBool("Aerial", true);
        animController.SetBool("Jumping", false);
        rb.gravityScale = 0;
        moveSpeed_vertical = speedverti;
        moveSpeed_horizontal = speedhori;
        
        
    }
    private void ChuteVol()
    {
            is_flying = false;
            tr.emitting = false;
            Govol = false;
            animController.SetBool("Aerial", false);
        
            moveSpeed_vertical = 400f;
            moveSpeed_horizontal = 550f;
            rb.gravityScale = gravityfall;

    }


    void PhysicDoubleJump()
    {
        // Garantit que nous ne pouvons pas appeler Jump plusieurs fois à partir d'une seule pression
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        CountJump -= 1;

        // On augmente la force appliquée si on tombe
        // Cela signifie que nous aurons toujours l'impression de sauter le même montant
        float force = jumpForceAerial;
        if (rb.velocity.y < 0)
            force -= rb.velocity.y;


        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);

    }
    private void Flip()
    {
        if (isFacingRight && horizontal_value < 0f || !isFacingRight && horizontal_value > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    /* #region WALLSLIDE
   bool IsWalled()
   {
       return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
   }

       void WallSlide()
   {
       if (IsWalled() && !can_jump && horizontal_value != 0f)
       {
           isWallSliding = true;
           rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));

       }
       else
       {
           isWallSliding = false;
       }
   }
   #endregion */
    #endregion

}








/*IEnumerator Dash()
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
   }*/
// Flip Du Sprite selon la direction
/*if (horizontal_value > 0)
{
   // sr.flipX = false;


}
else if (horizontal_value < 0)
{
   // sr.flipX = true;


}*/

// Co routine pour modifier les variables necessaires, couper l'anim de jump et changer la gravité et les controles pendant 1.2s puis remettre ce qu'il faut
/*IEnumerator DashPropulsion()
{
    is_dashing = true;
    animController.SetBool("Aerial", true);
    animController.SetBool("Jumping", false);
    rb.gravityScale = 0;
    moveSpeed_vertical = speedverti;
    moveSpeed_horizontal = speedhori;

        yield return new WaitForSeconds(Timerseconde);

    is_dashing = false;
    animController.SetBool("Aerial", false);

    moveSpeed_vertical = 400f;
    moveSpeed_horizontal = 550f;
    rb.gravityScale = gravityfall;
}*/