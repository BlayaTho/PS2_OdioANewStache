using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAndDash : MonoBehaviour
{
     public Player player;
    #region VOL VAR
    [Header("VOL")]
    [SerializeField] public bool is_flying = false;
    [SerializeField] public bool aerial = false;
    [SerializeField] public bool MadeAFly = false;
    [SerializeField] public float speedVertFly;
    [SerializeField] public float speedHoriFly;
    [SerializeField] public float timerStamina;
    [Range(0, 15)][SerializeField] float gravityfall;
    [HideInInspector] public float smoothFly = 0.06f;
    private bool Govol = false;
    private bool flyactivated = false;


    [Header("STAMINA")]
    public float maxStamina;
    public float currentStamina;
    [SerializeField] StaminaBar staminaBar;
    //private bool Recharging = false;
    #endregion

    #region DASH VAR
    [Header("DASH")]
    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingCooldown;
    [SerializeField] private float dashingBeforeFlyCooldown;
    private Vector2 dashingdirection;
    [HideInInspector] public bool canDash = true;
    [HideInInspector] public bool isDashing = false;
    [SerializeField] private float DashingPowerDash;
    [SerializeField] private float dashingPowerOriginal;
    [SerializeField] public TrailRenderer tr;
    #endregion

    void Start()
    {
        maxStamina = timerStamina;
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    
    void Update()
    {
        #region VOL IF
        // Fire1 = à la touche X, DUALSENSE = rond
        if (Input.GetButton("Fire1") && is_flying == false && aerial && currentStamina == maxStamina && MadeAFly == false)
        {
            dashingPower = dashingPowerOriginal;
            is_flying = true;
            flyactivated = true;
            isDashing = true;
            canDash = false;
            tr.emitting = true;
            dashingdirection = new Vector2(player.horizontal_value, player.vertical_value);

            if (dashingdirection == Vector2.zero)
            {
                dashingdirection = new Vector2(transform.localScale.x, y: 0);
            }
            player.releasejump = false;
            Invoke("Vol", 0.25f);
            player.duringJump = false;
            StartCoroutine(StopDashing());
        }
        // On commence a consumer la barre
        if (is_flying && currentStamina > 0)
        {
            StartBar();
        }
        // lorsque la barre est à 0 on lance la fin du vol
        if (currentStamina == 0)
        {
            ChuteVol();
        }
        #endregion

        #region DASH IF
        // Fire 2 = a la touche B, DUALSENSE = croix
        if (Input.GetButtonDown("Fire2") && canDash)
        {
            
            dashingPower = DashingPowerDash;
            isDashing = true;
            canDash = false;
            tr.emitting = true;
            dashingdirection = new Vector2(player.horizontal_value, player.vertical_value);
            if (dashingdirection == Vector2.zero)
            {
                dashingdirection = new Vector2(transform.localScale.x, y: 0);
            }
            StartCoroutine(StopDashing());

        }

        if (isDashing)
        {
            player.rb.velocity = dashingdirection.normalized * dashingPower;
            return;
        }

        #endregion
    }

    private void FixedUpdate()
    {
        //Stopper le vol si le player stop les mouvements pour eviter de flotter immobile
        if ((player.horizontal_value == 0 && player.vertical_value == 0) && is_flying)
        {

            ChuteVol();
        }
    }

    #region FONCTION ENUM
    #region Bar
    void StartBar()
    {
        timerStamina -= Time.deltaTime;

        if (timerStamina > 0)
        {
            currentStamina -= Time.deltaTime;
            staminaBar.SetStamina(currentStamina);
        }
        if (timerStamina < 0)
        {
            timerStamina = 0;
            currentStamina = 0;
        }
    }
    public void RegenBar()
    {
        timerStamina = 4;
        if (currentStamina < maxStamina)
        {
            currentStamina += Time.deltaTime * 2.5f;
            staminaBar.SetStamina(currentStamina);
        }
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
    }
#endregion

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


    public void Vol()
    {
        MadeAFly = true;
        is_flying = true;
        //animController.SetBool("Aerial", true);
        // animController.SetBool("Jumping", false);
        player.rb.gravityScale = 0;
        player.moveSpeed_vertical = speedVertFly;
        player.moveSpeed_horizontal = speedHoriFly;
    }
    private void ChuteVol()
    {
        is_flying = false;
        flyactivated = false;
        tr.emitting = false;
        Govol = false;
        // animController.SetBool("Aerial", false);
        player.moveSpeed_vertical = 400f;
        player.moveSpeed_horizontal = 550f;
        player.rb.gravityScale = gravityfall;

    }
#endregion
}
