using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animController;
    float horizontal_value;
    float vertical_value;
    Vector2 ref_velocity = Vector2.zero;

    [SerializeField] float jumpForce = 12f;
    [SerializeField] float moveSpeed_horizontal = 400.0f;
    [SerializeField] float moveSpeed_vertical = 0f;
    [SerializeField] bool is_jumping = false;
    [SerializeField] bool is_dashing = false;
    [SerializeField] bool aerial = false;
    [SerializeField] bool can_jump = false;
    [Range(0, 1)][SerializeField] float smooth_time = 0.5f;
    [Range(0, 15)][SerializeField] float gravityfall;

    [SerializeField] float smoothdash;

    [SerializeField] float speedverti;
    [SerializeField] float speedhori;

    [SerializeField] int fallGravityMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animController = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal_value = Input.GetAxis("Horizontal");
        vertical_value = Input.GetAxis("Vertical");

        if (horizontal_value > 0) sr.flipX = false;
        else if (horizontal_value < 0) sr.flipX = true;
        
        animController.SetFloat("Speed", Mathf.Abs(horizontal_value));
   
        if (Input.GetButtonDown("Jump") && can_jump)
        {
            is_jumping = true;
            animController.SetBool("Jumping", true);
        }

        // Appuyer sur la touche ctrl gauche pour lancer la co routine si le personnage ne dahs pas deja et est en l'air
        if (Input.GetButtonDown("Fire1") && is_dashing == false && aerial )
        {
            StartCoroutine(DashPropulsion());
        }
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
        // flip flop pour couper le rb velocity du premier vector qui bloque les deplacements sur le y lors du dash
        if (is_dashing == false)
        {
            Vector2 target_velocity = new Vector2(horizontal_value * moveSpeed_horizontal * Time.fixedDeltaTime, rb.velocity.y);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocity, ref ref_velocity, 0.05f);
        }
        else if (is_dashing)
        {
            Vector2 target_velocitydash = new Vector2(horizontal_value * moveSpeed_horizontal * Time.fixedDeltaTime, vertical_value * moveSpeed_vertical * Time.fixedDeltaTime);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocitydash, ref ref_velocity, smoothdash);
            
        }
       /* #region
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = gravityfall * fallGravityMultiplier;
        }
        else
        {
            rb.gravityScale = gravityfall;
        }
        #endregion */
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        can_jump = true;
        aerial = false;
        rb.gravityScale = 3f;
        moveSpeed_horizontal = 400f;
        animController.SetBool("Jumping", false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        animController.SetBool("Jumping", false);        
    }

    // Co routine pour modifier les variables necessaires, couper l'anim de jump et changer la gravité et les controles pendant 1.2s puis remettre ce qu'il faut
    IEnumerator DashPropulsion()
    {
        is_dashing = true;
        animController.SetBool("Aerial", true);
        animController.SetBool("Jumping", false);
        rb.gravityScale = 0;
        moveSpeed_vertical = speedverti;
        moveSpeed_horizontal = speedhori;

        yield return new WaitForSeconds(1.2f);
        is_dashing = false;
        animController.SetBool("Aerial", false);
        moveSpeed_vertical = 0f;
        moveSpeed_vertical = 400f;
        moveSpeed_horizontal = 550f;
        rb.gravityScale = gravityfall;
    }

}