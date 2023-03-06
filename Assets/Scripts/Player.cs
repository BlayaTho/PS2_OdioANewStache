using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
     public FlyAndDash flyDash;
    public CheckController checkC;
    
     public Rigidbody2D rb;
    SpriteRenderer sr;
    public Animator animController;
    [HideInInspector] public float horizontal_value;
    [HideInInspector] public float vertical_value;
    
    Vector2 ref_velocity = Vector2.zero;

    #region MOVEMENT VAR
    [Header("MOVEMENT")]
    [SerializeField] float jumpForce ;
    [SerializeField] public float moveSpeed_horizontal ;
    [HideInInspector] public float moveSpeed_vertical = 0f;
    [SerializeField] public bool duringJump = false;
    [SerializeField] public bool can_jump = false;
    [Range(0, 1)][SerializeField] float smooth_time ;
    [SerializeField] float maxFallSpeed;
    private bool is_jumping = false;
    [HideInInspector] public bool releasejump = false;
    private bool isFacingRight = true;
    #endregion

    #region HEALTHBAR
    [Header("STAMINA")]
    public int maxHealth;
    public int currentHealth;
    public HealthBar healthBar;
    [SerializeField] private float regenvie;
    [SerializeField] private float regenvieReset;
    #endregion

    /* #region DOUBLE JUMP VAR
     [Header("DOUBLE JUMP")]
     [SerializeField] float jumpForceAerial;
     [SerializeField] int CountJump;
     private int LastPressedJumpTime = 0;
     private int LastOnGroundTime = 0;
     private bool IsGrounded = false;
     #endregion*/


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animController = GetComponent<Animator>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        
    }

    void Update()
    {
        
        
        #region CAPTURE DONNEES
        horizontal_value = Input.GetAxis("Horizontal");
        vertical_value = Input.GetAxis("Vertical");
        if(horizontal_value < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
        // Flip();
        animController.SetFloat("Speed", Mathf.Abs(horizontal_value));
        #endregion

        #region JUMP IF
        //Jump = à la touche X et Y, DUALSENSE = croix et triangle
        if (Input.GetButton(checkC.inputJump) && can_jump)
        {
            is_jumping = true;
            animController.SetBool("Jumping", true);
        }
        // active la variable qui annonce qu'on a laché le bouton jump
        if(Input.GetButtonUp(checkC.inputJump)) releasejump = true;

        /* //Active DoubleJump si le personnage est en l'air et si il lui reste des double jump
         if (Input.GetButtonDown("Jump") && duringJump && CountJump > 0)
         {
             releasejump = false;
             PhysicDoubleJump();
         }*/
        #endregion

        if (currentHealth == 0)
        {
            SceneManager.LoadScene("Alpha 1.0");
        }
        if(currentHealth < maxHealth)
        {
            regenvie -= Time.deltaTime;
         
            if(regenvie < 0)
            {
                regenvie = 0;
            }
            if (regenvie == 0)
            {
                currentHealth += 1;
                regenvie = regenvieReset;
                healthBar.SetHealth(currentHealth);
            }
        }
    }



    void FixedUpdate()
    {
        //le jump avec un addforce
        if (is_jumping && can_jump)
        {           
            is_jumping = false;
            duringJump = true;
            flyDash.aerial = true;
            
            
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            
            can_jump = false;
        }
        
        
       // Higher gravity when falling of a jump
        if (releasejump && duringJump && flyDash.isDashing == false && flyDash.is_flying == false)
        {
            
            rb.AddForce(new Vector2(0, -50), ForceMode2D.Force);
        }

      
      // Vitesse de chute limité 
        if(rb.velocity.y < 0 && flyDash.is_flying == false)
        {
            
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, maxFallSpeed));
        }

        #region DEPLACEMENT DE BASE AVEC ET SANS VOL
        // flip flop pour couper le rb velocity du premier vector qui bloque les deplacements sur le y lors du vol
        if (flyDash.is_flying == false)
        {
            Vector2 target_velocity = new Vector2(horizontal_value * moveSpeed_horizontal * Time.fixedDeltaTime, rb.velocity.y);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocity, ref ref_velocity, smooth_time);
        }
        else if (flyDash.is_flying)
        {
            Vector2 target_velocitydash = new Vector2(horizontal_value * moveSpeed_horizontal * Time.fixedDeltaTime, vertical_value * moveSpeed_vertical * Time.fixedDeltaTime);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocitydash, ref ref_velocity, flyDash.smoothFly);
            
        }
        #endregion

    } //FIN DU FIXUPDATE


    // Lorsque le personnage touche le sol
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            if (flyDash.is_flying == false)
            {
            flyDash.RegenBar();
            flyDash.MadeAFly = false;
            duringJump = false;
            releasejump = false;
            can_jump = true;
            flyDash.aerial = false;
            flyDash.canDash = true;
            rb.gravityScale = 4f;
            moveSpeed_horizontal = 720f;
            animController.SetBool("Jumping", false);
            }
            //if (flyDash.is_flying == true) flyDash.ChuteVol();
           // CountJump = 1; //reset double saut quand on touche le sol
            
        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        flyDash.aerial = true;
        animController.SetBool("Jumping", true);
        can_jump = false;
    }

    #region ENUM VOID
  
    public void TakeDamage(int damage)
    {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);      
    }

    /* void PhysicDoubleJump()
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

   }*/

    /*private void Flip()
    {
        if (isFacingRight && horizontal_value < 0f || !isFacingRight && horizontal_value > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }*/
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