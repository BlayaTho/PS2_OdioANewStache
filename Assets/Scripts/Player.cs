using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animController;
    [SerializeField] float horizontal_value;
    [SerializeField] float vertical_value;
    Vector2 ref_velocity = Vector2.zero;

    //DEPLACEMENT 
    [SerializeField] float jumpForce ;
    [SerializeField] float moveSpeed_horizontal ;
    [SerializeField] float moveSpeed_vertical = 0f;
    [SerializeField] bool is_jumping = false;
    [SerializeField] bool can_jump = false;
    [Range(0, 1)][SerializeField] float smooth_time ;
   

    //DASH PROPULSION
    [SerializeField] bool is_dashing = false;
    [SerializeField] bool aerial = false;
    [SerializeField] bool MadeADash = false;
    

    float Timerseconde = 1.2f;

    [SerializeField] float smoothdash;
    [SerializeField] float speedverti;
    [SerializeField] float speedhori;
   
    [Range(0, 15)][SerializeField] float gravityfall;


    //DOUBLE JUMP
    [SerializeField] bool IsGrounded = false;
    [SerializeField] int CountJump;
    private int LastPressedJumpTime = 0;
    private int LastOnGroundTime = 0;
    [SerializeField] float jumpForceAerial;


    //DASH
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingTime ;
    [SerializeField] private float dashingCooldown ;
    [SerializeField] private TrailRenderer tr;



    //test
    private int Vol = 0;
    


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

        // Flip Du Sprite selon la direction
        if (horizontal_value > 0) sr.flipX = false;
        else if (horizontal_value < 0) sr.flipX = true;


        if (isDashing)
        {
            return ;
        }
        
        animController.SetFloat("Speed", Mathf.Abs(horizontal_value));
   
        //Jump = à la touche A et Y
        if (Input.GetButtonDown("Jump") && can_jump)
        {
            is_jumping = true;
            animController.SetBool("Jumping", true);
        }

        if (Input.GetButtonDown("Jump") && aerial && CountJump > 0)
        {
            PhysicDoubleJump();

        }

      
        // Fire1 = à la touche X
        if (Input.GetButton("Fire1") && is_dashing == false && aerial)
        {
            //StartCoroutine(DashPropulsion());
            if (MadeADash)
            {

            }
            else
            {
                DashPropulsion2();
                Invoke("ChuteDashPropulsion", Timerseconde);
            }
        }
        
        // Fire 2 = a la touche B
        if (Input.GetButtonDown("Fire2") && canDash)
        {
            StartCoroutine(Dash());
            
        }
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


    void FixedUpdate()
    {
        if (is_jumping && can_jump)
        {           
            is_jumping = false;
            aerial = true;
            
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            can_jump = false;
        }

        if (isDashing)
        {
            return;
        }
        
        // flip flop pour couper le rb velocity du premier vector qui bloque les deplacements sur le y lors du dash
        if (is_dashing == false)
        {
            Vector2 target_velocity = new Vector2(horizontal_value * moveSpeed_horizontal * Time.fixedDeltaTime, rb.velocity.y);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocity, ref ref_velocity, smooth_time);
        }
        else if (is_dashing)
        {
            Vector2 target_velocitydash = new Vector2(horizontal_value * moveSpeed_horizontal * Time.fixedDeltaTime, vertical_value * moveSpeed_vertical * Time.fixedDeltaTime);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocitydash, ref ref_velocity, smoothdash);
            
        }

        //Stopper le dashProp si le player stop les mouvements pour eviter de flotter immobile
         if ((horizontal_value == 0 && vertical_value ==0) && is_dashing)
         {
            Debug.Log("Beh");
             ChuteDashPropulsion();
         }
         
         
        
        
    }

    // Lorsque le personnage touche le sol
    private void OnTriggerStay2D(Collider2D collision)
    {
        MadeADash = false;
        can_jump = true;
        aerial = false;
        rb.gravityScale = 4f;
        moveSpeed_horizontal = 500f;
        animController.SetBool("Jumping", false);
        CountJump = 1; //reset double saut quand on touche le sol
        
        Timerseconde = 1.2f;
    }
    
 

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

   
    
    private void DashPropulsion2()
    {
        MadeADash = true;
        is_dashing = true;
        animController.SetBool("Aerial", true);
        animController.SetBool("Jumping", false);
        rb.gravityScale = 0;
        moveSpeed_vertical = speedverti;
        moveSpeed_horizontal = speedhori;
        
    }
    private void ChuteDashPropulsion()
    {
        
            is_dashing = false;
            animController.SetBool("Aerial", false);

            moveSpeed_vertical = 400f;
            moveSpeed_horizontal = 550f;
            rb.gravityScale = gravityfall;
            
        

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